using AP.Collections;

namespace AP.ComponentModel.Conversion;

internal interface ILinkedConverter
{
    IListView<Converter> Converters { get; }
}
