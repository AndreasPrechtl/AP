using AP.Collections.ReadOnly;

namespace AP.Linq;

public static class SetExtensions
{
    public static ReadOnlySet<TElement> AsReadOnly<TElement>(this System.Collections.Generic.ISet<TElement> set) => new(set);
}
