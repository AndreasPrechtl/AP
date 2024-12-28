using AP.Reflection;
using System;

namespace AP.ComponentModel.Validation;

public class ValidationMessage
{
    private readonly object _value;
    private readonly string _message;
    private readonly MemberPath _memberPath;
    private readonly ValidationMessageType _type;
    
    public ValidationMessage(MemberPath memberPath, object value, ValidationMessageType type = ValidationMessageType.Info, string? message = null)
    {
        ArgumentNullException.ThrowIfNull(memberPath);

        _value = value;
        _message = message;
        _memberPath = memberPath;
        _type = type;
    }

    public object Value => _value;
    public MemberPath MemberPath => _memberPath;
    public string Message => _message;
    public ValidationMessageType Type => _type;
}
