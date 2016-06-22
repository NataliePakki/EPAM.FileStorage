using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORM {
    [Table("Users")]
    public class User {
        public User() {
            Roles = new HashSet<Role>();
            FileStorage = new HashSet<File>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100), MinLength(8)]
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public Byte[] Photo { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<File> FileStorage { get; set; } 
    }
}
