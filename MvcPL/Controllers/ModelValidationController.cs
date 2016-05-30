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

        public ModelValidationController(IUserService userService) {
            _userService = userService;
        }
        public JsonResult IsPasswordsMatch(string confirmPassword, string password) {
            return Json(String.CompareOrdinal(confirmPassword, password) == 0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUserEmailExist(string email) {
            var userExist = _userService.GetAllUserEntities().Any(u => u.UserEmail == email);
            if(string.CompareOrdinal(User.Identity.Name, email) == 0) {
                userExist = false;
            }
            return Json(!userExist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsUserOldPasswordMatch(string oldPassword, int id) {
            var user = _userService.GetUserEntity(id);
            var result = user != null && Crypto.VerifyHashedPassword(user.Password, oldPassword);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}