using AP.Collections;
using System;
using System.Globalization;

namespace AP.Localization
{
    // that'd probably spawn like 100000 extra classes if I opt to do it this way - so probably a bad idea
    //public sealed partial class MyFuckingTranslation : TranslationValues
    //{

    //}
    public sealed partial class TranslationValues
    {
        public required string Key { get; init; }
        public required string Default { get; init; }

        public string this[CultureInfo culture] => this[culture.Name];
        public string this[string locale]
        {
            get
            {
                if (Translations.TryGetValue(locale, out var value))
                    return value()!;

                return Default;
            }
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        // created & filled via SourceGenerator
        private readonly IDictionaryView<string, Func<string>> Translations;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // auto generated via source generator -> will be moved to TranslationValues.g.cs
        // also use the get to use the default fallback, otherwise use the set value -> no need for complicated dictionary lookups
        // this is just a way to have safe property keys

    }

    // generated via TranslationValuesSourceGenerator
    //public sealed partial class TranslationValues
    //{
    //    public TranslationValues()
    //    {
    //        // could also do this via reflection, but generated might startup faster?
    //        Translations = new Dictionary<string, Func<string>>
    //        {
    //            { nameof(Default), () => Default },
    //            { nameof(de_DE), () => de_DE },
    //            { nameof(en_US), () => en_US }
    //            // only those with a value will be added, the others are not interesting for use with the indexer                
    //        };
    //    }

    //    public string? en_US { get; init; }
    //    public string? de_DE { get; init; }        
    //}
}