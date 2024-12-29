using System;
using System.ComponentModel;

namespace AP.ComponentModel.Conversion;

// todo: revisit that pattern, it's bad
public abstract class Converters : StaticType
{
    private static IConverterManager _current = null!;
    public static readonly object SyncRoot = new();

    protected Converters()        
    { }

    public static IConverterManager Manager
    {
        get => _current!;
        internal set
        {
            lock (SyncRoot)
            {
                if (HasManager)
                    throw new InvalidOperationException("Mapper is already set, use dispose first");

                _current = value;
            }
        }
    }

    public static bool HasManager => _current != null;

    public static GenericHelper Generic => Manager!.Generic;
    public static NonGenericHelper NonGeneric => Manager!.NonGeneric;

    private static ExtendedTypeDescriptionProvider _extendedTypeDescriptionProvider = null!;
    private static TypeDescriptionProvider _originalTypeDescriptionProvider = null!;

    public static bool UseExtendedTypeConverters
    {
        get => _extendedTypeDescriptionProvider != null;
        set
        {
            if (UseExtendedTypeConverters != value)
            {
                Type t = typeof(object);

                lock (SyncRoot)
                {
                    if (value)
                    {
                        TypeDescriptionProvider original = TypeDescriptor.GetProvider(t);

                        ExtendedTypeDescriptionProvider extended = new(original);

                        lock (extended)
                        {
                            TypeDescriptor.RemoveProvider(original, t);
                            TypeDescriptor.AddProvider(extended, t);

                            _extendedTypeDescriptionProvider = extended;
                            _originalTypeDescriptionProvider = original;
                        }                    
                    }
                    else
                    {
                        lock (SyncRoot)                            
                        {
                            TypeDescriptor.RemoveProvider(_extendedTypeDescriptionProvider!, t);
                            TypeDescriptor.AddProvider(_originalTypeDescriptionProvider!, t);

                            _extendedTypeDescriptionProvider = null!;
                            _originalTypeDescriptionProvider = null!;
                        }
                    }
                }
            }
        }
    }

    public static void Dispose(bool disposeManager = true)
    {
        lock (SyncRoot)
        {
            UseExtendedTypeConverters = false;
            IConverterManager current = _current;
            _current = null!;
            
            if (disposeManager)
                current.Dispose();                
        }                        
    }

    public static Converter? GetConverter(Type inputType, Type outputType)
    {
        if (HasManager)
        {
            var c = Manager.GetConverter(inputType, outputType);
            if (c != null)
                return c;
        }

        if (UseExtendedTypeConverters)
        {
            var typeConverter = _originalTypeDescriptionProvider.GetTypeDescriptor(inputType)?.GetConverter();

            if (typeConverter?.CanConvertFrom(inputType) is true)
                return (Converter)New.Instance(typeof(TypeConverterWrapper<,>).MakeGenericType(inputType, outputType), typeConverter, UsedTypeConverterMethod.From);

            typeConverter = _originalTypeDescriptionProvider.GetTypeDescriptor(outputType)?.GetConverter();

            if (typeConverter?.CanConvertTo(outputType) is true)
                return (Converter)New.Instance(typeof(TypeConverterWrapper<,>).MakeGenericType(inputType, outputType), typeConverter, UsedTypeConverterMethod.To);                
        }

        return null;
    }

    public static Converter<TInput, TOutput>? GetConverter<TInput, TOutput>()
        where TInput : notnull
    {
        if (HasManager)
        {
            var c = Manager.GetConverter<TInput, TOutput>();                
            if (c != null)
                return c;
        }

        if (UseExtendedTypeConverters)
        {
            Type inputType = typeof(TInput);
            Type outputType = typeof(TOutput);

            var typeConverter = _originalTypeDescriptionProvider.GetTypeDescriptor(inputType)?.GetConverter();
                        
            if (typeConverter?.CanConvertFrom(inputType) is true)
                return new TypeConverterWrapper<TInput, TOutput>(typeConverter, UsedTypeConverterMethod.From);

            typeConverter = _originalTypeDescriptionProvider.GetTypeDescriptor(outputType)?.GetConverter();

            if (typeConverter?.CanConvertTo(outputType) is true)
                return new TypeConverterWrapper<TInput, TOutput>(typeConverter, UsedTypeConverterMethod.To);
        }

        return null;
    }

    public static void Register(Converter converter)
    {
        lock (SyncRoot)
            _current.Register(converter);
    }

    public static void Release(Converter converter)
    {
        lock (SyncRoot)
            _current.Release(converter);
    }

    public static Converter<TInput, TOutput> Register<TInput, TOutput>(TypeConverter converter, UsedTypeConverterMethod method)
        where TInput : notnull
    {
        var c = new TypeConverterWrapper<TInput, TOutput>(converter, method);

        _current.Register(c);

        return c;
    }
    
    public static Converter<TInput, TOutput> Register<TInput, TOutput>(Convert<TInput, TOutput> converter)
        where TInput : notnull
    {
        var c = new ConvertDelegateWrapper<TInput, TOutput>(converter);

        _current.Register(c);

        return c;
    }        
}
