using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using AP.Linq;

namespace AP
{
    /// <summary>
    /// Contains extensions for System.Text.StringBuilder
    /// </summary>
    public static class StringBuilderExtensions
    {
        public static IEnumerable<char> AsEnumerable(this StringBuilder builder)
        {
            return builder.ToString().AsEnumerable();
            //int l = builder.Length;
            //for (int i = 0; i < l; i++)
            //    yield return builder[i];
        }

        public static IEnumerator<char> GetEnumerator(this StringBuilder builder)
        {
            return builder.ToString().GetEnumerator();
            //return AsEnumerable(builder).GetEnumerator();
        }

        public static bool IsNullOrEmpty(this StringBuilder builder)
        {
            return builder == null || builder.IsEmpty();
        }

        public static bool IsEmpty(this StringBuilder builder)
        {
            return builder.Length == 0;
        }

        public static bool IsNullOrWhitespace(this StringBuilder builder)
        {
            return builder.ToString().IsNullOrWhiteSpace();
        }

        public static string[] Split(this StringBuilder builder, params int[] indexes)
        {
            return builder.Split(StringSplitOptions.None, indexes);
        }

        public static string[] Split(this StringBuilder builder, StringSplitOptions options, params int[] indexes)
        {
            return builder.ToString().Split(options, indexes);
        }

        public static string[] Split(this StringBuilder builder, string separator = " ", StringSplitOptions options = StringSplitOptions.None, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return builder.ToString().Split(separator, options, comparisonType);
        }

        public static string[] Split(this StringBuilder builder, char[] separators, StringSplitOptions options = StringSplitOptions.None)
        {
            return builder.ToString().Split(separators, options);

            //int l = builder.Length;
            //List<string> split = new List<string>();
            //StringBuilder sb = new StringBuilder(builder.ToString());

            //for (int i = 0; i < sb.Length; i++)
            //{
            //    if (separators.Contains(sb[i]))
            //    {
            //        string s = sb.Substring(0, i).ToString();
            //        split.Add(s);

            //        if (options == StringSplitOptions.None)
            //            split.Add(string.Empty); // null or string.Empty ?

            //        i = 0;
            //    }
            //}
            //return EnumerableExtensions.AsReadOnly(split);
        }

        public static string[] Split(this StringBuilder builder, string[] separators, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return builder.ToString().Split(separators, options);
        }

        public static bool StartsWith(this StringBuilder builder, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return builder.ToString().StartsWith(value, comparisonType);

            //ExceptionHelper.ThrowOnArgumentNullException(() => value);

            //int builderLength = builder.Length;
            //int valueLength = value.Length;

            //if (builderLength < valueLength)
            //    return false;

            //StringBuilder sb = new StringBuilder(valueLength);
            //for (int i = 0; i < valueLength; i++)
            //    sb.Append(builder[i]);

            //return sb.ToString().Equals(value, comparisonType);
        }

        /// <summary>
        /// Removes whitespace characters from the start and the end
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static StringBuilder Trim(this StringBuilder builder)
        {
            return builder.TrimStart().TrimEnd();
        }

        /// <summary>
        /// Removes whitespace characters from the start
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static StringBuilder TrimStart(this StringBuilder builder)
        {
            int count = 0;
            
            for (int l = builder.Length, i = 0; i < l; ++i)
            {
                if (char.IsWhiteSpace(builder[i]))
                    count++;
                else
                    break;
            }

            return builder.Remove(0, count);
        }

        /// <summary>
        /// Removes whitespace characters from the end
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder builder)
        {
            int count = 0;
            int l = builder.Length;

            for (int i = builder.Length; --i > 0; )
            {
                if (char.IsWhiteSpace(builder[i]))
                    count++;
                else
                    break;
            }

            return builder.Remove(l - count, count);
        }

        public static bool EndsWith(this StringBuilder builder, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return builder.ToString().EndsWith(value, comparisonType);
            //ExceptionHelper.ThrowOnArgumentNullException(() => value);

            //int builderLength = builder.Length;
            //int valueLength = value.Length;

            //if (builderLength < valueLength)
            //    return false;

            //StringBuilder sb = new StringBuilder(valueLength);

            //int minIndex = builderLength - valueLength - 1;

            //for (int i = builderLength - 1; i > minIndex; i--)
            //    sb.Insert(0, builder[i]);

            //return sb.ToString().Equals(value, comparisonType);
        }

        public static bool Contains(this StringBuilder builder, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return builder.IndexOf(value, comparisonType) > -1;
        }

        [MethodImpl((MethodImplOptions)256)]
        public static bool Contains(this StringBuilder builder, char character, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return builder.Contains(character.ToString(), comparisonType);
        }

        public static StringBuilder Remove(this StringBuilder builder, int startIndex)
        {            
            return builder.Remove(startIndex, builder.Length - startIndex);
        }

        public static StringBuilder Substring(this StringBuilder builder, int startIndex)
        {
            return builder.Substring(startIndex, builder.Length - startIndex);
        }

        public static StringBuilder Substring(this StringBuilder builder, int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("StartIndex cannot be less than zero.");

            // if the length is specified -> remove the "end" first
            if (length < 0)
                throw new ArgumentOutOfRangeException("Length cannot be less than zero.");

            if (length > (builder.Length - startIndex))
                throw new ArgumentOutOfRangeException("Index and length must refer to a location within the string.");

            //string s = builder.ToString().Substring(startIndex, length);

            // remove the start
            builder.Remove(0, startIndex);
                                    
            // remove the end
            int endLength = builder.Length - length;

            if (endLength > 0)
                builder.Remove(length, endLength);
            
            return builder;
        }

        public static int IndexOf(this StringBuilder builder, char value)
        {
            return builder.ToString().IndexOf(value);
        }

        public static int IndexOf(this StringBuilder builder, char value, int startIndex)
        {
            return builder.ToString().IndexOf(value, startIndex);
        }

        public static int IndexOf(this StringBuilder builder, char value, int startIndex, int count)
        {
            return builder.ToString().IndexOf(value, startIndex, count);

            //int builderLength = builder.Length;

            //if (startIndex < 0 || startIndex > builderLength)
            //    ExceptionHelper.ThrowArgumentOutOfRangeException(() => startIndex);

            //if (count < 0 || count > (builderLength - startIndex))
            //    ExceptionHelper.ThrowArgumentOutOfRangeException(() => count);

            //int max = startIndex + count;

            //for (int i = startIndex; i < max; i++)
            //{
            //    if (builder[i] == value)
            //        return i;                
            //}

            //return -1;
        }

        public static int IndexOf(this StringBuilder builder, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return IndexOf(builder, value, 0);
        }

        public static int IndexOf(this StringBuilder builder, string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return IndexOf(builder, value, startIndex, builder.Length - startIndex);
        }

        public static int IndexOf(this StringBuilder builder, string value, int startIndex, int count, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return builder.ToString().IndexOf(value, startIndex, count, comparisonType);
            //ExceptionHelper.ThrowOnArgumentNullException(() => value);

            //int builderLength = builder.Length;
            //int valueLength = value.Length;

            //if (startIndex < 0 || startIndex > builderLength)
            //    ExceptionHelper.ThrowArgumentOutOfRangeException(() => startIndex);

            //if (count < 0 || count > (builderLength - startIndex))
            //    ExceptionHelper.ThrowArgumentOutOfRangeException(() => count);

            //if (builderLength < valueLength)
            //    return -1;

            //int max = startIndex + count;

            //StringBuilder sb = new StringBuilder(builderLength);

            //for (int x = startIndex; x < max; x++)
            //{
            //    for (int y = x; y < max; y++)
            //    {
            //        sb.Append(builder[y]);

            //        if (sb.ToString().Equals(value, comparisonType))
            //            return x;
            //    }
            //    sb.Clear();
            //}

            //return -1;
        }

        public static int IndexOfAny(this StringBuilder builder, char[] anyOf)
        {
            return builder.ToString().IndexOfAny(anyOf);
            //foreach (char c in anyOf)
            //{
            //    int index = IndexOf(builder, c);
            //    if (index > -1)
            //        return index;
            //}

            //return -1;
        }

        public static int LastIndexOf(this StringBuilder builder, char value)
        {
            return builder.ToString().LastIndexOf(value);
            //return LastIndexOf(builder, value, builder.Length - 1, builder.Length);
        }

        public static int LastIndexOf(this StringBuilder builder, char value, int startIndex)
        {
            return builder.ToString().LastIndexOf(value, startIndex);
            //return LastIndexOf(builder, value, startIndex, startIndex + 1);
        }

        public static int LastIndexOf(this StringBuilder builder, char value, int startIndex, int count)
        {
            return builder.ToString().LastIndexOf(value, startIndex, count);
            //ExceptionHelper.ThrowOnArgumentNullException(() => value);

            //int builderLength = builder.Length;

            //if (startIndex < 0 || startIndex > builderLength)
            //    ExceptionHelper.ThrowArgumentOutOfRangeException(() => startIndex);

            //if (count < 0 || count > (builderLength - startIndex))
            //    ExceptionHelper.ThrowArgumentOutOfRangeException(() => count);

            //int max = startIndex + count;
            //int min = startIndex - 1;

            //for (int i = max; i > min; i--)
            //{
            //    if (builder[i] == value)
            //        return i;
            //}

            //return -1;
        }

        public static int LastIndexOfAny(this StringBuilder builder, char[] anyOf)
        {
            return builder.ToString().LastIndexOfAny(anyOf);
            //foreach (char c in anyOf)
            //{
            //    int index = LastIndexOf(builder, c);
            //    if (index > -1)
            //        return index;
            //}

            //return -1;
        }


        public static int LastIndexOf(this StringBuilder builder, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return LastIndexOf(builder, value, 0, comparisonType);
        }

        public static int LastIndexOf(this StringBuilder builder, string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return LastIndexOf(builder, value, startIndex, builder.Length - startIndex, comparisonType);
        }

        public static int LastIndexOf(this StringBuilder builder, string value, int startIndex, int count, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return builder.ToString().LastIndexOf(value, startIndex, count, comparisonType);
            //ExceptionHelper.ThrowOnArgumentNullException(() => value);

            //int builderLength = builder.Length;
            //int valueLength = value.Length;

            //if (startIndex < 0 || startIndex > builderLength)
            //    ExceptionHelper.ThrowArgumentOutOfRangeException(() => startIndex);

            //if (count < 0 || count > (builderLength - startIndex))
            //    ExceptionHelper.ThrowArgumentOutOfRangeException(() => count);

            //if (builderLength < valueLength)
            //    return -1;

            //int max = startIndex + count - 1;
            //int min = startIndex - 1;

            //ExceptionHelper.ThrowOnArgumentNullException(() => value);

            //StringBuilder sb = new StringBuilder(builderLength);

            //for (int x = max; x > min; x--)
            //{
            //    for (int y = x; y > min; y--)
            //    {
            //        sb.Insert(0, builder[y]);

            //        if (sb.ToString().Equals(value, comparisonType))
            //            return x + 1 - sb.Length;
            //    }
            //    sb.Clear();
            //}

            //return -1;
        }

        public static StringBuilder RemoveNullCharacters(this StringBuilder builder)
        {
            // not used - is slower - however using the Replace directly on a string is quite a lot faster
            //return builder.Replace("\0", string.Empty);

            for (int l = builder.Length, i = 0; i < l; ++i)
            {
                if (builder[i] == '\0')
                {
                    builder.Remove(i, 1);
                    l--;
                    i--;
                }
            }

            return builder;
        }
        public static StringBuilder RemoveWhiteSpaces(this StringBuilder builder)
        {
            for (int l = builder.Length, i = 0; i < l; ++i)
            {
                if (char.IsWhiteSpace(builder[i]))
                {
                    builder.Remove(i, 1);
                    l--;
                    i--;
                }
            }
            return builder;
        }
    }
}