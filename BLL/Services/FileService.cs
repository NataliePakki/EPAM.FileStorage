using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using DAL.Interfaces.Repository;

namespace BLL.Services {
    public class FileService : IFileService {
        private readonly IFileRepository _repository;
        private readonly IUnitOfWork _uow;

        public FileService(IUnitOfWork uow, IFileRepository repository) {
            _uow = uow;
            _repository = repository;
        }

        public FileEntity GetFileEntity(int id) {
            return _repository.Get(id).ToBllFile();
        }

        public IEnumerable<FileEntity> GetFiles(string substring = null, int? userId = null) {
            if(!string.IsNullOrEmpty(substring)) 
                return GetFilesBySubstring(substring, userId);
            if(userId.HasValue)
                return _repository.GetFilesByUserId(userId.Value).Select(file => file.ToBllFile());
            return _repository.GetPublicFiles().Select(file => file.ToBllFile());
        }

        public IEnumerable<FileEntity> GetAllFiles() {
            return _repository.GetAll().Select(f => f.ToBllFile());
        }

        public IEnumerable<FileEntity> GetAllFiles(int userId) {
            return _repository.GetFilesByUserId(userId).Select(file => file.ToBllFile());
        }

        public IEnumerable<FileEntity> GetPublicFiles() {
            return _repository.GetPublicFiles().Select(file => file.ToBllFile());
        }

        public IEnumerable<FileEntity> GetFilesBySubstring(string substring, int? userId = null) {
            var findFiles = _repository.GetFilesBySubstring(substring);
            if(userId.HasValue)
                findFiles = findFiles.Where(file => file.UserId == userId);
            else
                findFiles = findFiles.Where(file => file.IsShared);
            return findFiles.Select(file => file.ToBllFile());
        }

        public void CreateFile(FileEntity fileEntity) {
            _repository.Create(fileEntity.ToDalFile());
            _uow.Commit();
        }


        public void UpdateFile(FileEntity fileEntity) {
            _repository.Update(fileEntity.ToDalFile());
            _uow.Commit();
        }

        public void DeleteFile(int id) {
            _repository.Delete(id);
            _uow.Commit();
        }

        public byte[] GetPhysicalFile(int id) {
            return _repository.GetPhysicalFile(id);
        }
   }
}