using DAL.Interfacies.DTO;
using ORM;

namespace DAL.Mappers {
    public static class DalUserMapper {
        public static DalUser ToDalUser(this User user) {
            if (user == null) return null;
            return new DalUser() {
                Name = user.Email,
                Id = user.Id,
                Password = user.Password,
                Photo = user.Photo,
                Roles = user.Roles.ToDalRoleCollection()
            };
        }
        public static User ToUser(this DalUser dalUser) {
            if (dalUser == null) return null;
            return new User() {
                Email = dalUser.Name,
                Id = dalUser.Id,
                Password = dalUser.Password,
                Photo = dalUser.Photo,
                Roles = dalUser.Roles.ToRoleCollection(),
            };
        }

    }
}