using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using DAL.Interfaces.Repository;

namespace BLL.Services {
    public class UserService : IUserService {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork uow, IUserRepository repository) {
            _uow = uow;
            _userRepository = repository;
        }

        public UserEntity GetUserEntity(int id) {
            return _userRepository.Get(id).ToBllUser();
        }

        public UserEntity GetUserEntity(string email) {
            return _userRepository.GetUserByEmail(email).ToBllUser();
        }

        public int GetId(string email) {
            return _userRepository.GetUserByEmail(email).Id;
        }

        public string GetPasswod(int id) {
            return _userRepository.Get(id).Password;
        }

        public IEnumerable<UserEntity> GetAllUserEntities() {
            return _userRepository.GetAll().Select(user => user?.ToBllUser());
        }

        public bool IsUserEmailExist(string email) {
            return _userRepository.GetUserByEmail(email) != null;
        }
        public bool IsUserNameExist(string name) {
            return _userRepository.GetUserByName(name) != null;
        }

        public void EditPhoto(int id, Image photo) {
            var user = _userRepository.Get(id).ToBllUser();
            if (user == null) return;
            user.Photo = photo;
            UpdateUser(user);
        }

        public void DeletePhoto(int id) {
            var user = _userRepository.Get(id).ToBllUser();
            if(user == null)
                return;
            user.Photo = null;
            UpdateUser(user);
        }

        public void EditPassword(int id, string password) {
            var user = _userRepository.Get(id).ToBllUser();
            if(user == null)
                return;
            user.Password = password;
            UpdateUser(user);
        }

        public void EditEmail(int id, string newEmail) {
            var user = _userRepository.Get(id).ToBllUser();
            if (user == null) return;
            user.Email = newEmail;
            UpdateUser(user);
        }
        public void EditName(int id, string newName) {
            var user = _userRepository.Get(id).ToBllUser();
            if(user == null)
                return;
            user.Name = newName;
            UpdateUser(user);
        }

        public void CreateUser(UserEntity user) {
            _userRepository.Create(user.ToDalUser());
            _uow.Commit();
        }

        public void UpdateUser(UserEntity user) {
            _userRepository.Update(user.ToDalUser());
            _uow.Commit();
        }

        public void DeleteUser(int id) {
            _userRepository.Delete(id);
            _uow.Commit();
        }

        public void BlockUser(int id) {
            var user = GetUserEntity(id);
            user.IsBlocked = !user.IsBlocked;
            UpdateUser(user);
        }
    }
}