using System;
using AP.Data;

namespace AP.Web.Mvc
{
    public class ViewModel<TDataObject> : AP.Data.ViewModel<TDataObject, object> 
        where TDataObject : IDataObject
    {
        public ViewModel
        (
            TDataObject current, 
            int currentIndex = 0, 
            int count = 0, 
            object first = null, 
            object previous = null, 
            object next = null, 
            object last = null
        ) 
            : base(current, currentIndex, count, first, previous, next, last)
        {
        }
    }
}

