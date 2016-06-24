using System;
using System.Web.Helpers;
using System.Web.Mvc;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;

namespace MvcPL.Controllers {
    public class ModelValidationController : Controller {
        private readonly IUserService _userService;
        private UserEntity CurrentUser => _userService.GetUserEntity(User.Identity.Name);
        private string CurrentUserEmail => CurrentUser?.Email;
        private string CurrentUserName => CurrentUser?.Name;
        public ModelValidationController(IUserService userService) {
            _userService = userService;
        }
        public JsonResult IsPasswordsMatch(string confirmPassword, string password) {
            return Json(String.CompareOrdinal(confirmPassword, password) == 0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUserEmailExist(string email) {
           var userExist = _userService.IsUserEmailExist(email);
            if(string.CompareOrdinal(CurrentUserEmail, email) == 0) {
                userExist = false;
            }
            return Json(!userExist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsUserNameExist(string name) {
            var userExist = _userService.IsUserNameExist(name);
            if(string.CompareOrdinal(CurrentUserName, name) == 0) {
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