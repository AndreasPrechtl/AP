namespace AP.Linq;

public delegate object KeySelector(object item);
public delegate object KeySelector<in TSource>(TSource item);
public delegate TKey KeySelector<in TSource, out TKey>(TSource item);
