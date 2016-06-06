using System;
using System.Linq;
using System.Web.Security;
using BLL.Interfacies.Entities;
using BLL.Interfacies.Services;

namespace MvcPL.Providers {
    public class CustomRoleProvider : RoleProvider {
        public IUserService UserService => (IUserService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IUserService));
        public IRoleService RoleService => (IRoleService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IRoleService));

        public override bool IsUserInRole(string email, string roleName) {
            var user = UserService.GetUserEntity(email);

            if(user == null)
                return false;
            var userRoles = RoleService.GetAllRolesByUserId(user.Id);
            if(userRoles != null) {
                foreach(RoleEntity role in userRoles) {
                    if(role != null && role.Name == roleName) {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string[] GetRolesForUser(string email) {
            var roles = new string[] { };
            var user = UserService.GetUserEntity(email);

            if(user == null)
                return roles;
            var userRoles = RoleService.GetAllRolesByUserId(user.Id);
            if(userRoles != null) {
                roles = userRoles.Select(r => r.Name).ToArray();
            }
            return roles;
        }

        public override void CreateRole(string roleName) {
            var role = new RoleEntity() { Name = roleName };
            RoleService.CreateRole(role);
        }

        #region Stabs

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName) {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName) {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles() {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
        #endregion
    }
}