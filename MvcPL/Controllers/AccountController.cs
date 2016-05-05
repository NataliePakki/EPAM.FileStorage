using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interfacies.Services;
using MvcPL.Infrastructure.Mappers;
using MvcPL.Models;
using MvcPL.Providers;

namespace MvcPL.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private readonly IUserService _userService;


        public AccountController(IUserService userService) {
            _userService = userService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel viewModel, string returnUrl) {
            if(ModelState.IsValid) {
                if(Membership.ValidateUser(viewModel.Email, viewModel.Password)) {
                    var user = _userService.GetUserEntityByEmail(viewModel.Email);
                    FormsAuthentication.SetAuthCookie(viewModel.Email, viewModel.RememberMe);
                    Session["Photo"] = user.Photo.ImageToByteArray();
                    if(Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    } else {
                        return RedirectToAction("Index", "File");
                    }
                } else {
                    ModelState.AddModelError("", "Incorrect login or password.");
                }
            }
            return View(viewModel);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel viewModel) {
            if(ModelState.IsValid) {

                var users = _userService.GetAllUserEntities().ToList();
                var userWithSameLogin = users.Any(user => user.UserEmail.Contains(viewModel.Email));

                if(userWithSameLogin) {
                    ModelState.AddModelError("", "User with this login already exist.");
                    return View(viewModel);
                }

                var membershipUser = ((CustomMembershipProvider)Membership.Provider)
                    .CreateUser(viewModel);

                if(membershipUser != null) {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                    Session["Photo"] = viewModel.Photo.HttpPostedFileBaseToByteArray();
                    return RedirectToAction("Index", "File");
                } else {
                    ModelState.AddModelError("", "Error registration.");
                }
            }
            return View(viewModel);
        }



        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            FormsAuthentication.SignOut();
            Session["Photo"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}