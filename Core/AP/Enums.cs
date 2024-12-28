using System;
using System.Collections.Generic;

namespace AP;

/// <summary>
/// Contains several helper methods for enums
/// </summary>
public static class Enums
{
    public static bool TryParse<TEnum>(string value, out TEnum parsed, bool ignoreCase = true)
        where TEnum : struct, IComparable, IConvertible, IFormattable => Enum.TryParse<TEnum>(value, ignoreCase, out parsed);

    public static TEnum Parse<TEnum>(string value, bool ignoreCase = true)
        where TEnum : struct, IComparable, IConvertible, IFormattable => (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);

    public static IEnumerable<TEnum> GetValues<TEnum>()
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            yield return value;
    }

    public static string Format<TEnum>(TEnum value, string format)
        where TEnum : struct, IComparable, IConvertible, IFormattable => Enum.Format(typeof(TEnum), value, format);

    public static bool IsDefined<TEnum>(TEnum value)
        where TEnum : struct, IComparable, IConvertible, IFormattable => Enum.IsDefined(typeof(TEnum), value);

    public static Type GetUnderlyingType<TEnum>()
        where TEnum : struct, IComparable, IConvertible, IFormattable => Enum.GetUnderlyingType(typeof(TEnum));

    public static AP.Collections.Specialized.NameValueDictionary<TEnum> ToDictionary<TEnum>()
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        Type t = typeof(TEnum);
        string[] names = Enum.GetNames(t);
        Array values = Enum.GetValues(t);

        int l = names.Length;

        AP.Collections.Specialized.NameValueDictionary<TEnum> dict = new(l);

        for (int i = 0; i < l; ++i)
            dict.Add(names[i], (TEnum)values.GetValue(i)!);

        return dict;
    }

    public static AP.Collections.Specialized.ReadOnlyNameValueDictionary<TEnum> ToReadOnlyDictionary<TEnum>()
        where TEnum : struct, IComparable, IConvertible, IFormattable => new(Enums.ToDictionary<TEnum>());

    // omfg what a mongo code... hope i'll never see sth like that again:

    ///// <summary>
    ///// Converts Enumeration type into a dictionary of names and values
    ///// </summary>
    ///// <param name="t">Enum type</param>
    //public static IDictionary<string, int> EnumToDictionary(this Type t)
    //{
    //    if (t == null) throw new NullReferenceException();
    //    if (!t.IsEnum) throw new InvalidCastException("object is not an Enumeration");

    //    string[] names = Enum.GetNames(t);
    //    Array values = Enum.GetValues(t);

    //    return (from i in Enumerable.Range(0, names.Length)
    //            select new { Key = names[i], Value = (int)values.GetValue(i) })
    //                .ToDictionary(k => k.Key, k => k.Value);
    //}
}
