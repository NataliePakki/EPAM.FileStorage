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

        public static UserDetailsViewModel ToUserDetailsModel(this UserEntity userEntity) {
            return new UserDetailsViewModel() {
                Email = userEntity.UserEmail,
                Id = userEntity.Id,
                IsBlocked = userEntity.IsBlocked,
                Roles = userEntity.Roles.ToMvcRoleCollection(),
                Photo = userEntity.Photo.ImageToByteArray()
            };
        }
    

       
    }
}
       
