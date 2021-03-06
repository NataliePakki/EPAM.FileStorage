﻿using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces.Entities;
using DAL.Interfaces.DTO;

namespace BLL.Mappers {
    public static class FileMapper {
        public static FileEntity ToBllFile(this DalFile dalFile) {
            if (dalFile == null)
                return null;
            return new FileEntity {
                Id = dalFile.Id,
                TimeAdded = dalFile.TimeAdded,
                Name = dalFile.Name,
                UserId = dalFile.UserId,
                UserName = dalFile.UserName,
                UserEmail = dalFile.UserEmail,
                IsShared = dalFile.IsShared,
                Description = dalFile.Description,
                ContentType = dalFile.ContentType,
                FileBytes = dalFile.FileBytes,
                Size = dalFile.Size
            };
        }

        public static DalFile ToDalFile(this FileEntity fileEntity) {
            if (fileEntity == null)
                return null;
            return new DalFile {
                Id = fileEntity.Id,
                TimeAdded = fileEntity.TimeAdded,
                Name = fileEntity.Name,
                UserId = fileEntity.UserId,
                UserName = fileEntity.UserName,
                UserEmail = fileEntity.UserEmail,
                IsShared = fileEntity.IsShared,
                Description = fileEntity.Description,
                ContentType = fileEntity.ContentType,
                FileBytes = fileEntity.FileBytes,
                Size = fileEntity.Size
            };
        }

        public static ICollection<DalFile> ToDalFileCollection(this ICollection<FileEntity> files) {
            var fileList = files?.Select(f => f.ToDalFile());
            return fileList?.ToList();
        }
    }
}