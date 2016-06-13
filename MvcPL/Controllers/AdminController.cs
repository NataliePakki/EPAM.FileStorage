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
        public bool BlockUser(bool isBlocked, int userId) {
            if (!ModelState.IsValid)
                return false;
            _userService.BlockUser(userId);
            return true;
        }
        public ActionResult BlockUser(int id) {
            _userService.BlockUser(id);
            return RedirectToAction("Details", "User", new { userId = id});
        }

    }
}