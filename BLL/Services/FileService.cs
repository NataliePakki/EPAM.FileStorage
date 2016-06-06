using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfacies.Entities;
using BLL.Interfacies.Services;
using BLL.Mappers;
using DAL.Interfacies.Repository;

namespace BLL.Services {
    public class FileService : IFileService {
        private readonly IUnitOfWork _uow;
        private readonly IFileRepository _repository;

        private static string FileStorageDirectory(int id, string name) => AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + id + "_" + name;

        public FileService(IUnitOfWork uow, IFileRepository repository) {
            this._uow = uow;
            this._repository = repository;
        }
        public FileEntity GetFileEntity(int id) {
            return _repository.GetById(id).ToBllFile();
        }

        public IEnumerable<FileEntity> GetAllFileEntities() {
            return _repository.GetAll().Select(f => f.ToBllFile());
        }

        public IEnumerable<FileEntity> GetAllFileEntities(string userName) {
            return _repository.GetFilesByUserEmail(userName).Select(file => file.ToBllFile());
        }
        public IEnumerable<FileEntity> GetAllFileEntities(int userId) {
            return _repository.GetFilesByUserId(userId).Select(file => file.ToBllFile());
        }

        public IEnumerable<FileEntity> GetPublicFileEntities() {
            return _repository.GetPublicFiles().Select(file => file.ToBllFile());
        }

        public IEnumerable<FileEntity> GetFileEntitiesBySubstring(string substring) {
            return _repository.GetFilesBySubstring(substring).Select(file => file.ToBllFile());
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
                : System.IO.File.ReadAllBytes(FileStorageDirectory(file.Id, file.Name));
        }

        private void AddPhysicalFile(FileEntity file) {
            var id = _repository.GetId(file.ToDalFile());
            var name = file.Name;
            var fileBytes = file.FileBytes;
            System.IO.File.WriteAllBytes(FileStorageDirectory(id,name), fileBytes);
        }

        private static void DeletePhysicalFile(FileEntity file) {
            var id = file.Id;
            var name = file.Name;
            System.IO.File.Delete(FileStorageDirectory(id,name));
        }

    }
}