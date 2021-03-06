﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using BLL.Interfaces.Entities;
using MvcPL.Models;
using File = ORM.File;

namespace MvcPL.Infrastructure.Mappers {
    public static class FileMapper {
        public static FileViewModel ToMvcFile(this FileEntity fileEntity) {
            return new FileViewModel() {
                Id = fileEntity.Id,
                Name = fileEntity.Name,
                Description = fileEntity.Description,
                UserName = fileEntity.UserName,
                UserEmail = fileEntity.UserEmail,
                CreationDate = fileEntity.TimeAdded,
                Size = GetSizeString(fileEntity.Size),
                UserId = fileEntity.UserId,
                IsShared = fileEntity.IsShared
            };

        }
        public static FileEntity ToFileEntity(this CreateFileViewModel createFileViewModel) {
            var fileBase = createFileViewModel.File;
            return new FileEntity() {
                Name = fileBase.FileName,
                Description = createFileViewModel.Description,
                ContentType = fileBase.ContentType,
                Size = fileBase.ContentLength,
                IsShared = createFileViewModel.IsShared,
                UserId = createFileViewModel.UserId,
                TimeAdded = DateTime.Now,
                FileBytes = fileBase.ToFileBytes(),
            };
        }
        public static DeleteFileViewModel ToDeleteFileViewModel(this FileEntity fileEntity) {
            return new DeleteFileViewModel() {
                Id = fileEntity.Id,
                Name = fileEntity.Name,
                UserId = fileEntity.UserId,
            };

        }

        public static EditFileViewModel ToEditFileViewModel(this FileEntity fileEntity) {
            return new EditFileViewModel {
                Id = fileEntity.Id,
                Name = fileEntity.Name,
                Description = fileEntity.Description,
                IsShared = fileEntity.IsShared,
                UserId = fileEntity.UserId
                
            };
        }
        public static FileEntity ToFileEntity(this EditFileViewModel fileViewModel) {
            return new FileEntity(){
                Id = fileViewModel.Id,
                Description = fileViewModel.Description,
                IsShared = fileViewModel.IsShared
            };
        }


        public static TableViewModel ToTableViewModel(this List<FileEntity> files, int page, string searchString, int? userId = null) {
            int pageSize = 10;
            var pageInfo = new PageInfo {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = files.Count
            };
            return new TableViewModel() {
                SearchSubString = searchString,
                PageInfo = pageInfo,
                Files = files.Skip((pageInfo.PageNumber - 1) * pageInfo.PageSize).Take(pageSize).Select(f => f.ToMvcFile()).ToList(),
                UserId = userId
            };

        }
        public static ICollection<ORM.File> ToMvcFileCollection(this ICollection<FileEntity> files) {
            return files?.Select(file => new ORM.File() {
                Id = file.Id,
                Name = file.Name,
                UserId = file.UserId,
                TimeAdded = file.TimeAdded,
                IsShared = file.IsShared,
                ContentType = file.ContentType,
                Description = file.Description,
                Size = file.Size,
            }).ToList();

        }

        public static ICollection<FileEntity> ToFileEntityCollection(this ICollection<File> files) {
            return files?.Select(file => new FileEntity() {
                Id = file.Id,
                Name = file.Name,
                UserId = file.UserId,
                TimeAdded = file.TimeAdded,
                IsShared = file.IsShared,
                ContentType = file.ContentType,
                Description = file.Description,
                Size = file.Size,
            }).ToList();
        }

        public static Byte[] HttpPostedFileBaseToByteArray(this HttpPostedFileBase file) {

            Image image = file.HttpPostedFileBaseToImage();
            if(image != null) {
                using (var ms = new MemoryStream()) {
                    image.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            return null;
        }
        public static Image HttpPostedFileBaseToImage(this HttpPostedFileBase file) {
            return file != null ? Image.FromStream(file.InputStream) : null;
        }

        public static Byte[] ImageToByteArray(this Image imageIn) {
            if(imageIn != null) {
                using (var ms = new MemoryStream()) {
                    imageIn.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            return null;
        }

        public static bool IsImage(this HttpPostedFileBase file) {
            if(file.ContentType.ToLower() != "image/jpg" &&
                    file.ContentType.ToLower() != "image/jpeg" &&
                    file.ContentType.ToLower() != "image/png") {
                return false;
            }
            if(Path.GetExtension(file.FileName).ToLower() != ".jpg"
                && Path.GetExtension(file.FileName).ToLower() != ".png"
                && Path.GetExtension(file.FileName).ToLower() != ".jpeg") {
                return false;
            }
            return true;
        }


        private static string GetSizeString(long size) {
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

        private static byte[] ToFileBytes(this HttpPostedFileBase fileBase) {
            byte[] fileBytes;
            using(var ms = new MemoryStream()) {
                fileBase.InputStream.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            return fileBytes;
        }
    }
}