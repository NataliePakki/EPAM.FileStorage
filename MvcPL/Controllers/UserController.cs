using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interfacies.Services;
using MvcPL.Infrastructure.Mappers;
using MvcPL.Models;

namespace MvcPL.Controllers {
    [Authorize]
    public class UserController : Controller {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        public ActionResult Edit() {
            var user = _userService.GetUserEntity(User.Identity.Name).ToMvcEditUserModel();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                if(viewModel.Photo != null && !viewModel.Photo.IsImage()) {
                    ModelState.AddModelError("", "Select jpg/png image.");
                    return View(viewModel);
                }
                var userWithSameEmail = _userService.GetUserEntity(viewModel.Email);
                if(userWithSameEmail != null && userWithSameEmail.Id != viewModel.Id) {
                    ModelState.AddModelError("", "User with this email already exist.");
                    return View(viewModel);
                }
                var user = _userService.GetUserEntity(User.Identity.Name);
                user.UserEmail = viewModel.Email;
                if (viewModel.Photo != null) {
                    user.Photo = viewModel.Photo.HttpPostedFileBaseToImage();
                }
                _userService.UpdateUser(user);
                FormsAuthentication.SetAuthCookie(viewModel.Email, true);
                Session["Photo"] = user.Photo.ImageToByteArray();
                return RedirectToAction("Edit", "User");
            }
            return View(viewModel);
        }

        public ActionResult EditPassword(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var user = _userService.GetUserEntity(User.Identity.Name);
                if(Crypto.VerifyHashedPassword(user.Password, viewModel.OldPassword)
                    && String.CompareOrdinal(viewModel.Password, viewModel.ConfirmPassword) == 0) {
                    user.Password = Crypto.HashPassword(viewModel.Password);
                    _userService.UpdateUser(user);
                    return RedirectToAction("Edit", "User");
                }
            }
            return RedirectToAction("Edit", "User");
        }
        [AllowAnonymous]
        public ActionResult UserBlocked() {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int userId) {
            var userInfo = _userService.GetUserEntity(userId).ToUserDetailsModel();
            return View(userInfo);
        }
    }
}