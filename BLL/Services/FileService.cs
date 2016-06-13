using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BLL.Interfacies.Entities;
using BLL.Interfacies.Services;
using BLL.Mappers;
using DAL.Interfacies.Repository;

namespace BLL.Services {
    public class FileService : IFileService {
        private readonly IFileRepository _repository;
        private readonly IUnitOfWork _uow;

        public FileService(IUnitOfWork uow, IFileRepository repository) {
            _uow = uow;
            _repository = repository;
        }

        public FileEntity GetFileEntity(int id) {
            return _repository.GetById(id).ToBllFile();
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

        public IEnumerable<FileEntity> GetAllFiles(string userName) {
            return _repository.GetFilesByUserEmail(userName).Select(file => file.ToBllFile());
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
            AddPhysicalFile(fileEntity);
        }


        public void UpdateFile(FileEntity fileEntity) {
            _repository.Update(fileEntity.ToDalFile());
            _uow.Commit();
        }

        public void DeleteFile(int id) {
            var file = _repository.GetById(id).ToBllFile();
            _repository.Delete(id);
            _uow.Commit();
            DeletePhysicalFile(file);
        }

        public byte[] GetPhysicalFile(int id) {
            var file = _repository.GetById(id);
            return (file == null)
                ? null
                : File.ReadAllBytes(FileStorageDirectory(file.Id, file.Name));
        }

        private static string FileStorageDirectory(int id, string name) => AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + id + "_" + name;

        private void AddPhysicalFile(FileEntity file) {
            var id = _repository.GetId(file.ToDalFile());
            var name = file.Name;
            var fileBytes = file.FileBytes;
            File.WriteAllBytes(FileStorageDirectory(id, name), fileBytes);
        }

        private static void DeletePhysicalFile(FileEntity file) {
            var id = file.Id;
            var name = file.Name;
            File.Delete(FileStorageDirectory(id, name));
        }
    }
}