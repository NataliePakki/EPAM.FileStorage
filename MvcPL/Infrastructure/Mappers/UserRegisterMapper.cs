using System.Collections.Generic;
using System.Linq;
using BLL.Interfacies.Entities;
using MvcPL.Models;

namespace MvcPL.Infrastructure.Mappers {
    public static  class UserRegisterMapper {
        public static RegisterViewModel ToRegisterMvcUser(this UserEntity userEntity) {
            return new RegisterViewModel() {
                Email = userEntity.UserEmail,
                Password = userEntity.Password,
                Roles = userEntity.Roles.ToMvcRoleCollection(),
            };
        }


        public static UserEntity ToBllUser(this RegisterViewModel userViewModel) {
            return new UserEntity() {
                UserEmail = userViewModel.Email,
                Password = userViewModel.Password,
                Roles = userViewModel.Roles.ToRoleEntityCollection(),
            };
        }

        public static ICollection<Role> ToMvcRoleCollection(this ICollection<RoleEntity> roles) {
            var roleList = roles.Select(r => (Role)r.Id);
            return roleList.ToList();
        }

        public static ICollection<RoleEntity> ToRoleEntityCollection(this ICollection<Role> roles) {
            var roleList = roles.Select(r => new RoleEntity() {
                Id = (int)r,
                Name = r.ToString()
            });
            return roleList.ToList();
        }
    }
}