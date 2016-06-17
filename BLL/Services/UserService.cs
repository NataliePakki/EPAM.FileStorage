using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BLL.Interfacies.Entities;
using BLL.Interfacies.Services;
using BLL.Mappers;
using DAL.Interfacies.Repository;

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

        public IEnumerable<UserEntity> GetAllUserEntities() {
            return _userRepository.GetAll().Select(user => user?.ToBllUser());
        }

        public bool IsUserExist(string email) {
            return _userRepository.GetUserByEmail(email) != null;
        }

        public void EditPhoto(int id, Image photo) {
            var user = _userRepository.Get(id);
            if (user == null) return;
            user.Photo = photo.ImageToByteArray();
            _userRepository.Update(user);
            _uow.Commit();
        }

        public void EditPassword(int id, string password) {
            var user = _userRepository.Get(id);
            if(user == null)
                return;
            user.Password = password;
            _userRepository.Update(user);
            _uow.Commit();
        }

        public void EditEmail(int id, string newEmail) {
            var user = _userRepository.Get(id);
            if (user == null) return;
            user.Name = newEmail;
           _userRepository.Update(user);
            _uow.Commit();
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