using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AP.Data;

public interface IDelete
{
    void Delete<TEntity>(params IEnumerable<TEntity> entities) where TEntity : class;    
}

public interface IDelete<TEntity>
    where TEntity : class
{
    void Delete(params IEnumerable<TEntity> entities); 
}