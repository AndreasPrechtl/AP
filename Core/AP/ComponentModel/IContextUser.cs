using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.ComponentModel
{
    /// <summary>
    /// An interface for types that use disposable objects.
    /// </summary>
    public interface IContextUser : AP.IDisposable
    {
        AP.IDisposable Context { get; }
        bool OwnsContext { get; }
    }

    /// <summary>
    /// A generic interface for types that use disposable objects.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IContextUser<out TContext> : IContextUser
        where TContext : class, AP.IDisposable
    {
        new TContext Context { get; }
    }
}
