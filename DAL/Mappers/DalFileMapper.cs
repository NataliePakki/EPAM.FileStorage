
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
                Path = file.Path,
                UserId = file.UserId,
                UserName = file.User.Email,
                TimeAdded = file.TimeAdded,
                IsPublic = file.IsPublic
            };
        }
        public static File ToFile(this DalFile dalFile) {
            if(dalFile == null)
                return null;
            return new File() {
                Id = dalFile.Id,
                Name = dalFile.Name,
                Path = dalFile.Path,
                UserId = dalFile.UserId,
                TimeAdded = dalFile.TimeAdded,
                IsPublic = dalFile.IsPublic
            };
        }
        public static ICollection<DalFile> ToDalFileCollection(this IEnumerable<File> files) {
            var fileList = files.Select(f => f.ToDalFile());//TODO: if fileList = null?
            return fileList.ToList();
        }

        public static ICollection<File> ToFileCollection(this IEnumerable<DalFile> files) {
            var fileList = files.Select(f => f.ToFile());//TODO: if fileList = null?
            return fileList.ToList();
        }


    }
}
