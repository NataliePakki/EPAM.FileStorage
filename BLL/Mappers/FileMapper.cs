using BLL.Interfacies.Entities;
using DAL.Interfacies.DTO;

namespace BLL.Mappers {
    public static class FileMapper {
        public static FileEntity ToBllFile(this DalFile dalFile) {
            if (dalFile == null)
                return null;
            return new FileEntity() {
                Id = dalFile.Id,
                TimeAdded = dalFile.TimeAdded,
                Name = dalFile.Name,
                UserId = dalFile.UserId,
                UserName = dalFile.UserName,
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
            return new DalFile() {
                Id = fileEntity.Id,
                TimeAdded = fileEntity.TimeAdded,
                Name = fileEntity.Name,
                UserId = fileEntity.UserId,
                UserName = fileEntity.UserName,
                IsShared = fileEntity.IsShared,
                Description = fileEntity.Description,
                ContentType = fileEntity.ContentType,
                FileBytes = fileEntity.FileBytes,
                Size = fileEntity.Size
            };
        }
    }
}