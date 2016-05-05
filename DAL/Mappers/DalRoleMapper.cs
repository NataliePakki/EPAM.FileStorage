using System.Collections.Generic;
using System.Linq;
using DAL.Interfacies.DTO;
using ORM;

namespace DAL.Mappers {
    public static class DalRoleMapper {
        public static ICollection<DalRole> ToDalRoleCollection(this IEnumerable<Role> roles) {
            var roleList = roles.Select(r => new DalRole() {
                Id = r.Id,
                Name = r.Name
            });
            return roleList.ToList();
        }

        public static ICollection<Role> ToRoleCollection(this IEnumerable<DalRole> roles) {
            var roleList = roles.Select(r => new Role() {
                Id = r.Id,
                Name = r.Name
            });
            return roleList.ToList();
        }

    }
}
