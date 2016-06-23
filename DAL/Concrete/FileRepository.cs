using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using DAL.Interfacies.DTO;
using DAL.Interfacies.Repository;
using DAL.Mappers;
using File = ORM.File;

namespace DAL.Concrete {
    public class FileRepository : IFileRepository {
        private readonly DbContext _context;

        public FileRepository(DbContext dbContext) {
            _context = dbContext;
        }
        public IEnumerable<DalFile> GetAll() {
            return _context.Set<File>().ToList().Select(file => file.ToDalFile());
        }

        public DalFile Get(int id) {
            return  _context.Set<File>().Find(id)?.ToDalFile();
            }


        public IEnumerable<DalFile> GetFilesByUserId(int userId) {
            var files = _context.Set<File>()
               .Where(u => u.UserId == userId).ToList().Select(file=>file.ToDalFile());
            return files;
        }

        public void Create(DalFile entity) {
            entity.FileGuid = Guid.NewGuid().ToString();
            var file = entity.ToFile();
            _context.Set<File>().Add(file);
            AddPhysicalFile(entity);
        }

        public void Delete(int id) {
            var file = _context.Set<File>().Find(id);
            if(file == null)
                return;
            DeletePhysicalFile(file.ToDalFile());
            _context.Set<File>().Remove(file);
            
        }


        public void Update(DalFile entity) {
            var existedFile = _context.Entry<File>(_context.Set<File>().Find(entity.Id));
            if(existedFile == null) {
                return;
            }
            existedFile.State = EntityState.Modified;
            existedFile.Entity.Description = entity.Description;
            existedFile.Entity.IsShared = entity.IsShared;
        }

//        public int GetId(DalFile file) {
//            return _context.Set<File>().Where(f => f.Name == file.Name && f.ContentType == file.ContentType && f.UserId == file.UserId && f.Description == file.Description && f.Size == file.Size).ToList().First(f => Math.Abs((f.TimeAdded - file.TimeAdded).TotalMilliseconds) < 10.0).Id;
//        }

        public IEnumerable<DalFile> GetFilesBySubstring(string subsrting) {
            var files = _context.Set<File>()
                .Where(a => a.Name.Contains(subsrting) || a.Description.Contains(subsrting)).ToList()
                .Select(file => file.ToDalFile());
            return files;
        }


        public IEnumerable<DalFile> GetPublicFiles() {
           return _context.Set<File>().Where(file => file.IsShared).ToList().Select(file => file.ToDalFile());
        }
        public byte[] GetPhysicalFile(int id) {
            var file = _context.Set<File>().Find(id)?.ToDalFile();
            if(file == null)
                return null;
            try {
                return System.IO.File.ReadAllBytes(FileStorageDirectory(file.FileGuid, file.Name));
            } catch(FileNotFoundException) {
                return null;
            }
        }
        private static string FileStorageDirectory(string guid, string name) => $"{AppDomain.CurrentDomain.BaseDirectory}/App_Data/Storage/{guid}_{name}";

        private void AddPhysicalFile(DalFile file) {
            var quid = file.FileGuid;
            var name = file.Name;
            var fileBytes = file.FileBytes;
            System.IO.File.WriteAllBytes(FileStorageDirectory(quid, name), fileBytes);
        }

        private static void DeletePhysicalFile(DalFile file) {
            var quid = file.FileGuid;
            var name = file.Name;
            System.IO.File.Delete(FileStorageDirectory(quid, name));
        }
    }
}