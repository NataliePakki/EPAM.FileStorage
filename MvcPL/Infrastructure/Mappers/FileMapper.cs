using System.Collections.Generic;
using System.Linq;
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
        public static TableViewModel ToMvcTable(this List<FileEntity> files, PageInfo pageInfo) {
            return new TableViewModel() {
                PageInfo = pageInfo,
                Files = files.Skip((pageInfo.PageNumber - 1) * pageInfo.PageSize).Take(10).Select(f => f.ToMvcFile()).ToList()
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