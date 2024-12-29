using System;
using System.Collections.Generic;
using System.Linq;

namespace AP.Security
{
    public abstract class UserContextBase : IContextPartInternal<User>
    {
        public abstract void Rename(string name, string newUserName);

        public abstract void SetPassword(string userName, string currentPassword, string newPassword);
        public abstract void SetEmailAddress(string userName, string emailAddress);
        public abstract void SetDescription(string userName, string description);
        public abstract void SetDisplayName(string userName);
        
        /// <summary>
        /// Returns the password in plain text or a hash.
        /// </summary>
        /// <param name="userName">The user's name.</param>
        /// <returns>The plain text password or a hash.</returns>
        public abstract string GetPassword(string userName);
        public abstract string GetEmailAddress(string userName);
        public abstract string GetDescription(string userName);
        public abstract string GetDisplayName(string userName);

        public abstract IQueryable<Role> GetRoles(string userName);
        public abstract IQueryable<KeyValuePair<Activity, PermissionType>> GetPermissions(string userName);

        public abstract void Delete(string userName);

        public bool Exists(string userName)
        {
            return this.Query().Any(p => p.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        public abstract User Create(string userName, string emailAddress, string password, string displayName);
        
        #region IQuery<User> Members
        
        public abstract IQueryable<User> Query();

        #endregion
    }
}
