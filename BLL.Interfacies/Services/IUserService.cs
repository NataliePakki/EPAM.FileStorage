using System.Collections.Generic;
using BLL.Interfacies.Entities;

namespace BLL.Interfacies.Services {
    public interface IUserService {
        UserEntity GetUserEntity(int id);
        UserEntity GetUserEntity(string email);
        IEnumerable<UserEntity> GetAllUserEntities();
        void UpdateUser(UserEntity user);
        void CreateUser(UserEntity user);
        void DeleteUser(int id);
    }
}