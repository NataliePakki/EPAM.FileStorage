﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using BLL.Interfacies.Entities;
using MvcPL.Models;
using File = ORM.File;

namespace MvcPL.Infrastructure.Mappers {
    public static class UserMapper {

        public static UserEditViewModel ToMvcEditUserModel(this UserEntity userEntity) {
            return new UserEditViewModel() {
                Id = userEntity.Id,
                Email = userEntity.UserEmail,
                OldPhoto = userEntity.Photo,
                Roles = userEntity.Roles.ToMvcRoleCollection()
            };
        }
        public static UserViewModel ToMvcUser(this UserEntity userEntity) {
            return new UserViewModel() {
                Email = userEntity.UserEmail,
                Roles = userEntity.Roles.ToMvcRoleCollection(),
                Photo = userEntity.Photo,
                Files = userEntity.FileStorage.ToMvcFileCollection()
            };
        }

        public static ICollection<File> ToMvcFileCollection(this ICollection<FileEntity> files) {
            return files?.Select(file => new File() {
                Id = file.Id,
                Name = file.Name,
                UserId = file.UserId,
                IsPublic = file.IsPublic,
                Path = file.Path,
                TimeAdded = file.TimeAdded
            }).ToList();

        }

        public static ICollection<FileEntity> ToFileEntityCollection(this ICollection<File> files) {
            return files?.Select(file => new FileEntity() {
                Id = file.Id,
                Name = file.Name,
                UserName = file.User.Email,
                UserId = file.UserId,
                IsPublic = file.IsPublic,
                Path = file.Path,
                TimeAdded = file.TimeAdded
            }).ToList();
        }

        public static  Byte[] HttpPostedFileBaseToByteArray(this HttpPostedFileBase file) {

            Image image = file.HttpPostedFileBaseToImage();
            if(image != null) {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
            return null;
        }
        public static Image HttpPostedFileBaseToImage(this HttpPostedFileBase file) {
            return file != null ? Image.FromStream(file.InputStream) : null;
        }
        public static Byte[] ImageToByteArray(this Image imageIn) {
            if(imageIn != null) {
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
            return null;
        }
    }
}
       
