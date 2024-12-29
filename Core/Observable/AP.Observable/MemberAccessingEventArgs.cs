using System;
using System.Runtime.CompilerServices;

namespace AP.Observable;

public class MemberAccessingEventArgs : System.ComponentModel.CancelEventArgs
{
    public static new readonly MemberAccessingEventArgs Empty = new();

    private readonly string _callingMemberName = null!;
    private readonly string _memberName = null!;

    public string CallingMemberName => _callingMemberName;
    public string MemberName => _memberName;

    private MemberAccessingEventArgs()
    { }

    public MemberAccessingEventArgs(string memberName, [CallerMemberName] string callingMemberName = default!)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            throw new ArgumentNullException(nameof(memberName));

        if (string.IsNullOrWhiteSpace(callingMemberName))
            throw new ArgumentNullException(nameof(callingMemberName));

        _memberName = memberName;
        _callingMemberName = callingMemberName;
    }
}