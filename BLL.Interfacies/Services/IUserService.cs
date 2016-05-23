using System.Collections.Generic;
using BLL.Interfacies.Entities;

namespace BLL.Interfacies.Services {
    public interface IUserService {
        UserEntity GetUserEntity(int id);
        IEnumerable<UserEntity> GetAllUserEntities();
        UserEntity GetUserEntityByEmail(string email);
        void UpdateUser(UserEntity user);

        void CreateUser(UserEntity user);
        void DeleteUser(int id);
    }
}