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

        public UserRepository(DbContext dbContext) {
            _context = dbContext;
        }
        public IEnumerable<DalUser> GetAll() {
            return _context.Set<User>().ToList().Select(user => user.ToDalUser()).ToList();

        }

        public DalUser Get(int id) {
            return _context.Set<User>().FirstOrDefault(u => u.Id == id)?.ToDalUser();
        }


        public void Create(DalUser e) {
            var user = e.ToUser();
            user.Roles = user.Roles.Select(t => _context.Set<Role>().Find(t.Id)).ToList();
            AttachRoles(user.Roles);
            _context.Set<User>().Add(user);
        }


        public void Delete(int id) {
            var user = _context.Set<User>().Single(u => u.Id == id);
           _context.Set<User>().Remove(user);
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
            updatedUser.FileStorage = existedUser.Entity.FileStorage.ToList();
            existedUser.Collection(u => u.FileStorage).Load();
            existedUser.Entity.FileStorage.Clear();

            foreach(File file in updatedUser.FileStorage) {
                var loaded = _context.Set<File>().Find(file.Id);
                existedUser.Entity.FileStorage.Add(loaded);
            }

            existedUser.Entity.Photo = entity.Photo;
            existedUser.Entity.Email = entity.Name;
            existedUser.Entity.Password = entity.Password;
            existedUser.Entity.IsBlocked = entity.IsBlocked;
        }


        public DalUser GetUserByEmail(string email) {
            var user = _context.Set<User>().FirstOrDefault(u => u.Email == email);
            var dalUser = user?.ToDalUser();
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
