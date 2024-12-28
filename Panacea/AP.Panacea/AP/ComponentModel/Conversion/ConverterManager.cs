using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;

namespace AP.ComponentModel.Conversion
{
    /// <summary>
    /// Manages type converters.
    /// </summary>
    public sealed partial class ConverterManager : DisposableObject, IConverterManager 
    {        
        private readonly Dictionary<Key, Item> _map = new Dictionary<Key, Item>();
        public readonly object SyncRoot = new object();

        public readonly bool CanRegisterAliases;
        
        private NonGenericHelper _nonGeneric;
        private GenericHelper _generic;

        public ConverterManager(bool canRegisterAliases = true)
        {
            this.CanRegisterAliases = canRegisterAliases;

            _generic = new GenericHelper(this);
            _nonGeneric = new NonGenericHelper(this);
    
            if (!Converters.HasManager)
                Converters.Manager = this;              
        }

        public GenericHelper Generic { get { return _generic; } }
        public NonGenericHelper NonGeneric { get { return _nonGeneric; } }
                
        #region Register

        /// <summary>
        /// Registers a converter.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public void Register(Converter converter)
        {
            if (converter == null)
                throw new ArgumentNullException("converter");

            Key key = this.CreateKey(converter);
            Item item = this.GetItem(key, false);

            if (item != null)
            {
                if (item.IsGenerated)
                    this.Release(item.Key);                
                else
                    throw new InvalidOperationException("A converter for the certain types is already registered");
            }            
            _map.Add(key, new Item(key, converter, converter is ILinkedConverter));
        }

        #endregion

        #region Helper Methods
                
        private Key CreateKey<TInput, TOutput>()
        {
            return this.CreateKey(typeof(TInput), typeof(TOutput));
        }

        private Key CreateKey(Converter converter)
        {
            return this.CreateKey(converter.InputType, converter.OutputType);
        }

        private Key CreateKey<TInput, TOutput>(Converter<TInput, TOutput> converter)
        {
            if (converter != null)
                return this.CreateKey(converter);

            return this.CreateKey<TInput, TOutput>();
        }

        private Key CreateKey(Type inputType, Type outputType)
        {
            return new Key(inputType, outputType);
        }

        #endregion

        #region Search Methods

        public bool Contains(Converter converter)
        {
            return converter.Equals(this.GetConverter(this.CreateKey(converter), false, false));                
        }

        private Converter CreateLinkedConverter(Type inputType, Type outputType)
        {
            // patched to work with item (changed the cast and added the select clause)
            AP.Collections.List<Converter> converters = new AP.Collections.List<Converter>(_map.Values.Select(p => p.Converter));

            // test if it's possible to build a chained converter
            bool tryLinking = false;
            foreach (Converter c in converters)
            {
                if (!tryLinking && (c.InputType.IsAssignableFrom(inputType) || c.OutputType.IsAssignableFrom(outputType)))
                {
                    tryLinking = true;
                    break;
                }
            }
            if (!tryLinking)
                return null;

            HashSet<Converter> results = new HashSet<Converter>();

            bool done = false;
            //bool found = false;
            //HashSet<Edge> visited0 = new HashSet<Edge>();   
            foreach (Converter c0 in converters)
            {
                // is the original target the source?
                if (inputType.IsSubclassOf(c0.InputType) && !results.Contains(c0))
                {
                    // then follow
                    //Edge current = new Converter(e0.OutputType, e0.InputType);
                    Type currentOutputType = c0.InputType;

                    foreach (Converter c1 in converters)
                    {
                        if (currentOutputType.IsAssignableFrom(c1.InputType))
                        {
                            results.Add(c1);

                            if (results.Count > 0)
                            {
                                foreach (Converter c2 in converters)
                                {
                                    // todo: think about type covariance
                                    if (c2.InputType.IsAssignableFrom(currentOutputType) && c2.OutputType.IsSubclassOf(outputType))
                                    {
                                        results.Add(c2);

                                        done = true;
                                        break;
                                    }
                                }

                            }
                            currentOutputType = c1.OutputType;
                        }
                        if (done)
                            break;
                    }
                    if (done)
                        break;
                }
            }
            if (results.Count > 0)
                return (Converter)Activator.CreateInstance(typeof(LinkedConverter<,>).MakeGenericType(inputType, outputType), results);
            
            return null;
        }

        private Converter GetConverter(Key key, bool advancedSearch = true, bool createAndRegisterAlternatives = true)
        {
            Item item = this.GetItem(key, advancedSearch);

            if (item != null)
            {
                // test if the keys are equal (tests if the advancedSearch was used)
                if (advancedSearch && createAndRegisterAlternatives && !key.Equals(item.Key))
                {                    
                    Item alias = new Item(key, item.Converter, true);                
                        
                    lock (this.SyncRoot)
                        _map.Add(key, alias);                                            
                }
                
                return item.Converter;
            }

            // the advanced search failed - so try to build a generated converter
            if (createAndRegisterAlternatives)
            {
                Converter linked = this.CreateLinkedConverter(key.InputType, key.OutputType);

                if (linked != null)
                {                    
                    lock (this.SyncRoot)
                        _map.Add(key, new Item(key, linked, true));

                    return linked;
                }
            }

            return null;
        }

        private Item GetItem(Key key, bool advancedSearch = true)
        {
            // create a key
            Item found = null;
            
            if (_map.TryGetValue(key, out found) || !advancedSearch)
                return found;

            // iterate and narrow down the search - holy crap it#s hot here >_<
            // need input..... beep beep brain malfunctionüühhn.g

            // and the winner is... 
            foreach (KeyValuePair<Key, Item> kvp in _map)
            {
                Key k = kvp.Key;
                
                Type it = k.InputType;
                Type ot = k.OutputType;

                if (it.IsAssignableFrom(key.InputType) && key.OutputType.IsAssignableFrom(ot))
                {
                    found = kvp.Value;
                    break;
                }
            }

            return found;
        }

        public Converter GetConverter(Type inputType, Type outputType)
        {
            return this.GetConverter(this.CreateKey(inputType, outputType));
        }

        public Converter<TInput, TOutput> GetConverter<TInput, TOutput>()
        {
            return (Converter<TInput, TOutput>)this.GetConverter(this.CreateKey<TInput, TOutput>(), true);
        }
        
        #endregion

        #region Release Members
               
        private void Release(Key key, Item item = null)
        {
            Item tmp = _map[key];

            // if the converter is managed somewhere else, ignore it.
            if (item != null && !item.Converter.Equals(tmp.Converter))
                return;

            item = tmp;
    
            lock (this.SyncRoot)
                _map.Remove(key);

            // now find and remove all possible aliases - no longer needed - there are no more aliases!
            Converter converter = item.Converter;
            Type inputType = converter.InputType;
            Type outputType = converter.OutputType;
            
            // find all generated converters (ILinkedConverter) or aliases
            IEnumerator en = _map.Values.GetEnumerator();

            while (en.MoveNext())
            {                  
                Item currentItem = (Item)en.Current;
                    
                if (currentItem.IsGenerated)
                {
                    bool removeCurrent = false;
                        
                    if (converter == currentItem.Converter)
                    {
                        removeCurrent = true;
                    }
                    else if (converter is ILinkedConverter)
                    {
                        // if one brick is being removed the whole generated converter isn't going to work -> remove it
                        removeCurrent = ((ILinkedConverter)converter).Converters.Contains(converter);
                    }

                    if (removeCurrent)
                    {
                        lock (SyncRoot)
                            _map.Remove(currentItem.Key);

                        // get a new enumerator otherwise the stack will corrupt
                        en = _map.Values.GetEnumerator();
                    }
                }                    
            }        
        }
        
        public void Release(Converter converter)
        {
            Key key = this.CreateKey(converter);

            this.Release(key, new Item(key, converter, false));
        }

        public void Clear()
        {
            _map.Clear();
        }

        #endregion

        #region IDisposable Members
  
        protected override void CleanUpResources()
        {
            base.CleanUpResources();

            if (Converters.HasManager && Converters.Manager == this)
                Converters.Dispose(true);

            this.Clear();

            _generic = null;
            _nonGeneric = null;
        }

        #endregion
    }
}