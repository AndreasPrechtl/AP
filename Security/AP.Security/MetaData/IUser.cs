using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.ComponentModel;

namespace AP.Security.MetaData
{
    public interface IUser
    {
        string Name { get; }
        string EmailAddress { get; }
        string DisplayName { get; }

        //ISetView<IRole> Roles { get; }
        //IDictionaryView<IActivity, PermissionType> Permissions { get; }
    }
}
