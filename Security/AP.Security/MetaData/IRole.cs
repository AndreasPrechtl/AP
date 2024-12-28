using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.ComponentModel;

namespace AP.Security.MetaData
{
    public interface IRole
    {
        string Name { get; }
        string Description { get; set; }

        //ISetView<IUser> Users { get; }
        //IDictionaryView<IActivity, PermissionType> Permissions { get; }
    }
}
