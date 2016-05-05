using System.Collections.Generic;
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
            return _userRepository.GetById(id).ToBllUser();
        }

        public UserEntity GetUserEntityByEmail(string email) {
                return _userRepository.GetUserByEmail(email).ToBllUser();
        }

        public void UpdateUser(UserEntity user) {
            _userRepository.Update(user.ToDalUser());
            _uow.Commit();
        }

        public IEnumerable<UserEntity> GetAllUserEntities() {
                return _userRepository.GetAll().Select(user => user?.ToBllUser());
        }

        public void CreateUser(UserEntity user) {
            _userRepository.Create(user.ToDalUser());
            _uow.Commit();
        }

        public void DeleteUser(UserEntity user) {
            _userRepository.Delete(user.ToDalUser());
            _uow.Commit();
        }
    }
}
