using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using DAL.Interfaces.Repository;

namespace BLL.Services {
    public class RoleService : IRoleService {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _uow;

        public RoleService(IUnitOfWork uow, IRoleRepository roleRepository) {
            _uow = uow;
            _roleRepository = roleRepository;
        }

        public IEnumerable<RoleEntity> GetAllRoleEntities() {
            return _roleRepository.GetAll().Select(role => role.ToBllRole());
        }

        public RoleEntity GetRoleEntity(int id) {
            return _roleRepository.Get(id).ToBllRole();
        }

        public IEnumerable<RoleEntity> GetAllRolesByUserId(int id) {
            return _roleRepository.GetRolesByUserId(id).Select(role => role.ToBllRole());
        }

        public void CreateRole(RoleEntity role) {
            _roleRepository.Create(role.ToDalRole());
            _uow.Commit();
        }
    }
}