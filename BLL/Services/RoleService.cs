using System.Collections.Generic;
using System.Linq;
using BLL.Interfacies.Entities;
using BLL.Interfacies.Services;
using BLL.Mappers;
using DAL.Interfacies.Repository;

namespace BLL.Services {
    public class RoleService : IRoleService {
        private readonly IUnitOfWork _uow;
        private readonly IRoleRepository _roleRepository;

        public RoleService(IUnitOfWork uow, IRoleRepository roleRepository) {
            _uow = uow;
            _roleRepository = roleRepository;
        }

        public IEnumerable<RoleEntity> GetAllRoleEntities() {
            return _roleRepository.GetAll().Select(role => role.ToBllRole());
        }

        public RoleEntity GetRoleEntity(int id) {
            return _roleRepository.GetById(id).ToBllRole();
        }

        public IEnumerable<RoleEntity> GetAllRolesByUserId(int id) {
            return _roleRepository.GetRolesByUserId(id).ToRoleEntityCollection();
        }

        public void CreateRole(RoleEntity role) {
            _roleRepository.Create(role.ToDalRole());
            _uow.Commit();
        }

    }
}
