using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AP.Collections;
using AP.Collections.Specialized;
using AP.Linq;

namespace AP.ComponentModel.Validation
{
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

        protected internal ValidationResult(object target, IEnumerable<ValidationMessage> messages, IEqualityComparer<ValidationMessage> comparer = null)
        {
            comparer = comparer ?? ValidationMessageEqualityComparer.Default;
            
            Set<ValidationMessage> msgs = new Set<ValidationMessage>(comparer);
            Set<ValidationMessage> successes = new Set<ValidationMessage>(comparer);
            Set<ValidationMessage> warnings = new Set<ValidationMessage>(comparer);
            Set<ValidationMessage> errors = new Set<ValidationMessage>(comparer);
            Set<ValidationMessage> infos = new Set<ValidationMessage>(comparer);

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
                
        public object Target
        {
            get { return _target; }
        }
        
        public MessageSet Messages
        {
            get { return _messages; }
        }
        
        public bool ContainsMessages { get { return _messages.Count > 0; } }

        public MessageSet Infos { get { return _infos; } }
        public MessageSet SuccessMessages { get { return _successes; } }
        public MessageSet WarningMessages { get { return _warnings; } }
        public MessageSet ErrorMessages { get { return _errors; } }
        
        public bool ContainsInfos { get { return _infos.Count > 0; } }
        public bool ContainsSuccessMessages { get { return _successes.Count > 0; } }
        public bool ContainsWarningMessages { get { return _warnings.Count > 0; } }
        public bool ContainsErrorMessages { get { return _errors.Count > 0; } }        
    }
}
