using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Conversion
{
    public partial class ConverterManager
    {
        private sealed class Key
        {
            internal readonly Type InputType;
            internal readonly Type OutputType;
                        
            public Key(Type inputType, Type outputType)
            {
                InputType = inputType;
                OutputType = outputType;
            }

            /// <summary>
            /// Returns true when obj is ConverterKey<TInput, TOutput>
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                Key key = (Key)obj;
            
                return this == obj || InputType == key.InputType && OutputType == key.OutputType;
            }

            public override int GetHashCode()
            {
                return InputType.GetHashCode() & OutputType.GetHashCode();
            }
        }    
    }
}
