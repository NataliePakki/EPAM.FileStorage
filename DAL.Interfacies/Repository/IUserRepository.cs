using DAL.Interfaces.DTO;

namespace DAL.Interfaces.Repository {
    public interface IUserRepository : IRepository<DalUser> {
        DalUser GetUserByEmail(string email);
        DalUser GetUserByName(string name);
    }
}