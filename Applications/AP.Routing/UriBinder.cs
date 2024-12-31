using AP.Collections;
using AP.Collections.ReadOnly;
using AP.Collections.Specialized;
using AP.Reflection;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AP.Routing
{
    public class UriBinder<TContext, TParameters> : BinderBase<TContext, TParameters>
        where TContext : RoutingContext
    {
        private readonly Converter<IDictionaryView<string, string> , TParameters> _dictionaryToParameters;
        private readonly IDictionaryView<string, Converter<string, object>> _memberValueConverters;

        public UriBinder()
        { }

        public UriBinder(Converter<IDictionaryView<string, string>, TParameters> dictionaryToParameters)
        {
            _dictionaryToParameters = dictionaryToParameters;
        }

        public UriBinder(IDictionaryView<string, Converter<string, object>> memberValueConverters)
        {            
            _memberValueConverters = memberValueConverters ?? ReadOnlyDictionary<string, Converter<string, object>>.Empty;
        }

        public override bool IsMatch(TContext context, Route<TContext, TParameters> route, out TParameters parameters)
        {
            parameters = default(TParameters);

            if (!context.HasUri)
                return false;

            string path = this.GetRelevantUriPart(context.Uri);

            UriSegmentList segments = route.UriSegments;
            int count = segments.Count;

            StringDictionary parametersDictionary = new StringDictionary(route.UriSegments.Count);

            int lastIndex = count - 1;

            StringComparison comp = StringComparison.OrdinalIgnoreCase;

            // analyze the segments
            for (int i = 0; i < count; ++i)
            {
                UriSegment currentSegment = segments[i];

                // the easy check for fixed segments
                if (currentSegment.Type == UriSegmentType.Fixed)
                {
                    FixedUriSegment currentFixed = (FixedUriSegment)currentSegment;

                    string value = currentFixed.Value;

                    if (!(path.StartsWith(value, comp)))
                        break;

                    path = path.Substring(value.Length);
                }
                else
                {
                    // need to find the next fixed element
                    int nextIndex = i + 1;

                    if (nextIndex <= lastIndex)
                    {
                        // the next one HAS to be a fixed segment 
                        FixedUriSegment nextFixedSegment = (FixedUriSegment)segments[nextIndex];

                        string nextValue = nextFixedSegment.Value;

                        int fixedStartIndex = path.IndexOf(nextValue, comp);

                        // wasn't the right part - also there has to be "some" space between - so check if the index is bigger 1 and not 0
                        // also: if it's the last segment there cannot be any template left that'd use up the last uri part
                        if (fixedStartIndex < 1 || (nextIndex == lastIndex && fixedStartIndex + nextValue.Length < path.Length))
                            break;

                        // template segment - add the value and skip the next fixed element.
                        string name = ((TemplateUriSegment)currentSegment).Name;
                        string value = path.Substring(0, fixedStartIndex);
                        i++;
                        
                        parametersDictionary.Add(name, value);

                        // skip the next one and remove the left part of the path
                        path = path.Remove(0, fixedStartIndex + nextValue.Length);
                    }
                    else // it is the last element and a template
                    {
                        string name = ((TemplateUriSegment)currentSegment).Name;
                        parametersDictionary.Add(name, path);

                        // "clear" the path - it's no longer needed
                        path = null;
                    }
                }

                // it should be the right route - path does no longer hold any valuable information
                if (path.IsNullOrWhiteSpace())
                {
                    // there might be a last template left, if the current already is a template, it's safe to assume it's not the matching route.
                    if (currentSegment.Type == UriSegmentType.Template && i + 1 < lastIndex)
                        return false;

                    // is it safe to assume the value of the string should be null or empty - which one should it be?
                    // it probably depends on how the conversion will work                    
                    if (i < lastIndex)
                    {
                        string name = ((TemplateUriSegment)segments[lastIndex]).Name;
                        string value = null;

                        parametersDictionary.Add(name, value);
                    }
                    
                    // finally convert the parameters
                    parameters = this.ConvertToParameters(parametersDictionary);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Extracts the relevant part of the Uri for routing purposes.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns>A string consisting only of the used part of the uri. When the uri is a http url the part will consist of path, query and fragments.</returns>
        protected virtual string GetRelevantUriPart(IUri uri)
        {
            string part = string.Empty;

            if (uri is IHierarchicalUri)
                part = ((IHierarchicalUri)uri).Path;
            
            if (uri is IQueryableUri)
                part += ((IQueryableUri)uri).Query.Value;

            if (uri is IFragmentableUri)
                part += ((IFragmentableUri)uri).Fragments.Value;
            
            if (part == string.Empty)
                return uri.FullName;

            return part;
        }

        /// <summary>
        /// Converts the dictionary to the actual parameters object.
        /// When the dictionaryToParameters converter was set, it will be used instead of the default implementation.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>Returns the parameters.</returns>
        protected virtual TParameters ConvertToParameters(IDictionaryView<string, string> dictionary)
        {
            if (_dictionaryToParameters != null)
                return _dictionaryToParameters(dictionary);

            TParameters parameters = New.OrUninitialized<TParameters>();
                        
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.GetField | BindingFlags.GetProperty;

            foreach (KeyValuePair<string, string> kvp in dictionary)
            {
                IEnumerable<MemberInfo> members = Reflect.GetMembers<TParameters>(flags);

                foreach (MemberInfo member in members)
                {
                    // compare the keys and set the member
                    if (member.Name.Equals(kvp.Key, StringComparison.Ordinal))
                    {
                        Delegate setter = null;

                        if (member.MemberType == MemberTypes.Property)
                            setter = ((PropertyInfo)member).CreateSetterDelegate();
                        else if (member.MemberType == MemberTypes.Field)
                            setter = ((FieldInfo)member).CreateSetterDelegate();
                        else
                            throw new InvalidOperationException(string.Format("member {0} is not a property or field.", member.Name));

                        // get the converter
                        Converter<string, object> converter = this.GetMemberConverter(member);
                        setter.DynamicInvoke(parameters, converter(kvp.Value));
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        /// Gets the converter for the specified field or property.        
        /// </summary>
        /// <param name="member">The field or property.</param>
        /// <returns>Returns either a converter found in the memberConverters dictionary or the TypeDescriptor.GetConverter.ConvertFromString method as a delegate.</returns>
        protected virtual Converter<string, object> GetMemberConverter(MemberInfo member)
        {
            if (member.MemberType != MemberTypes.Field && member.MemberType != MemberTypes.Property)
                throw new ArgumentException(string.Format("member {0} is not a field or property."));

            Converter<string, object> converter;

            if (!_memberValueConverters.TryGetValue(member.Name, out converter))
            {
                AP.ComponentModel.Conversion.Converter c = null;
                Type t = member.GetMemberType();

                if (AP.ComponentModel.Conversion.Converters.UseExtendedTypeConverters || (AP.ComponentModel.Conversion.Converters.HasManager && (c = AP.ComponentModel.Conversion.Converters.GetConverter(typeof(string), t)) == null))
                    converter = TypeDescriptor.GetConverter(t).ConvertFromString;
                else
                    converter = (s) => c.Convert(s);                                    
            }

            return converter;
        }
    }
}
