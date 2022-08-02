using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.ComponentModel;

namespace AP.Data
{
    public interface IEntityFactory
    {
        TEntity GetNewEntity<TEntity>() where TEntity : class;
    }

    public interface IEntityFactory<out TEntity>
        where TEntity : class
    {
        TEntity GetNewEntity();
    }
}
