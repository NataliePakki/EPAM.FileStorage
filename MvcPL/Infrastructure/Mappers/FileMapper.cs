using System;
using BLL.Interfacies.Entities;
using MvcPL.Models;

namespace MvcPL.Infrastructure.Mappers {
    public static class FileMapper {
        public static FileViewModel ToMvcFile(this FileEntity fileEntity) {
            return new FileViewModel() {
                Name = fileEntity.Name,
                UserName = fileEntity.UserName,
                CreationDate = fileEntity.TimeAdded
            };

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