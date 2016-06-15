using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.Interfacies.DTO;
using DAL.Interfacies.Repository;
using DAL.Mappers;
using ORM;

namespace DAL.Concrete {
    public class FileRepository : IFileRepository {
        private readonly DbContext _context;

        public FileRepository(DbContext dbContext) {
            _context = dbContext;
        }
        public IEnumerable<DalFile> GetAll() {
            return _context.Set<File>().ToList().Select(file => file.ToDalFile());
        }

        public DalFile GetById(int id) {
            return  _context.Set<File>().FirstOrDefault(u => u.Id == id)?.ToDalFile();
            }

        public ICollection<DalFile> GetFilesByUserEmail(string userEmail) {
            var files = _context.Set<User>()
               .Where(u => u.Email == userEmail)
               .SelectMany(r => r.FileStorage).ToDalFileCollection();
            return files;
        }
        public ICollection<DalFile> GetFilesByUserId(int userId) {
            var files = _context.Set<User>()
               .Where(u => u.Id == userId)
               .SelectMany(r => r.FileStorage).ToDalFileCollection();
            return files;
        }

        public void Create(DalFile entity) {
            var file = entity.ToFile();
            _context.Set<File>().Add(file);
        }

        public void Delete(int id) {
            var file = _context.Set<File>().FirstOrDefault(f => f.Id == id);
            if(file == null)
                return;
            _context.Set<File>().Remove(file);
        }


        public void Update(DalFile entity) {
            var updatedFile = entity.ToFile();
            var existedFile = _context.Entry<File>(_context.Set<File>().Find(updatedFile.Id));
            if(existedFile == null) {
                return;
            }
            existedFile.State = EntityState.Modified;
            existedFile.Entity.Description = entity.Description;
            existedFile.Entity.IsShared = entity.IsShared;
        }

        public int GetId(DalFile file) {
            return _context.Set<File>().First(f => f.Name == file.Name).Id;
        }

        public IEnumerable<DalFile> GetFilesBySubstring(string subsrting) {
            var files = _context.Set<File>()
                .Where(a => a.Name.Contains(subsrting)).ToList()
                .Select(file => file.ToDalFile());
            return files;
        }


        public IEnumerable<DalFile> GetPublicFiles() {
           return _context.Set<File>().Where(file => file.IsShared).ToList().Select(file => file.ToDalFile());
        }
    }
}