using System.Collections.Generic;
using DAL.Interfacies.DTO;

namespace DAL.Interfacies.Repository {
    public interface IRoleRepository : IRepository<DalRole> {
        IEnumerable<DalRole> GetRolesByUserId(int userId);
    }
}