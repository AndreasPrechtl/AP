using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.ComponentModel;

namespace AP.Security.MetaData
{
    public interface IActivity
    {
        string Name { get; }
        string Description { get; set; }

        //IDictionaryView<IUser, PermissionType> UserPermissions { get; }
        //IDictionaryView<IRole, PermissionType> RolePermissions { get; }
    }
}
