
using System.Collections.Generic;
using System.Linq;
using DAL.Interfacies.DTO;
using ORM;

namespace DAL.Mappers {
    public static class DalFileMapper {
        public static DalFile ToDalFile(this File file) {
            if(file == null)
                return null;
            return new DalFile() {
                Id = file.Id,
                Name = file.Name,
                UserId = file.User.Id,
                UserName = file.User.Email,
                TimeAdded = file.TimeAdded,
                IsShared = file.IsShared,
                ContentType = file.ContentType,
                Description = file.Description,
                Size = file.Size,
            };
        }
        public static File ToFile(this DalFile dalFile) {
            if(dalFile == null)
                return null;
            return new File() {
                Id = dalFile.Id,
                Name = dalFile.Name,
                UserId = dalFile.UserId,
                TimeAdded = dalFile.TimeAdded,
                IsShared = dalFile.IsShared,
                ContentType = dalFile.ContentType,
                Description = dalFile.Description,
                Size = dalFile.Size,
                };
        }
        public static ICollection<DalFile> ToDalFileCollection(this IEnumerable<File> files) {
            var fileList = files?.Select(f => f.ToDalFile());
            return fileList?.ToList();
        }

        public static ICollection<File> ToFileCollection(this IEnumerable<DalFile> files) {
            var fileList = files?.Select(f => f.ToFile());
            return fileList?.ToList();
        }


    }
}
