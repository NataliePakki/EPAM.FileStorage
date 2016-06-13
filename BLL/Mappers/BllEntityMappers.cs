using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BLL.Interfacies.Entities;
using DAL.Interfacies.DTO;

namespace BLL.Mappers {
    public static class BllEntityMappers {
        public static DalUser ToDalUser(this UserEntity userEntity) {
            if (userEntity == null)
                return null;
            return new DalUser {
                Id = userEntity.Id,
                Name = userEntity.UserEmail,
                Password = userEntity.Password,
                IsBlocked = userEntity.IsBlocked,
                Roles = userEntity.Roles.ToDalRoleCollection(),
                Photo = userEntity.Photo.ImageToByteArray(),
                FileStorage = userEntity.FileStorage.ToDalFileCollection()
            };
        }

        public static UserEntity ToBllUser(this DalUser dalUser) {
            if (dalUser == null)
                return null;
            return new UserEntity {
                Id = dalUser.Id,
                UserEmail = dalUser.Name,
                Password = dalUser.Password,
                IsBlocked = dalUser.IsBlocked,
                Photo = dalUser.Photo.ByteArrayToImage(),
                Roles = dalUser.Roles.ToRoleEntityCollection()
            };
        }

        public static DalRole ToDalRole(this RoleEntity roleEntity) {
            if (roleEntity == null)
                return null;
            return new DalRole {
                Id = roleEntity.Id,
                Name = roleEntity.Name
            };
        }

        public static RoleEntity ToBllRole(this DalRole dalRole) {
            if (dalRole == null)
                return null;
            return new RoleEntity {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }

        public static ICollection<DalRole> ToDalRoleCollection(this ICollection<RoleEntity> roles) {
            var roleList = roles?.Select(r => new DalRole {
                Id = r.Id,
                Name = r.Name
            });
            return roleList?.ToList();
        }

        public static ICollection<RoleEntity> ToRoleEntityCollection(this ICollection<DalRole> roles) {
            var roleList = roles?.Select(r => new RoleEntity {
                Id = r.Id,
                Name = r.Name
            });
            return roleList?.ToList();
        }

        public static byte[] ImageToByteArray(this Image imageIn) {
            if (imageIn != null) {
                var ms = new MemoryStream();
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
            return null;
        }

        public static Image ByteArrayToImage(this byte[] byteArrayIn) {
            if (byteArrayIn != null) {
                var ms = new MemoryStream(byteArrayIn);
                var returnImage = Image.FromStream(ms);
                return returnImage;
            }
            return null;
        }
    }
}