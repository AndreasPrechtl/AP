namespace AP.Collections.ObjectModel;

public partial class DictionaryBase<TKey, TValue> 
{
    public class ValueCollection : DictionaryValueCollection<DictionaryBase<TKey, TValue>, TKey, TValue>
    {
        public ValueCollection(DictionaryBase<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public new ValueCollection Clone() => (ValueCollection)this.OnClone();
    }
}
