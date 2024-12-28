using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.ComponentModel;

namespace AP.ComponentModel.Conversion
{
    public abstract class Converters : StaticType
    {
        private static IConverterManager _current;
        public static readonly object SyncRoot = new object();

        protected Converters()
            : base()
        { }

        public static IConverterManager Manager
        {
            get
            {
                return _current;
            }
            set
            {
                lock (SyncRoot)
                {
                    if (HasManager)
                        throw new InvalidOperationException("Mapper is already set, use dispose first");

                    _current = value;
                }
            }
        }

        public static bool HasManager
        {
            get
            {
                lock (SyncRoot) 
                    return _current != null;
            }
        }

        public static GenericHelper Generic { get { return Manager.Generic; } }
        public static NonGenericHelper NonGeneric { get { return Manager.NonGeneric; } }

        private static ExtendedTypeDescriptionProvider _extendedTypeDescriptionProvider;
        private static TypeDescriptionProvider _originalTypeDescriptionProvider;

        public static bool UseExtendedTypeConverters
        {
            get
            {
                return _extendedTypeDescriptionProvider != null;
            }
            set
            {                
                if (UseExtendedTypeConverters != value)
                {
                    Type t = typeof(object);

                    lock (SyncRoot)
                    {
                        if (value)
                        {
                            TypeDescriptionProvider original = TypeDescriptor.GetProvider(t);

                            lock (original)
                            {
                                ExtendedTypeDescriptionProvider extended = new ExtendedTypeDescriptionProvider(original);

                                lock (extended)
                                {
                                    TypeDescriptor.RemoveProvider(original, t);
                                    TypeDescriptor.AddProvider(extended, t);

                                    _extendedTypeDescriptionProvider = extended;
                                    _originalTypeDescriptionProvider = original;
                                }
                            }
                        }
                        else
                        {
                            lock (_originalTypeDescriptionProvider)
                            lock (_extendedTypeDescriptionProvider)                            
                            {
                                TypeDescriptor.RemoveProvider(_extendedTypeDescriptionProvider, t);
                                TypeDescriptor.AddProvider(_originalTypeDescriptionProvider, t);

                                _extendedTypeDescriptionProvider = null;
                                _originalTypeDescriptionProvider = null;
                            }
                        }
                    }
                }
            }
        }

        public static void Dispose(bool disposeManager = true)
        {
            lock (SyncRoot)
            {
                UseExtendedTypeConverters = false;
                IConverterManager current = _current;
                _current = null;
                
                if (disposeManager)
                    current.Dispose();                
            }                        
        }

        public static Converter GetConverter(Type inputType, Type outputType)
        {
            if (HasManager)
            {
                Converter c = Manager.GetConverter(inputType, outputType);
                if (c != null)
                    return c;
            }

            if (UseExtendedTypeConverters)
            {
                TypeConverter typeConverter = _originalTypeDescriptionProvider.GetTypeDescriptor(inputType).GetConverter();

                if (typeConverter.CanConvertFrom(inputType))
                    return (Converter)New.Instance(typeof(TypeConverterWrapper<,>).MakeGenericType(inputType, outputType), typeConverter, UsedTypeConverterMethod.From);

                typeConverter = _originalTypeDescriptionProvider.GetTypeDescriptor(outputType).GetConverter();

                if (typeConverter.CanConvertTo(outputType))
                    return (Converter)New.Instance(typeof(TypeConverterWrapper<,>).MakeGenericType(inputType, outputType), typeConverter, UsedTypeConverterMethod.To);                
            }

            return null;
        }

        public static Converter<TInput, TOutput> GetConverter<TInput, TOutput>()
        {
            if (HasManager)
            {
                Converter<TInput, TOutput> c = Manager.GetConverter<TInput, TOutput>();                
                if (c != null)
                    return c;
            }

            if (UseExtendedTypeConverters)
            {
                Type inputType = typeof(TInput);
                Type outputType = typeof(TOutput);

                TypeConverter typeConverter = _originalTypeDescriptionProvider.GetTypeDescriptor(inputType).GetConverter();

                if (typeConverter.CanConvertFrom(inputType))
                    return new TypeConverterWrapper<TInput, TOutput>(typeConverter, UsedTypeConverterMethod.From);

                typeConverter = _originalTypeDescriptionProvider.GetTypeDescriptor(outputType).GetConverter();

                if (typeConverter.CanConvertTo(outputType))
                    return new TypeConverterWrapper<TInput, TOutput>(typeConverter, UsedTypeConverterMethod.To);
            }

            return null;
        }

        public static void Register(Converter converter)
        {
            lock (SyncRoot)
                _current.Register(converter);
        }

        public static void Release(Converter converter)
        {
            lock (SyncRoot)
                _current.Release(converter);
        }

        public static Converter<TInput, TOutput> Register<TInput, TOutput>(TypeConverter converter, UsedTypeConverterMethod method)
        {
            Converter<TInput, TOutput> c = new TypeConverterWrapper<TInput, TOutput>(converter, method);

            _current.Register(c);

            return c;
        }
        
        public static Converter<TInput, TOutput> Register<TInput, TOutput>(Convert<TInput, TOutput> converter)
        {
            Converter<TInput, TOutput> c = new ConvertDelegateWrapper<TInput, TOutput>(converter);

            _current.Register(c);

            return c;
        }        
    }
}
