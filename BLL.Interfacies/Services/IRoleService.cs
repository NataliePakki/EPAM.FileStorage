using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services {
    public interface IRoleService {
        IEnumerable<RoleEntity> GetAllRoleEntities();
        RoleEntity GetRoleEntity(int id);
        IEnumerable<RoleEntity> GetAllRolesByUserId(int userId);
        void CreateRole(RoleEntity role);
    }
}
