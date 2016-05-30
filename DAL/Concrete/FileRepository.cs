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

        public void Delete(int id) {
            var file = _context.Set<File>().FirstOrDefault(f => f.Id == id);
            if(file == null)
                return;
            _context.Set<File>().Remove(file);
        }


        public void Update(DalFile entity) {
            var oldFile = _context.Set<File>().FirstOrDefault(f => f.Id == entity.Id);
            if(oldFile == null)
                return;
            oldFile.Description = entity.Description;
            oldFile.IsPublic = entity.IsPublic;
        }

        public int GetId(DalFile file) {
            return _context.Set<File>().First(f => f.Name == file.Name).Id;
        }

        public IEnumerable<DalFile> SearchBySubstring(string subsrting) {
            var files = _context.Set<File>()
                .Where(a => a.Name.Contains(subsrting))
                .Select(file => new DalFile() {
                    Id = file.Id,
                    Name = file.Name,
                    IsPublic = file.IsPublic,
                    TimeAdded = file.TimeAdded,
                    UserId = file.UserId,
                    UserName = file.User.Email,
                    Description = file.Description,
                    Size = file.Size,
                    ContentType = file.ContentType
                    });
            return files;
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
                UserName = file.User.Email,
                ContentType = file.ContentType,
                Size = file.Size,
                Description = file.Description
            });
        }

        public byte[] GetPhysicalFile(int id) {
            var file = _context.Set<File>().FirstOrDefault(f => f.Id == id);
            return (file == null)
                ? null
                : System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Storage/" + file.Id +
                                              "_" + file.Name);
        }

    }
}