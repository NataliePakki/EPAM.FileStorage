using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Helpers;
using BLL.Interfaces.Entities;
using MvcPL.Models;

namespace MvcPL.Infrastructure.Mappers {
    public static  class UserRegisterMapper {
        public static RegisterViewModel ToRegisterMvcUser(this UserEntity userEntity) {
            return new RegisterViewModel() {
                Name = userEntity.Name,
                Email = userEntity.Email,
                Password = userEntity.Password,
                Roles = userEntity.Roles.ToMvcRoleCollection(),
            };
        }


        public static UserEntity ToBllUser(this RegisterViewModel userViewModel) {
            return new UserEntity() {
                Name = userViewModel.Name,
                Email = userViewModel.Email,
                Password = Crypto.HashPassword(userViewModel.Password),
                Photo = userViewModel.Photo != null ? Image.FromStream(userViewModel.Photo.InputStream) : null,
                Roles = userViewModel.Roles.ToRoleEntityCollection()
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