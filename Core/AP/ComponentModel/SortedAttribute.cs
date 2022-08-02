using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter)]
    public sealed class SortedAttribute : Attribute
    {
        private readonly bool _isSorted;        
        public bool IsSorted { get { return _isSorted; } }

        public SortedAttribute(bool isSorted = true)
            : base()
        {
            _isSorted = isSorted;
        }

        public override bool Match(object obj)
        {
            return obj is SortedAttribute && ((SortedAttribute)obj)._isSorted == _isSorted;
        }
    }
}
