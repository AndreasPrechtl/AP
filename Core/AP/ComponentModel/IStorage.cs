using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.ComponentModel
{
    public interface IStorage
    {
        bool Commit();
        void Rollback();
    }

    public interface IStorageUser
    {
        IStorage Storage { get; }
    }
}
