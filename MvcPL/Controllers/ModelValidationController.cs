using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BLL.Interfacies.Services;

namespace MvcPL.Controllers {
    public class ModelValidationController : Controller{
        private readonly IUserService _userService;
        private string CurrentUserEmail => _userService.GetUserEntity(User.Identity.Name).UserEmail;
        public ModelValidationController(IUserService userService) {
            _userService = userService;
        }
        public JsonResult IsPasswordsMatch(string confirmPassword, string password) {
            return Json(String.CompareOrdinal(confirmPassword, password) == 0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUserEmailExist(string email) {
            var userExist = _userService.IsUserExist(email);
            if(string.CompareOrdinal(CurrentUserEmail, email) == 0) {
                userExist = false;
            }
            return Json(!userExist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsUserOldPasswordMatch(string oldPassword) {
            var user = _userService.GetUserEntity(CurrentUserEmail);
            var result = user != null && Crypto.VerifyHashedPassword(user.Password, oldPassword);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}