using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Data
{
    public interface IEntitySetProvider
    {
        IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : class;
    }
}
