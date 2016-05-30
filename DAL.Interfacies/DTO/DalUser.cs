using System;
using System.Collections.Generic;

namespace DAL.Interfacies.DTO {
    public class DalUser : IEntity {
        public DalUser() {
            Roles = new HashSet<DalRole>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public Byte[] Photo { get; set; }
        public ICollection<DalRole> Roles { get; set; }
        public ICollection<DalFile> FileStorage { get; set; }

    }
}