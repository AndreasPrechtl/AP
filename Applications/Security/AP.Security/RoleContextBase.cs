using AP.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AP.Security
{
    public abstract class RoleContextBase : IContextPartInternal<Role>, IQuery<Role>
    {
        public abstract void Rename(string roleName, string newRoleName);
        
        public abstract string GetDescription(string roleName); 
        public abstract void SetDescription(string roleName, string description);

        public abstract IQueryable<User> GetUsers(string roleName);
        public abstract IQueryable<KeyValuePair<Activity, PermissionType>> GetPermissions(string roleName);
               
        public bool Exists(string roleName)
        {
            return this.Query().Any(p => p.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public abstract void Delete(string roleName);
        
        #region IQuery<Role> Members

        public abstract IQueryable<Role> Query();

        #endregion
    }
}
