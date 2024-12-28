using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using AP.Reflection;

namespace AP
{
    /// <summary>
    /// Contains methods for cloning objects.
    /// </summary>
    public static class Cloner
    {
        private const BindingFlags _flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
        private const string _memberwiseCloneMethodName = "MemberwiseClone";        
        private static Dictionary<Type, Delegate> _memberwiseCloneMethodCache = new Dictionary<Type, Delegate>();

        public static T MemberwiseClone<T>(this T instance)
        {
            Type t = instance.GetType();
            Delegate d = null;

            if (!_memberwiseCloneMethodCache.TryGetValue(t, out d))
            {
                d = Delegate.CreateDelegate(t, t.GetMethod(_memberwiseCloneMethodName, _flags));
                lock (_memberwiseCloneMethodCache)
                    _memberwiseCloneMethodCache.Add(t, d);
            }
            return (T)d.DynamicInvoke(instance);
        }

        private const string _cloneMethodName = "Clone";
        private static readonly Dictionary<Type, Delegate> _cloneMethodCache = new Dictionary<Type, Delegate>();
                
        public static T Clone<T>(this T instance)
        {
            if (instance is ICloneable)
                return (T)((ICloneable)instance).Clone();

            Type t = instance.GetType();
            Delegate d = null;

            if (!_cloneMethodCache.TryGetValue(t, out d))
            {
                MethodInfo mi = t.GetMethod(_cloneMethodName, _flags);
                if (mi != null)
                {
                    d = Delegate.CreateDelegate(t, mi);
                    lock (_cloneMethodCache)
                        _cloneMethodCache.Add(t, d);
                }
            }
            if (d != null)
                return (T)d.DynamicInvoke(instance);
            
            return MemberwiseClone(instance);
        }
    }
}
