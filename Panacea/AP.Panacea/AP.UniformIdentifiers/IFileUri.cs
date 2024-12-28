using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.UniformIdentifiers
{
    public interface IFileUri : IHierarchicalUri
    {
        string Extension { get; }
        //string Name { get; }
    }
}
