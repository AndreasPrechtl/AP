using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.Observable
{    
    public enum ChangeType
    {
        Add = 0,
        Remove = 1,
        Clear = 4
    }   

    public enum ListChangeType
    { 
        Add = 0,
        Remove = 1,
        Replace = 2,        
        Move = 3,
        Clear = 4,
        Insert = 5
    }

    public enum DictionaryChangeType
    {
        Add = 0,
        Remove = 1,
        Update = 2,
        Clear = 4
    }

    public enum SetChangeType
    {
        Add = 0,
        Union = 0,
        Remove = 1,
        Except = 1,
        Clear = 4,
        Intersect = 1,        
        SymmetricExcept = 2        
    }

    public enum QueueChangeType
    {
        Enqueue = 0,
        Dequeue = 1,
        Clear = 4
    }

    public enum StackChangeType
    {
        Push = 0,
        Pop = 1,
        Clear = 4
    }
}
