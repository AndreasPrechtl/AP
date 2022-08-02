using System;
using System.Runtime.CompilerServices;

namespace AP.Observable
{
    public class MemberAccessingEventArgs : System.ComponentModel.CancelEventArgs
    {
        private readonly string _callingMemberName;
        private readonly string _memberName;

        public string CallingMemberName { get { return _callingMemberName; } }
        public string MemberName { get { return _memberName; } }

        public MemberAccessingEventArgs(string memberName, [CallerMemberName] string callingMemberName = null)
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