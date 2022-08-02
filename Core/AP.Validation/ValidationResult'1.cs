using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.ComponentModel.Validation
{
    public class ValidationResult<TTarget> : ValidationResult
    {
        /// <summary>
        /// cctor.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="messages"></param>
        protected ValidationResult(TTarget target, MessageSet messages)
            : base(target, messages)
        { }

        /// <summary>
        /// Creates a new ValidationResult.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="messages"></param>
        public ValidationResult(TTarget target, IEnumerable<ValidationMessage> messages)
            : base(target, messages, null)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (messages == null)
                throw new ArgumentNullException("messages");
        }

        /// <summary>
        /// Gets the validation target.
        /// </summary>
        public new TTarget Target
        {
            get { return (TTarget)base.Target; }
        }
    }
}
