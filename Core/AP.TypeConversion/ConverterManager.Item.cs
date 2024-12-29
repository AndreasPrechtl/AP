namespace AP.ComponentModel.Conversion;

public partial class ConverterManager
{
    private class Item
    {
        public readonly bool IsGenerated;
        public readonly Converter Converter;
        public readonly Key Key;

        public Item(Key key, Converter converter, bool isGenerated = false)
        {
            this.Converter = converter;
            this.IsGenerated = isGenerated;
            this.Key = key;
        }
    }    
}
