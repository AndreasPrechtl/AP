using AP.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Validation
{
    public class ValidationMessage
    {
        private readonly object _value;
        private readonly string _message;
        private readonly MemberPath _memberPath;
        private readonly ValidationMessageType _type;
        
        public ValidationMessage(MemberPath memberPath, object value, ValidationMessageType type = ValidationMessageType.Info, string message = null)
        {
            if (memberPath == null)
                throw new ArgumentNullException("memberPath");

            _value = value;
            _message = message;
            _memberPath = memberPath;
            _type = type;
        }

        public object Value { get { return _value; } }
        public MemberPath MemberPath { get { return _memberPath; } }
        public string Message { get { return _message; } }
        public ValidationMessageType Type { get { return _type; } }      
    }
}
