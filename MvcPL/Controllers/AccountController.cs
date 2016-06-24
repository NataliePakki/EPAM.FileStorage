using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interfaces.Services;
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
                    var user = _userService.GetUserEntity(viewModel.Email);
                    if (user.IsBlocked) {
                        return RedirectToAction("UserBlocked", "User");
                    }
                    FormsAuthentication.SetAuthCookie(viewModel.Email, viewModel.RememberMe);
                    Session["Photo"] = user.Photo.ImageToByteArray();
                    Session["Name"] = user.Name;
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
                if (IsImageCorrect(viewModel.Photo)) {
                    ModelState.AddModelError("", "Select jpg/png image.");
                    return View(viewModel);
                }
                var userName = viewModel.Name;
                var userEmail = viewModel.Email;

                if (IsUserEmailExist(userEmail)) {
                    ModelState.AddModelError("", "User with this email already exist.");
                    return View(viewModel);
                }
                if(IsUserNameExist(userName)) {
                    ModelState.AddModelError("", "User with this name already exist.");
                    return View(viewModel);
                }

                var membershipUser = ((CustomMembershipProvider)Membership.Provider)
                    .CreateUser(viewModel);

                if(membershipUser != null) {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                    Session["Photo"] = viewModel.Photo.HttpPostedFileBaseToByteArray();
                    Session["Name"] = viewModel.Name;
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
            Session.Abandon();
            return RedirectToAction("Index", "File");
        }


        private bool IsImageCorrect(HttpPostedFileBase photo) => photo != null && !photo.IsImage();
        private bool IsUserEmailExist(string email) => _userService.IsUserEmailExist(email);
        private bool IsUserNameExist(string name) => _userService.IsUserNameExist(name);


    }
}