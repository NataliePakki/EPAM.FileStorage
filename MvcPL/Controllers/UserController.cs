using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interfacies.Entities;
using BLL.Interfacies.Services;
using MvcPL.Infrastructure.Mappers;
using MvcPL.Models;

namespace MvcPL.Controllers {
    [Authorize]
    public class UserController : Controller {
        private readonly IUserService _userService;
        private string CurrentUserEmail => CurrentUser.UserEmail;

        private UserEntity CurrentUser => _userService.GetUserEntity(User.Identity.Name);

        public UserController(IUserService userService) {
            _userService = userService;
        }

        public ActionResult Edit() {
            var user = CurrentUser.ToMvcEditUserModel();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhoto(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var photo = viewModel.Photo;
                if(photo != null && !photo.IsImage()) {
                    ModelState.AddModelError("", "Select jpg/png image.");
                    return View("Edit", viewModel);
                }
                _userService.EditPhoto(viewModel.Id, photo.HttpPostedFileBaseToImage());
                Session["Photo"] = photo.HttpPostedFileBaseToByteArray();
                return RedirectToAction("Edit", "User");
            }
            return View("Edit", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmail(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var id = viewModel.Id;
                var newEmail = viewModel.Email;
                var userExist = _userService.IsUserExist(viewModel.Email);
                if(string.CompareOrdinal(CurrentUserEmail, newEmail) == 0) {
                    userExist = false;
                }
                if(userExist) {
                    ModelState.AddModelError("", "User with this email already exist.");
                    return View("Edit",viewModel);
                }
                _userService.EditEmail(id, newEmail);
                FormsAuthentication.SetAuthCookie(viewModel.Email, true);
                if (Request.IsAjaxRequest()) {
                    return Json("Email edited!", JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Edit", "User");
            }
            return View("Edit", viewModel);
        }

        public ActionResult EditPassword(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var user = CurrentUser;
                if (String.CompareOrdinal(viewModel.Password, viewModel.ConfirmPassword) != 0) {
                    ModelState.AddModelError("", "Passwords must match.");
                    return View("Edit", viewModel);
                }
                if (!Crypto.VerifyHashedPassword(user.Password, viewModel.OldPassword)) {
                    ModelState.AddModelError("", "Incorrect old password.");
                    return View("Edit", viewModel);
                }
                var password = Crypto.HashPassword(viewModel.Password);
                _userService.EditPassword(user.Id, password);
                if(Request.IsAjaxRequest()) {
                    return Json("Password edited!", JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Edit", "User");
                
            }
            return RedirectToAction("Edit", "User");
        }
        [AllowAnonymous]
        public ActionResult UserBlocked() {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int userId) {
            var userDetailsModel = _userService.GetUserEntity(userId).ToUserDetailsModel();
            return View(userDetailsModel);
        }
    }
}