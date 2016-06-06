using System.Collections.Generic;
using DAL.Interfacies.DTO;

namespace DAL.Interfacies.Repository {
    public interface IRoleRepository : IRepository<DalRole> {
        ICollection<DalRole> GetRolesByUserId(int userId);
    }
}