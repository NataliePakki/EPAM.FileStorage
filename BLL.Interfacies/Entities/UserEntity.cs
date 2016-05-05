using System;
using System.Collections.Generic;
using System.Drawing;

namespace BLL.Interfacies.Entities {
    public class UserEntity {
        public UserEntity() {
            Roles = new HashSet<RoleEntity>();
        }
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public Image Photo { get; set; }
        public ICollection<RoleEntity> Roles { get; set; }
        public ICollection<FileEntity> FileStorage { get; set; }
    }
}