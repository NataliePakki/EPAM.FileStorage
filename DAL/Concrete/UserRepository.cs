using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.Interfacies.DTO;
using DAL.Interfacies.Repository;
using DAL.Mappers;
using ORM;

namespace DAL.Concrete {
    public class UserRepository : IUserRepository {
        private readonly DbContext _context;
        private readonly IRoleRepository _roleRepository;
        private readonly IFileRepository _fileRepostory;

        public UserRepository(DbContext dbContext, IRoleRepository roleRepository, IFileRepository fileRepository) {
            _context = dbContext;
            _roleRepository = roleRepository;
            _fileRepostory = fileRepository;
        }

        public IEnumerable<DalUser> GetAll() {
            var users = _context.Set<User>().Select(user => new DalUser() {
                Name = user.Email,
                Id = user.Id,
                Password = user.Password
            }).ToList();

            foreach(var user in users) {
                user.Roles = _roleRepository.GetRolesByUserId(user.Id);
            }
            foreach(var user in users) {
                user.FileStorage = _fileRepostory.GetFilesByUserId(user.Id);
            }
            return users;

        }

        public DalUser GetById(int id) {
            var user = _context.Set<User>().FirstOrDefault(u => u.Id == id);
            var dalUser = user?.ToDalUser();
            if(dalUser == null) {
                return null;
            }
            dalUser.Roles = _roleRepository.GetRolesByUserId(dalUser.Id);
            dalUser.FileStorage = _fileRepostory.GetFilesByUserId(dalUser.Id);
            return dalUser;
        }


        public void Create(DalUser e) {
            var user = e.ToUser();
            user.Roles = user.Roles.Select(t => _context.Set<Role>().Find(t.Id)).ToList();
            AttachRoles(user.Roles);
            _context.Set<User>().Add(user);
        }


        public void Delete(DalUser entity) {
            var user = entity.ToUser();
            user.Roles = _roleRepository.GetRolesByUserId(user.Id).ToRoleCollection();
            user = _context.Set<User>().Single(u => u.Id == user.Id);
            _context.Set<User>().Remove(user);
            //foreach (var file in _fileRepostory.GetFilesByUserId(user.Id)) TODO: delete files, when delete user?
            //    _context.Set<File>().Remove(file.ToFile());
        }

        public void Update(DalUser entity) {
            var updatedUser = entity.ToUser();
            var existedUser = _context.Entry<User>(_context.Set<User>().Find(updatedUser.Id));
            if(existedUser == null) {
                return;
            }
            existedUser.State = EntityState.Modified;

            existedUser.Collection(u => u.Roles).Load();
            existedUser.Entity.Roles.Clear();

            foreach(Role role in updatedUser.Roles) {
                var loaded = _context.Set<Role>().Find(role.Id);
                existedUser.Entity.Roles.Add(loaded);
            }

            existedUser.Collection(u => u.FileStorage).Load();
            existedUser.Entity.FileStorage.Clear();

            foreach(File role in updatedUser.FileStorage) {
                var loaded = _context.Set<File>().Find(role.Id);
                existedUser.Entity.FileStorage.Add(loaded);
            }


            existedUser.Entity.Photo = entity.Photo;
            existedUser.Entity.Email = entity.Name;
            existedUser.Entity.Password = entity.Password;
        }


        public DalUser GetUserByEmail(string email) {
            var user = _context.Set<User>().FirstOrDefault(u => u.Email == email);
            var dalUser = user?.ToDalUser();
            if(dalUser == null) {
                return null;
            }
            dalUser.Roles = _roleRepository.GetRolesByUserId(user.Id);
            dalUser.FileStorage = _fileRepostory.GetFilesByUserId(user.Id);
            return dalUser;
        }
        private void AttachRoles(IEnumerable<Role> roles) {
            foreach(var role in roles) {
                var existedRole = _context.Set<Role>().Find(role.Id);
                _context.Set<Role>().Attach(existedRole);
            }
        }
    }
}
