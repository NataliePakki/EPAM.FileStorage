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
            return _repository.GetFilesByUserName(userName).Select(file => file.ToBllFile());
        }
        public IEnumerable<FileEntity> GetAllFileEntities(int userId) {
            return _repository.GetFilesByUserId(userId).Select(file => file.ToBllFile());
        }

        public IEnumerable<FileEntity> GetAllPublicFileEntities() {
            return _repository.GetPublicFiles().Select(file => file.ToBllFile());
        }

        public IEnumerable<FileEntity> FindFilesBySubstring(string s) {
            return _repository.SearchBySubstring(s).Select(file => file.ToBllFile());
        }

        public void CreateFile(FileEntity file) {
            _repository.Create(file.ToDalFile());
            _uow.Commit();
            AddPhysicalFile(file);
        }


        public void UpdateFile(FileEntity file) {
            _repository.Update(file.ToDalFile());
            _uow.Commit();
        }

        public void DeleteFile(int id) {
            var file = _repository.GetById(id).ToBllFile();
            _repository.Delete(id);
            _uow.Commit();
            DeletePhysicalFile(file);
        }

        public byte[] GetPhysicalFile(int id) {
            return _repository.GetPhysicalFile(id);
        }
        private void AddPhysicalFile(FileEntity file) {
            var id = _repository.GetId(file.ToDalFile());
            System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + id + "_" + file.Name, file.FileBytes);
        }

        private void DeletePhysicalFile(FileEntity file) {
            System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + file.Id + "_" + file.Name);
        }
    }
}