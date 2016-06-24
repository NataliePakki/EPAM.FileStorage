using System;
using System.Linq;
using System.Web.Security;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;

namespace MvcPL.Providers {
    public class CustomRoleProvider : RoleProvider {
        public IUserService UserService => (IUserService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IUserService));
        public IRoleService RoleService => (IRoleService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IRoleService));

        public override bool IsUserInRole(string email, string roleName) {
            var user = UserService.GetUserEntity(email);
            return user.Roles.Any(role => role != null && role.Name == roleName);
        }

        public override string[] GetRolesForUser(string email) {
            var user = UserService.GetUserEntity(email);
            return user?.Roles.Select(r => r.Name).ToArray() ?? new string[] {};
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