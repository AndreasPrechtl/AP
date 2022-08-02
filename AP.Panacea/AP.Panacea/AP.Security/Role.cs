using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.ComponentModel;

namespace AP.Security
{
    public class Role
    {
        private readonly string _name;

        public Role(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name");

            _name = name;
        }

        #region IRole Members

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return Membership.Context.Roles.GetDescription(_name); }
            set { Membership.Context.Roles.SetDescription(_name, value); }
        }

        public IQueryable<User> Users 
        {
            get { return Membership.Context.Roles.GetUsers(_name); }
        }
        
        public IQueryable<KeyValuePair<Activity, PermissionType>> Permissions
        {
            get { return Membership.Context.Roles.GetPermissions(_name); }
        }

        public bool Exists
        {
            get { return Membership.Context.Roles.Exists(_name); }
        }
        
        #endregion
    }
}
