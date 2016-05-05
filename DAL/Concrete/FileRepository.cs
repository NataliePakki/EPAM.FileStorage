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
            return _context.Set<File>().Select(file => new DalFile() {
                Id = file.Id,
                Name = file.Name,
                IsPublic = file.IsPublic,
                TimeAdded = file.TimeAdded,
                UserId = file.UserId,
                UserName = file.User.Email

                });
        }

        public DalFile GetById(int id) {
            var file = _context.Set<File>().FirstOrDefault(u => u.Id == id);
            var dalFile = file?.ToDalFile();
            return dalFile;

        }


        public void Create(DalFile entity) {
            var file = entity.ToFile();
            _context.Set<File>().Add(file);
        }

        public void Delete(DalFile e) {
            var file = e.ToFile();
            file = _context.Set<File>().Single(u => u.Id == file.Id);
            _context.Set<File>().Remove(file);
        }


        public void Update(DalFile entity) {
            throw new NotImplementedException();
        }

        public IEnumerable<DalFile> SearchBySubstring(string subsrting) {
            return _context.Set<File>()
                .Where(a => a.Name.Contains(subsrting))
                .Select(file => file.ToDalFile());
        }

        public ICollection<DalFile> GetFilesByUserName(string userName) {
            var files = _context.Set<User>()
               .Where(u => u.Email == userName)
               .SelectMany(r => r.FileStorage).ToDalFileCollection();
            return files;
        }
        public ICollection<DalFile> GetFilesByUserId(int userId) {
            var files = _context.Set<User>()
               .Where(u => u.Id == userId)
               .SelectMany(r => r.FileStorage).ToDalFileCollection();
            return files;
        }
        public IEnumerable<DalFile> GetPublicFiles() {
            return _context.Set<File>().Where(file => file.IsPublic).Select(file => new DalFile() {
                Id = file.Id,
                Name = file.Name,
                IsPublic = file.IsPublic,
                TimeAdded = file.TimeAdded,
                UserId = file.UserId,
                UserName = file.User.Email
                
            });
        }
    }
}