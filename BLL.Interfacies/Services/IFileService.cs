using System.Collections.Generic;
using BLL.Interfacies.Entities;

namespace BLL.Interfacies.Services {
    public interface IFileService {
        FileEntity GetFileEntity(int id);
        IEnumerable<FileEntity> GetAllFileEntities();
        IEnumerable<FileEntity> GetAllFileEntities(string userName);
        IEnumerable<FileEntity> GetAllFileEntities(int userId);
        IEnumerable<FileEntity> GetAllPublicFileEntities();
        IEnumerable<FileEntity> FindFilesBySubstring(string s);
        void CreateFile(FileEntity file);
        void UpdateFile(FileEntity file);
        void DeleteFile(FileEntity file);

    }
}