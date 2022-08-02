using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AP.Observable
{
    public class MemberAccessedEventArgs : EventArgs
    {
        private readonly string _callingMemberName;
        private readonly string _memberName;

        public string CallingMemberName { get { return _callingMemberName; } }
        public string MemberName { get { return _memberName; } }

        public MemberAccessedEventArgs(string memberName, [CallerMemberName] string callingMemberName = null)
        {
            if (string.IsNullOrWhiteSpace(memberName))
                throw new ArgumentNullException("memberName");

            if (string.IsNullOrWhiteSpace(callingMemberName))
                throw new ArgumentNullException("callingMemberName");

            _memberName = memberName;
            _callingMemberName = callingMemberName;
        }
    }
}
