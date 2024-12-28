using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Reflection;

namespace AP
{
    /// <summary>
    /// Class used for "static inheritance".
    /// </summary>
    public abstract class StaticType
    {
        /// <summary>
        /// Throws an exception if it's being invoked.
        /// </summary>
        protected StaticType()
        {
            ThrowTypeInitializationException(this.GetType());
        }
                
        /// <summary>
        /// Throws a TypeInitializationException.
        /// Does not check if the type is invalid.
        /// </summary>
        /// <param name="type">The Type.</param>
        public static void ThrowTypeInitializationException(Type type)
        {
            throw new TypeInitializationException(type.FullName, new Exception("Type must be abstract"));
        }

        /// <summary>
        /// Tests if the type could be used like a StaticType and throws an exception if it is not abstract.
        /// </summary>
        /// <param name="type">The Type.</param>
        public static void AssertTypeQualifies(Type type)
        {
            if (!type.IsAbstract)
                ThrowTypeInitializationException(type);
        }
    }
}
