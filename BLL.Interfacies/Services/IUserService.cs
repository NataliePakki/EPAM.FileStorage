using System;
using System.Collections.Generic;
using System.Drawing;
using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services {
    public interface IUserService {
        UserEntity GetUserEntity(int id);
        UserEntity GetUserEntity(string email);
        int GetId(string email);
        string GetPasswod(int id);
        IEnumerable<UserEntity> GetAllUserEntities();
        bool IsUserEmailExist(string email);
        bool IsUserNameExist(string name);
        void EditPhoto(int id, Image photo);
        void DeletePhoto(int id);
        void EditPassword(int id, string password);
        void EditEmail(int id, string newEmail);
        void EditName(int id, string newName);
        void UpdateUser(UserEntity user);
        void CreateUser(UserEntity user);
        void DeleteUser(int id);
        void BlockUser(int id);
    }
}