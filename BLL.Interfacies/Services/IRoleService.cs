using System.Collections.Generic;
using BLL.Interfacies.Entities;

namespace BLL.Interfacies.Services {
    public interface IRoleService {
        IEnumerable<RoleEntity> GetAllRoleEntities();
        RoleEntity GetRoleEntity(int id);
        IEnumerable<RoleEntity> GetAllRolesByUserId(int userId);
        void CreateRole(RoleEntity role);
    }
}
