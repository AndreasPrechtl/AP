using System;
using System.Collections.Generic;
using System.Linq;
using AP.Security.MetaData;

namespace AP.Security
{
    public class User : IUser
    {
        private readonly string _name;

        public User(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name");

            _name = name;
        }

        #region IUser Members

        public string Name
        {
            get { return _name; }
        }

        public string DisplayName
        {
            get { return Membership.Context.Users.GetDisplayName(_name) ?? _name; }
            set { Membership.Context.Users.SetDisplayName(_name); }
        }

        public string Description
        {
            get { return Membership.Context.Users.GetDescription(_name); }
            set { Membership.Context.Users.SetDescription(_name, value); }
        }

        public bool IsLoggedIn
        {
            get { return Membership.Context.IsLoggedIn(_name); }
        }

        public void SetPassword(string currentPassword, string newPassword)
        {
            Membership.Context.Users.SetPassword(_name, currentPassword, newPassword);
        }

        public string EmailAddress
        {
            get { return Membership.Context.Users.GetEmailAddress(_name); }
            set { Membership.Context.Users.SetEmailAddress(_name, value); }
        }

        public IQueryable<Role> Roles 
        { 
            get { return Membership.Context.Users.GetRoles(_name); }
        }

        public IQueryable<KeyValuePair<Activity, PermissionType>> Permissions
        {
            get { return Membership.Context.Users.GetPermissions(_name); }
        }

        public bool Exists
        {
            get { return Membership.Context.Users.Exists(_name); }
        }

        #endregion
    }
}
