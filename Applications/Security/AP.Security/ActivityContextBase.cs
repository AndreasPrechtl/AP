using System.Collections.Generic;
using AP.Data;
using System.Linq;

namespace AP.Security
{
    public abstract class ActivityContextBase : IContextPartInternal<Activity>, IQuery<Activity>
    {
        public Activity Create(string name, string description, IEnumerable<KeyValuePair<User, PermissionType>> users, IEnumerable<KeyValuePair<Role, PermissionType>> roles)
        {
            return this.Create(name, description,
                               users != null ? users.Select(u => new KeyValuePair<string, PermissionType>(u.Key.Name, u.Value)) : [],
                               roles != null ? roles.Select(r => new KeyValuePair<string, PermissionType>(r.Key.Name, r.Value)) : []
                              );
        }

        public Activity Create(string name, string description)
        {
            return this.Create(name, description, (IEnumerable<KeyValuePair<string, PermissionType>>)null!, null!);
        }

        public Activity Create(string name, string description, IEnumerable<KeyValuePair<string, PermissionType>> users, IEnumerable<KeyValuePair<string, PermissionType>> roles)
        {
            return this.OnCreate(name, description, users ?? [], roles ?? []);
        }

        protected abstract Activity OnCreate(string name, string description, IEnumerable<KeyValuePair<string, PermissionType>> users, IEnumerable<KeyValuePair<string, PermissionType>> roles);

        public abstract void Rename(string activityName, string name);
        
        public abstract void SetDescription(string activityName, string description);
        public abstract string GetDescription(string activityName);

        public abstract IQueryable<KeyValuePair<User, PermissionType>> GetUserPermissions(string activityName);
        public abstract IQueryable<KeyValuePair<Role, PermissionType>> GetRolePermissions(string activityName);

        public abstract void SetUserPermission(string activityName, string userName, PermissionType permission);
        public abstract void SetRolePermission(string activityName, string roleName, PermissionType permission);

        public abstract void DeleteUserPermission(string activityName, string userName);
        public abstract void DeleteRolePermission(string activityName, string roleName);

        public abstract Activity? GetParent(string activityName);
        public abstract IQueryable<Activity> GetChildren(string activityName);

        public abstract void Delete(string activityName);

        public bool Exists(string activityName)
        {
            return this.Query().Any(p => p.Name.Equals(activityName, System.StringComparison.OrdinalIgnoreCase));
        }
        
        #region IQuery<IActivity> Members

        public abstract IQueryable<Activity> Query();

        #endregion
    }
}
