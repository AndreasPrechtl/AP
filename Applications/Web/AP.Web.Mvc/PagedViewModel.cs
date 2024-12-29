using System;
using System.Collections.ObjectModel;

namespace AP.Web.Mvc
{
    public class PagedViewModel<TDataObject> : AP.Data.PagedViewModel<TDataObject, object> 
        where TDataObject: AP.Data.IDataObject
    {
        public PagedViewModel
        (
            ReadOnlyCollection<TDataObject> current, 
            int currentIndex = -1, 
            int count = 0, 
            object first = null, 
            object previous = null, 
            object next = null, 
            object last = null, 
            int pageSize = 3
        ) 
            : base(current, currentIndex, count, first, previous, next, last, pageSize)
        {
        }
    }
}

