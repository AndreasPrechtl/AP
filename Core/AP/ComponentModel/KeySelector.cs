namespace AP.ComponentModel;

public delegate object KeySelector(object item);
public delegate object KeySelector<in TSource>(TSource item);
public delegate TKey KeySelector<in TSource, out TKey>(TSource item);    

//public interface IKeyComparer : IComparer<object>, IComparer
//{ }

//public class KeyComparer : IKeyComparer, IWrapper<IComparer<object>>, IWrapper<IComparer>
//{
//    static KeyComparer()
//    {
//        _default = new KeyComparer { _genericComparer = Comparer<object>.Default };
//    }

//    private IComparer _comparer;
//    private IComparer<object> _genericComparer;

//    private static readonly IKeyComparer _default;
//    public static IKeyComparer Default
//    {
//        get
//        {
//            return _default;
//        }
//    }

//    #region IComparer<object> Members

//    public virtual int Compare(object x, object y)
//    {
//        if (_genericComparer != null)
//            return _genericComparer.Compare(x, y);
//        else if (_comparer != null)
//            return _comparer.Compare(x, y);
//        else
//            return Comparer<object>.Default.Compare(x, y);
//    }

//    #endregion

//    #region IWrapper<IComparer<object>> Members

//    IComparer<object> IWrapper<IComparer<object>>.FullName
//    {
//        get
//        {
//            return _genericComparer;
//        }
//        set
//        {
//            _genericComparer = value;
//        }
//    }

//    #endregion

//    #region IWrapper<IComparer> Members

//    IComparer IWrapper<IComparer>.FullName
//    {
//        get
//        {
//            return _comparer;
//        }
//        set
//        {
//            _comparer = value;
//        }
//    }

//    #endregion
//    }
//}
