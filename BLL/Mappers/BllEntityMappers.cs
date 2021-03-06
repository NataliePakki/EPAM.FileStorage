﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BLL.Interfaces.Entities;
using DAL.Interfaces.DTO;

namespace BLL.Mappers {
    public static class BllEntityMappers {
        public static DalUser ToDalUser(this UserEntity userEntity) {
            if (userEntity == null)
                return null;
            return new DalUser {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Email = userEntity.Email,
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
                Name = dalUser.Name,
                Email = dalUser.Email,
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
            if (imageIn == null) return null;
            using (var mstream = new MemoryStream()) {
                imageIn.Save(mstream,ImageFormat.Jpeg);
                mstream.ToArray();
            }
            return null;
        }

        public static Image ByteArrayToImage(this byte[] byteArrayIn) {
            if (byteArrayIn == null) return null;
            using (MemoryStream mstream = new MemoryStream(byteArrayIn)) {
                return Image.FromStream(mstream);
            }
        }
    }
}