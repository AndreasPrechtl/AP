using System;
using System.Linq;
using System.Text;
using AP;
using AP.Linq;
using AP.Collections;
using SCG = System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AP
{
    public static class Strings
    {        
        /// <summary>
        /// Tests if a string is a number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            double d;

            return double.TryParse(value, out d);
        }
        
        /// <summary>
        /// Removes all '\0' characters from a string
        /// </summary>
        /// <param name="value">The string that should be stripped</param>
        /// <returns>A string without '\0' characters</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static string RemoveNullCharacters(this string value)
        {
            return value.Replace("\0", string.Empty);
        }

        private static string[] SplitInternal(this string value, int index, StringSplitOptions options)
        {
            if (index <= 0 || index >= value.Length)
                return new string[] { value };

            string s1 = value.Remove(index);
            string s2 = value.Substring(index);

            if (options == StringSplitOptions.None)
                return new string[] { s1, s2 };

            bool b1 = s1.IsNullOrWhiteSpace();
            bool b2 = s2.IsNullOrWhiteSpace();

            if (b1 && b2)
                return new string[0];

            if (b1)
                return new string[] { s2 };
            
            return new string[] { s1 };
        }

#pragma warning disable 109        
        /// <summary>
        /// Retrieves a reference to a specified System.String.
        /// </summary>
        /// <param name="str">The string to search for in the intern pool.</param>
        /// <returns>A reference to str if it is in the common language runtime intern pool; otherwise, null.</returns>
        /// <exception cref="System.ArgumentNullException">str is null.</exception>
        public static new bool IsInterned(this string str)
#pragma warning restore 109
        {
            return string.IsInterned(str) != null;
        }

        /// <summary>
        /// Tries to get an internally stored string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetInterned(this string str, out string value)
        {
            value = string.IsInterned(str);

            if (value == null)
            {
                value = str;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a string to the intern pool.
        /// </summary>
        /// <param name="value">The string that should be added to the intern pool.</param>
        /// <returns>The string that is interned.</returns>
        public static string Intern(this string value)
        {
            return string.Intern(value);
        }
        
        //private static string[] SplitInternal1(this string value, List<int> indexes, StringSplitOptions options)
        //{
        //    int length = value.Length;
        //    int count = indexes.Count;

        //    bool keepEmptyEntries = options == StringSplitOptions.None;

        //    string[] results = new string[count + 1];

        //    StringBuilder current = new StringBuilder(length);

        //    for (int i = 0, currentIndexer = 0; i < length; i++)
        //    {
        //        if (currentIndexer < count && i == indexes[currentIndexer])
        //        {
        //            string tmp = current.ToString();
        //            if (!keepEmptyEntries || !string.IsNullOrWhiteSpace(tmp))
        //                results[currentIndexer] = tmp;

        //            current.Clear();
        //            currentIndexer++;
        //        }
        //        current.Append(value[i]);
        //    }
        //    results[count] = current.ToString();

        //    return results;
        //}

        //private static string[] SplitInternal2(string value, List<int> filteredIndexes, StringSplitOptions options)
        //{
        //    List<string> results = new List<string>(filteredIndexes.Count + 1);

        //    int last = 0;
        //    bool keepEmptyEntries = options == StringSplitOptions.RemoveEmptyEntries;

        //    foreach (int current in filteredIndexes)
        //    {
        //        string s = value.Substring(last, current - last);

        //        if (!keepEmptyEntries || !string.IsNullOrEmpty(s))
        //            results.Add(s);

        //        last = current;
        //    }

        //    // add the last remaining string to the results
        //    string end = value.Substring(last);

        //    if (!keepEmptyEntries || !string.IsNullOrEmpty(end))
        //        results.Add(value.Substring(last));

        //    return results.ToArray();
        //}

        /// <summary>
        /// Splits a string at the given indexes, does not remove the characters at each index.
        /// </summary>
        /// <param name="value">The string to split.</param>
        /// <param name="indexes">The indexes where to split.</param>
        /// <returns>A string array</returns>
        public static string[] Split(this string value, params int[] indexes)
        {
            return value.Split(StringSplitOptions.None, indexes);
        }

        /// <summary>
        /// Splits a string at the given indexes.
        /// </summary>
        /// <param name="value">The string to split.</param>
        /// <param name="options">SplitOptions - if the character at the index should be removed use RemoveEmptyEntries.</param>
        /// <param name="indexes">The indexes where to split.</param>
        /// <returns>A string array.</returns>
        public static string[] Split(this string value, StringSplitOptions options, params int[] indexes)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (indexes == null)
                throw new ArgumentNullException("indexes");

            int indexesCount = indexes.Length;
            int length = value.Length;

            if (indexesCount == 0)
                throw new ArgumentException("indexes");

            // make the compact version if only one index is present 
            if (indexesCount == 1)
            {
                int index = indexes[0];
                if (index < 0 || index > length)
                    throw new ArgumentOutOfRangeException("indexes");

                return value.SplitInternal(index, options);
            }

            SCG.List<int> filteredIndexes = new SCG.List<int>(indexesCount);

            foreach (int i in indexes)
            {
                //if (i > -1 && i < length && !filteredIndexes.Contains(i))
                //    filteredIndexes.Add(i);

                if (i < 0 || i >= length)
                    throw new ArgumentOutOfRangeException("indexes");

                if (!filteredIndexes.Contains(i))
                    filteredIndexes.Add(i);
            }

            // sort the indexes to keep the order intact
            filteredIndexes.Sort();

            indexesCount = filteredIndexes.Count;

            // test if the compact version would suffice - note: index bounds have already been checked
            if (indexesCount == 1)
                return value.SplitInternal(filteredIndexes[0], options);
            
            SCG.List<string> results = new SCG.List<string>(indexesCount + 1);

            bool keepEmptyEntries = options == StringSplitOptions.RemoveEmptyEntries;
            int last = 0;
            foreach (int current in filteredIndexes)
            {
                if (last == current)
                    continue;

                string s = value.Substring(last, current - last);

                if (keepEmptyEntries || !string.IsNullOrWhiteSpace(s))
                    results.Add(s);

                last = current;
            }

            // add the last remaining string to the results
            string end = value.Substring(last);

            if (keepEmptyEntries || !string.IsNullOrWhiteSpace(end))
                results.Add(value.Substring(last));

            return results.ToArray();
        }

        /// <summary>
        /// Removes all whitespaces from a string
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>A string without whitespaces.</returns>
        public static string RemoveWhiteSpaces(this string value)
        {
            return new StringBuilder(value).RemoveWhiteSpaces().ToString();
        }

        /// <summary>
        /// Tests if a string is null or empty.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>Returns true if the string is null or empty.</returns>
        [MethodImpl((MethodImplOptions)256)]        
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Tests if a string is null or consists of whitespaces only.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>Returns true if the string is null or consists of whitespaces only.</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Replaces the format item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="value">A composite format string (see Remarks).</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        /// <exception cref="System.ArgumentNullException">format or args is null.</exception>
        /// <exception cref="System.FormatException">format is invalid.-or- The index of a format item is less than zero, or greater than or equal to the length of the args array."</exception>
        [MethodImpl((MethodImplOptions)256)]
        public static string Format(this string value, params object[] args)
        {
            return string.Format(value, args);
        }
        
        /// <summary>
        /// Compares two specified System.String objects using the specified rules, and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="value">The first string to compare.</param>
        /// <param name="other">The second string to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.Value Condition Less than zero value is less than other. Zero strA equals strB. Greater than zero value is greater than other.</returns>
        /// <exception cref="System.ArgumentException">comparisonType is not a System.StringComparison value.</exception>
        /// <exception cref="System.NotSupportedException">System.StringComparison is not supported.</exception>
        [MethodImpl((MethodImplOptions)256)]        
        public static int CompareTo(this string value, string other, StringComparison comparisonType = StringComparison.Ordinal)
        {            
            return string.Compare(value, other, comparisonType);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static bool Contains(this string value, string search, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return value.IndexOf(search, comparisonType) > -1;
        }

        [MethodImpl((MethodImplOptions)256)]
        public static bool Contains(this string value, char character, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return value.Contains(character.ToString(), comparisonType);
        }

        //public const string WhiteSpace = " ";
        //public const string NewLine = "\n";
        //public const string CarriageReturn = "\r";
        //public const string Escape = "\\";
        //public const string Tabulator = "\t";
        //public const string Empty = "";
        
        /// <summary>
        /// Splits a string at each string sequence
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitter"></param>
        /// <param name="options"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static string[] Split(this string value, string splitter = " ", StringSplitOptions options = StringSplitOptions.None, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            SCG.List<string> results = new SCG.List<string>();

            StringBuilder sb = new StringBuilder(value.Length);

            bool keepEmptyEntries = options == StringSplitOptions.None;

            string tmp = null;

            foreach (char c in value)
            {
                sb.Append(c);

                tmp = sb.ToString();
                int i = tmp.IndexOf(splitter, comparisonType);

                // does the current part contain a splitter?
                if (i > -1)
                {
                    string s = tmp.Remove(i);

                    if (!keepEmptyEntries || !string.IsNullOrWhiteSpace(s))
                        results.Add(s);

                    sb.Clear();
                }
            }
            if (!keepEmptyEntries || !string.IsNullOrWhiteSpace(tmp))
                results.Add(tmp);

            return results.ToArray();
        }

        /// <summary>
        /// Returns a comparer using System.StringComparison
        /// </summary>
        /// <param name="comparisonType">The comparisonType.</param>
        /// <returns>The StringComparer.</returns>
        public static StringComparer GetComparer(StringComparison comparisonType = StringComparison.Ordinal)
        {
            switch (comparisonType)
            {
                case StringComparison.CurrentCulture:
                    return StringComparer.CurrentCulture;

                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase;

                case StringComparison.InvariantCulture:
                    return StringComparer.InvariantCulture;

                case StringComparison.InvariantCultureIgnoreCase:
                    return StringComparer.InvariantCultureIgnoreCase;

                case StringComparison.Ordinal:
                    return StringComparer.Ordinal;

                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;

                default:
                    throw new ArgumentOutOfRangeException("comparisonType");
            }
        }
    }
}
