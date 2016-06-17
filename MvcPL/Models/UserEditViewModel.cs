using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Web;
using System.Web.Mvc;

namespace MvcPL.Models {
    public class UserEditViewModel {
        public int Id { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect E-mail.")]
        [StringLength(200, ErrorMessage = "The email must contain no more 200 characters.")]
        [Remote("IsUserEmailExist", "ModelValidation", ErrorMessage = "This email has already exist.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The password must contain at least 8 characters.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm the password")]
        [Remote("IsPasswordsMatch", "ModelValidation", AdditionalFields = "Password", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }

        public ICollection<Role> Roles { get; set; }

        public HttpPostedFileBase Photo { get; set; }
        [Display(Name = "Avatar")]
        public Image OldPhoto { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        [Remote("IsUserOldPasswordMatch", "ModelValidation", ErrorMessage = "Incorrect old password.")]
        public string OldPassword { get; set; }
    }
}