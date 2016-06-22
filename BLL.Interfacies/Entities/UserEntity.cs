using System.Collections.Generic;
using System.Drawing;

namespace BLL.Interfacies.Entities {
    public class UserEntity {
        public UserEntity() {
            Roles = new HashSet<RoleEntity>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Image Photo { get; set; }
        public bool IsBlocked { get; set; }
        public ICollection<RoleEntity> Roles { get; set; }
        public ICollection<FileEntity> FileStorage { get; set; }
    }
}