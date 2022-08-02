using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP;

namespace AP.ComponentModel.Validation
{
    public static class ValidationHelper
    {
        // Fields
        private static char[] startingChars = new char[] { '<', '&' };

        // Methods
        private static bool IsAtoZ(char c)
        {
            return (((c >= 'a') && (c <= 'z')) || ((c >= 'A') && (c <= 'Z')));
        }

        public static bool IsDangerousString(string s)
        {
            int startIndex = 0;
            while (true)
            {
                int index = s.IndexOfAny(startingChars, startIndex);
                if (index < 0)
                    return false;

                if (index == (s.Length - 1))
                    return false;
            
                char ch = s[index];
                if (ch != '&')
                {
                    if ((ch == '<') && ((IsAtoZ(s[index + 1]) || (s[index + 1] == '!')) || ((s[index + 1] == '/') || (s[index + 1] == '?'))))
                    {
                        return true;
                    }
                }
                else if (s[index + 1] == '#')
                    return true;

                startIndex = index + 1;
            }
        }

        /// <summary>
        /// Validates each item of a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<ValidationResult> Validate<T>(this IEnumerable<T> collection)
        {
            foreach (T c in collection)
                yield return Validate<T>(c);
        }
        
        /// <summary>
        /// Validates an object. If the object implements IValidateable'T it will use that method.
        /// Otherwise a DataAnnotationsValidator'T will be used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ValidationResult Validate<T>(this T value)
        {
            if (value is IValidateable)
                return ((IValidateable)value).Validate();
                       
            DataAnnotationsValidator<T> dav = DataAnnotationsValidator<T>.Default;

            return dav.Validate(value);
        }
    }
}
