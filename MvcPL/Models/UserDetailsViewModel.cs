using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPL.Models {
    public class UserDetailsViewModel {
        public int Id { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        [Display(Name = "Roles")]
        public ICollection<Role> Roles { get; set; }
        public Byte[] Photo { get; set; }

    }
}