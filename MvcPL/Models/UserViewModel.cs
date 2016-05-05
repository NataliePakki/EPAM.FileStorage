using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using ORM;

namespace MvcPL.Models {
    public class UserViewModel  {
        [Display(Name = "User Email")]
        public string Email { get; set; }

        [Display(Name = "User roles")]
        public ICollection<Role> Roles { get; set; }
        public ICollection<File> Files { get; set; }
        public Image Photo { get; set; }
    }
}