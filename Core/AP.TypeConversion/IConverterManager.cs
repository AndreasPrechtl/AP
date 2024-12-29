using System;

namespace AP.ComponentModel.Conversion;

public interface IConverterManager : AP.IDisposable
{
    GenericHelper Generic { get; }
    NonGenericHelper NonGeneric { get; }

    void Register(Converter converter);

    Converter<TInput, TOutput>? GetConverter<TInput, TOutput>()
        where TInput : notnull;        

    Converter? GetConverter(Type inputType, Type outputType);

    bool Contains(Converter converter);        
    
    void Release(Converter converter);
 
    void Clear();                
}
