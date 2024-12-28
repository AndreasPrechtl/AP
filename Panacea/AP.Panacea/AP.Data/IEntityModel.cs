using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Data
{
    public partial interface IEntityModel :
        IQuery,
        ICreate,
        IUpdate,
        IDelete,
        IEntitySetProvider,
        IEntityFactory
    { }

    //public partial interface IParallelEntityModel :
    //   IParallelQuery,
    //   IParallelInsert,
    //   IParallelUpdate,
    //   IParallelDelete,
    //   AP.IDisposable,
    //   IEntitySetProvider,
    //   IEntityFactory
    //{ }
}
