using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel
{
    public interface IManager : IResolver
    {
        void Register<T>(object key, T instance);
        void Release(object key);        
    }

    public interface IManager<TBase> : IResolver<TBase>
    {
        void Register<T>(object key, T instance) where T : TBase;
        void Release(object key);
    }

    public interface IManager<TKey, TBase> : IResolver<TKey, TBase>
    {
        void Register<T>(TKey key, T instance) where T : TBase;
        void Release(TKey key);
    }

    //public interface IKeyUserManager<TBase> : IResolver<TBase>
    //    where TBase : IKeyUser
    //{
    //    void Register<T>(T instance) where T : TBase;
    //    void Release<T>(T instance) where T : TBase;
    //}
    
    //public interface IKeyUserManager<TKey, TBase> : IResolver<TKey, TBase>
    //    where TBase : IKeyUser<TKey>
    //{
    //    void Register<T>(T instance) where T : TBase;
    //    void Release<T>(T instance) where T : TBase;
    //}
}
