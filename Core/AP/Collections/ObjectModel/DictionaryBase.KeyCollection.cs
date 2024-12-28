namespace AP.Collections.ObjectModel;

public partial class DictionaryBase<TKey, TValue> 
{
    public class KeyCollection : DictionaryKeyCollection<DictionaryBase<TKey, TValue>, TKey, TValue>
    {
        public KeyCollection(DictionaryBase<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public new KeyCollection Clone() => (KeyCollection)this.OnClone();
    }
}
