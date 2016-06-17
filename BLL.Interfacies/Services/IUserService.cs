using System;
using System.Collections.Generic;
using System.Drawing;
using BLL.Interfacies.Entities;

namespace BLL.Interfacies.Services {
    public interface IUserService {
        UserEntity GetUserEntity(int id);
        UserEntity GetUserEntity(string email);
        int GetId(string email);
        IEnumerable<UserEntity> GetAllUserEntities();
        bool IsUserExist(string email);
        void EditPhoto(int id, Image photo);
        void EditPassword(int id, string password);
        void EditEmail(int id, string newEmail);
        void UpdateUser(UserEntity user);
        void CreateUser(UserEntity user);
        void DeleteUser(int id);
        void BlockUser(int id);
    }
}