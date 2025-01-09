using System;
using System.Collections.Generic;
using System.Linq;
using AP.Collections;

namespace AP.Security
{
    public abstract class MembershipContextBase
    {
        private sealed class UserMonitorEntry
        {
            private readonly AuthenticatorBase _authenticator;
            private readonly string _loginName;

            internal UserMonitorEntry(string loginName, AuthenticatorBase authenticator)
            {
                if (string.IsNullOrWhiteSpace(loginName))
                    throw new ArgumentException("loginName cannot be null or whitespace");

                ArgumentNullException.ThrowIfNull(authenticator);

                _loginName = loginName; 
                _authenticator = authenticator;
            }

            public AuthenticatorBase Authenticator
            {
                get { return _authenticator; }
            }

            public string LoginName
            {
                get { return _loginName; }
            }

            public override bool Equals(object? obj)
            {
                if (obj == null)
                    return false;

                if (obj == this)
                    return true;

                if (obj is UserMonitorEntry)
                {
                    UserMonitorEntry e = (UserMonitorEntry)obj;

                    return _authenticator.Equals(e._authenticator) && _loginName.Equals(e._loginName, StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return _loginName.GetHashCode();
            }
        }

        protected MembershipContextBase()
        {
            if (Membership.Context == null)
                Membership.Context = this;
        }
        
        public virtual bool IsAuthorized(User user, Activity activity)
        {
            if (!activity.Exists)
                return true;
            
            KeyValuePair<User, PermissionType> permissionEntry = this.Activities.GetUserPermissions(activity.Name).FirstOrDefault(p => p.Key.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase));

            if (permissionEntry.Key != null)
            {
                Activity? parent = null;

                while ((parent = Activities.GetParent(activity.Name)) != null)
                {
                    permissionEntry = this.Activities.GetUserPermissions(parent.Name).FirstOrDefault(p => p.Key.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase));
                
                    if (permissionEntry.Value == PermissionType.Deny)
                        return false;                    
                }                
            }

            return true;
        }

        private readonly Set<UserMonitorEntry> _userMonitor = new Set<UserMonitorEntry>();

        public bool IsAuthorized(string userName, string activityName)
        {
            return this.Users.GetPermissions(userName).Where(p => p.Value == PermissionType.Allow).Any();    
        }
        
        public bool TryLogin(string userName, string password)
        {
            try
            {
                this.Login(userName, password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsLoggedIn(string userName, AuthenticatorBase authenticator = null)
        {
            return _userMonitor.Contains(new UserMonitorEntry(userName, authenticator ?? this.DefaultAuthenticator));
        }

        public abstract AuthenticatorBase DefaultAuthenticator
        {
            get;
        }

        public User Login(string userName, string password, AuthenticatorBase authenticator = null)
        {
            authenticator = authenticator ?? this.DefaultAuthenticator;

            UserMonitorEntry me = new UserMonitorEntry(userName, authenticator);

            if (_userMonitor.Contains(me))
                return new User(userName);

            if (authenticator.TryAuthenticateByName(userName, password))
            {                
                _userMonitor.Add(me);
                return new User(userName);
            }

            throw new UserNotFoundException();
        }

        public User LoginByEmail(string emailAddress, string password, AuthenticatorBase authenticator = null)
        {
            authenticator = authenticator ?? this.DefaultAuthenticator;

            string userName = null;

            if (authenticator.TryAuthenticateByEmailAddress(emailAddress, password, out userName))
            {
                _userMonitor.Add(new UserMonitorEntry(userName, authenticator));

                return new User(userName);
            }

            throw new UserNotFoundException();
        }
        
        public bool Logout(string userName, AuthenticatorBase authenticator = null)
        {
            return _userMonitor.Remove(new UserMonitorEntry(userName, authenticator ?? this.DefaultAuthenticator));
        }
        
        public UserContextBase Users { get; set; }
        public RoleContextBase Roles { get; set; }
        public ActivityContextBase Activities { get; set; }
    }
}
