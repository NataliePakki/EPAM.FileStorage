using System.Collections.Generic;
using BLL.Interfacies.Entities;

namespace BLL.Interfacies.Services {
    public interface IFileService {
        FileEntity GetFileEntity(int id);
        IEnumerable<FileEntity> GetAllFileEntities();
        IEnumerable<FileEntity> GetPublicFileEntities();
        IEnumerable<FileEntity> GetAllFileEntities(string userName);
        IEnumerable<FileEntity> GetAllFileEntities(int userId);
        IEnumerable<FileEntity> GetFileEntitiesBySubstring(string substring);
        void CreateFile(FileEntity fileEntity);
        void UpdateFile(FileEntity fileEntity);
        void DeleteFile(int id);
        byte[] GetPhysicalFile(int id);
    }
}