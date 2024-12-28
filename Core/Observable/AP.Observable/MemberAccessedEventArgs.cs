﻿using System;
using System.Runtime.CompilerServices;

namespace AP.Observable;

public class MemberAccessedEventArgs : EventArgs
{
    private readonly string _callingMemberName;
    private readonly string _memberName;

    public string CallingMemberName => _callingMemberName;
    public string MemberName => _memberName;

    public MemberAccessedEventArgs(string memberName, [CallerMemberName] string callingMemberName = default!)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            throw new ArgumentNullException(nameof(memberName));

        if (string.IsNullOrWhiteSpace(callingMemberName))
            throw new ArgumentNullException(nameof(callingMemberName));

        _memberName = memberName;
        _callingMemberName = callingMemberName;
    }
}
