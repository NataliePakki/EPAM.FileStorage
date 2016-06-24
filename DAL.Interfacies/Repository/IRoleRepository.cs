using System.Collections.Generic;
using DAL.Interfaces.DTO;

namespace DAL.Interfaces.Repository {
    public interface IRoleRepository : IRepository<DalRole> {
        IEnumerable<DalRole> GetRolesByUserId(int userId);
    }
}