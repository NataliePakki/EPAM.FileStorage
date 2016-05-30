using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfacies.Entities;
using MvcPL.Models;

namespace MvcPL.Infrastructure.Mappers {
    public static class FileMapper {
        public static FileViewModel ToMvcFile(this FileEntity fileEntity) {
            return new FileViewModel() {
                Id = fileEntity.Id,
                Name = fileEntity.Name,
                UserName = fileEntity.UserName,
                CreationDate = fileEntity.TimeAdded,
                Description = fileEntity.Description,
                Size = GetSizeString(fileEntity.Size),
                UserId = fileEntity.UserId
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
        public static TableViewModel ToMvcTable(this List<FileEntity> files, PageInfo pageInfo, string searchString, int userId = 0) {
            return new TableViewModel() {
                SearchSubString = searchString,
                PageInfo = pageInfo,
                Files = files.Skip((pageInfo.PageNumber - 1) * pageInfo.PageSize).Take(10).Select(f => f.ToMvcFile()).ToList(),
                UserId = userId
            };

        }
        public static string GetSizeString(long size) {
            //byte
            if(size > 1024) {
                //kb
                if(size > Math.Pow(1024, 2)) {
                    //mb
                    if(size > Math.Pow(1024, 3)) {
                        return Math.Round(size / Math.Pow(1024, 3), 2) + "GB";
                    }
                    return Math.Round(size / Math.Pow(1024, 2), 2) + "MB";
                }
                return Math.Round((double)size / 1024, 2) + "KB";
            }
            return size + "B";
        }

        //public static FileEntity ToBllFile(this FileViewModel fileViewModel) {
        //    return new FileEntity() {
        //        Id = fileViewModel.Id,
        //        Name = fileViewModel.Name,
        //        UserName = fileViewModel.UserName,
        //        TimeAdded = DateTime.Now //TODO: CHANGE IT
        //    };
        //}
    }
}