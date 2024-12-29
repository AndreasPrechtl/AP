using System;

namespace AP.ComponentModel.Conversion;

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
        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (obj == this)
                return true;

            if (obj is Key key)
                return this.InputType == key.InputType && this.OutputType == key.OutputType;

            return false;
        }

        public override int GetHashCode() => InputType.GetHashCode() & OutputType.GetHashCode();
    }    
}
