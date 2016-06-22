using DAL.Interfacies.DTO;
using ORM;

namespace DAL.Mappers {
    public static class DalUserMapper {
        public static DalUser ToDalUser(this User user) {
            if (user == null) return null;
            return new DalUser() {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
                Password = user.Password,
                Photo = user.Photo,
                IsBlocked = user.IsBlocked,
                Roles = user.Roles.ToDalRoleCollection(),
                FileStorage = user.FileStorage.ToDalFileCollection()
            };
        }
        public static User ToUser(this DalUser dalUser) {
            if (dalUser == null) return null;
            return new User() {
                Email = dalUser.Email,
                Name = dalUser.Name,
                Id = dalUser.Id,
                Password = dalUser.Password,
                Photo = dalUser.Photo,
                IsBlocked = dalUser.IsBlocked,
                Roles = dalUser.Roles.ToRoleCollection(),
                FileStorage = dalUser.FileStorage.ToFileCollection()
            };
        }

    }
}