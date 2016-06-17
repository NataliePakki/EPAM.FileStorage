using System.Collections.Generic;
using System.Linq;
using DAL.Interfacies.DTO;
using ORM;

namespace DAL.Mappers {
    public static class DalRoleMapper {
        public static DalRole ToDalRole(this Role role) {
            if (role == null) return null;
            return new DalRole() {
                Id = role.Id,
                Name = role.Name
            };
        }
        public static Role ToRole(this DalRole dalRole) {
            if(dalRole == null)
                return null;
            return new Role() {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }
        public static ICollection<DalRole> ToDalRoleCollection(this IEnumerable<Role> roles) {
            return roles?.Select(r => r.ToDalRole()).ToList();
        }

        public static ICollection<Role> ToRoleCollection(this IEnumerable<DalRole> roles) {
            return roles?.Select(r => r.ToRole()).ToList();
        }

    }
}
