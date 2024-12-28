using System.Collections.Generic;
using AP.Collections;

namespace AP.ComponentModel.Validation;

public abstract partial class ValidationResult
{
    private readonly object _target;        
    private readonly MessageSet _messages;
    
    private readonly MessageSet _infos;
    private readonly MessageSet _successes;
    private readonly MessageSet _warnings;
    private readonly MessageSet _errors;

    protected internal ValidationResult(object target, MessageSet messages)
    {
        _target = target;
        _messages = messages;
    }

    protected internal ValidationResult(object target, IEnumerable<ValidationMessage> messages, IEqualityComparer<ValidationMessage>? comparer = null)
    {
        comparer = comparer ?? ValidationMessageEqualityComparer.Default;
        
        Set<ValidationMessage> msgs = new(comparer);
        Set<ValidationMessage> successes = new(comparer);
        Set<ValidationMessage> warnings = new(comparer);
        Set<ValidationMessage> errors = new(comparer);
        Set<ValidationMessage> infos = new(comparer);

        foreach (ValidationMessage message in messages)
        {
            msgs.Add(msgs);

            switch (message.Type)
            {
                case ValidationMessageType.Info:
                    infos.Add(message);
                    break;
                case ValidationMessageType.Success:
                    successes.Add(message);
                    break;
                case ValidationMessageType.Warning:
                    warnings.Add(message);
                    break;
                case ValidationMessageType.Error:
                    errors.Add(message);
                    break;
            }
        }

        _messages = new MessageSet(msgs);

        _infos = new MessageSet(infos);
        _successes = new MessageSet(successes);
        _warnings = new MessageSet(warnings);
        _errors = new MessageSet(errors);
    }

    public object Target => _target;

    public MessageSet Messages => _messages;

    public bool ContainsMessages => _messages.Count > 0;

    public MessageSet Infos => _infos;
    public MessageSet SuccessMessages => _successes;
    public MessageSet WarningMessages => _warnings;
    public MessageSet ErrorMessages => _errors;

    public bool ContainsInfos => _infos.Count > 0;
    public bool ContainsSuccessMessages => _successes.Count > 0;
    public bool ContainsWarningMessages => _warnings.Count > 0;
    public bool ContainsErrorMessages => _errors.Count > 0;
}
