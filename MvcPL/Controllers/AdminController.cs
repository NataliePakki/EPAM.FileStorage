using System.Linq;
using System.Web.Mvc;
using BLL.Interfacies.Services;
using MvcPL.Infrastructure.Mappers;

namespace MvcPL.Controllers {
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller {
        private readonly IUserService _userService;

        public AdminController(IUserService userService) {
            _userService = userService;
        }


        [ActionName("Users")]
        public ActionResult GetAllUsers() {
            var users = _userService.GetAllUserEntities().Select(user => user.ToUserDetailsModel());
            return View(users);
        }
        [HttpPost]
        public bool BlockUser(bool isBlocked, string userEmail) {
            if (!ModelState.IsValid)
                return false;
            var user = _userService.GetUserEntityByEmail(userEmail);
            if (user == null) {
                return false;
            }
            user.IsBlocked = isBlocked;
            _userService.UpdateUser(user);
            return true;
        }
        public ActionResult BlockUser(string userEmail) {
            var user = _userService.GetUserEntityByEmail(userEmail);
            user.IsBlocked = !user.IsBlocked;
            _userService.UpdateUser(user);
            return RedirectToAction("Details", "User", new { userId = user.Id});
        }

    }
}