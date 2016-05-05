using DAL.Interfacies.DTO;

namespace DAL.Interfacies.Repository
{
    public interface IUserRepository : IRepository<DalUser>{
        DalUser GetUserByEmail(string name);
    }
}