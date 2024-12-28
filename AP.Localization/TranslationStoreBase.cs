using Microsoft.Extensions.Localization;

namespace AP.Localization
{
    public abstract class TranslationStoreBase<TTranslationStore> : IStringLocalizer<TTranslationStore>
        where TTranslationStore : TranslationStoreBase<TTranslationStore>
    {
        // Key, TranslationValue
        protected readonly IReadOnlyDictionary<string, TranslationValues> Translations;        
        protected readonly ICultureProvider CultureProvider;

        protected TranslationStoreBase(ICultureProvider cultureProvider)
        {
            this.CultureProvider = cultureProvider;
        }

        public string? this[string name] => this[name, Array.Empty<object>()];

        public string? this[string name, params object[] arguments]
        {
            get
            {
                if (this.Translations.TryGetValue(name, out var translationValue))
                    return string.Format(translationValue[this.CultureProvider.Culture], arguments);

                return null;
            }
        }

        LocalizedString IStringLocalizer.this[string name] => ((IStringLocalizer)this)[name, Array.Empty<object>()];

        LocalizedString IStringLocalizer.this[string name, params object[] arguments]
        {
            get
            {
                var translation = this[name, arguments];

                return translation is not null
                    ? new LocalizedString(name, translation, false, this.GetType().Name)
                    : new LocalizedString(name, new FieldAccessException(name).Message, true, this.GetType().Name);
            }
        }

        IEnumerable<LocalizedString> IStringLocalizer.GetAllStrings(bool includeParentCultures)
        {
            // this is a weird one... not sure where that's being used (/abused?) or how it should be used...
            throw new NotImplementedException();
        }
    }
}
