using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MvcPL.Models
{
    public enum Role {
        Administrator = 1,
        Moderator,
        User
    }
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your email.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Enter valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel {
        public RegisterViewModel() {
            Roles = new HashSet<Role>();
        }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "The field can not be empty.")]
        [StringLength(200, ErrorMessage = "Name must contain no more 200 characters.")]
        [Remote("IsUserNameExist", "ModelValidation", ErrorMessage = "This name has already exist.")]
        public string Name { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "The field can not be empty.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect E-mail.")]
        [StringLength(200, ErrorMessage = "The email must contain no more 200 characters.")]
        [Remote("IsUserEmailExist", "ModelValidation", ErrorMessage = "This email has already exist.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter your password.")]
        [StringLength(100, ErrorMessage = "The password must contain at least 8 characters.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm the password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User roles")]
        public ICollection<Role> Roles { get; set; }

        public HttpPostedFileBase Photo { get; set; }

    }

    
}
