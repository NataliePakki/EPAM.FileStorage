using BLL.Interfaces.Entities;
using MvcPL.Models;

namespace MvcPL.Infrastructure.Mappers {
    public static class UserMapper {
        public static UserEditViewModel ToMvcEditUserModel(this UserEntity userEntity) {
            return new UserEditViewModel() {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Email = userEntity.Email,
                OldPhoto = userEntity.Photo,
                Roles = userEntity.Roles.ToMvcRoleCollection()
            };
        }

        public static UserDetailsViewModel ToUserDetailsModel(this UserEntity userEntity) {
            return new UserDetailsViewModel() {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Email = userEntity.Email,
                IsBlocked = userEntity.IsBlocked,
                Roles = userEntity.Roles.ToMvcRoleCollection(),
                Photo = userEntity.Photo.ImageToByteArray()
            };
        }
    }
}

