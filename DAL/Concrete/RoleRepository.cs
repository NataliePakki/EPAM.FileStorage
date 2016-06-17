using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.Interfacies.DTO;
using DAL.Interfacies.Repository;
using DAL.Mappers;
using ORM;

namespace DAL.Concrete {
    public class RoleRepository : IRoleRepository {
        private readonly DbContext _context;

        public RoleRepository(DbContext dbContext) {
            _context = dbContext;
        }

        public IEnumerable<DalRole> GetAll() {
            return _context.Set<Role>().Select(role => role.ToDalRole());
        }


        public DalRole Get(int id) {
            var role = _context.Set<Role>().Find(id);
            return role != null ? new DalRole {
                Id = role.Id,
                Name = role.Name
            }
                : null;
        }


        public void Create(DalRole entity) {
            var role = new Role {
                Name = entity.Name
            };
            _context.Set<Role>().Add(role);
        }


        public void Update(DalRole entity) {
            throw new NotImplementedException();
        }

        public IEnumerable<DalRole> GetRolesByUserId(int id) {
            var roles = _context.Set<User>()
                .Where(u => u.Id == id)
                .SelectMany(r => r.Roles).Select(r => r.ToDalRole());
            return roles;
        }

        public void Delete(int id) {
            var role = _context.Set<Role>().Single(r => r.Id == id);
            if (role == null) {
                return;
            }
            _context.Set<Role>().Remove(role);
        }
    }
}