using System;
using System.Collections.Generic;
using System.Linq;

namespace AP.Security
{
    public class Activity 
    {
        private readonly string _name;

        public Activity(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name");

            _name = name;
        }

        #region IActivity Members

        public object Id
        {
            get;
            private set;
        }

        public string Name
        {
            get { return _name; }
        }
        
        public string Description
        {
            get { return Membership.Context.Activities.GetDescription(_name); }
            set { Membership.Context.Activities.SetDescription(_name, value); }
        }

        public IQueryable<KeyValuePair<User, PermissionType>> UserPermissions
        { 
            get { return Membership.Context.Activities.GetUserPermissions(_name); }
        }
        
        public IQueryable<KeyValuePair<Role, PermissionType>> RolePermissions
        {
            get { return Membership.Context.Activities.GetRolePermissions(_name); }
        }
        
        public bool Exists
        {
            get { return Membership.Context.Activities.Exists(_name); }
        }

        #endregion
    }
}
