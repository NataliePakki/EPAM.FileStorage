using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
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
            var user = CurrentUser.ToMvcEditUserModel();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhoto(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var photo = viewModel.Photo;
                var userId = viewModel.Id;
                if(IsImageCorrect(photo)) {
                    ModelState.AddModelError("", "Select jpg/png image.");
                    return View("Edit", viewModel);
                }
                EditPhoto(userId,photo);
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
                var userEmailExist = _userService.IsUserEmailExist(newEmail);
                if(userEmailExist) {
                    if (Request.IsAjaxRequest()) {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    ModelState.AddModelError("", "User with this email already exist.");
                    return View("Edit",viewModel);
                }
                _userService.EditEmail(id, newEmail);
                FormsAuthentication.SetAuthCookie(newEmail, true);
                if (Request.IsAjaxRequest()) {
                    return Json(true,JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Edit", "User");
            }
            return View("Edit", viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditName(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var id = viewModel.Id;
                var newName = viewModel.Name;
                var userNameExist = _userService.IsUserNameExist(viewModel.Name);
                if(userNameExist) {
                    if(Request.IsAjaxRequest()) {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    ModelState.AddModelError("", "User with this name already exist.");
                    return View("Edit", viewModel);
                }
                _userService.EditName(id, newName);
                Session["Name"] = newName;
                if(Request.IsAjaxRequest()) {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Edit", "User");
            }
            return View("Edit", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var newPassword = viewModel.Password;
                if (String.CompareOrdinal(newPassword, viewModel.ConfirmPassword) != 0) {
                    ModelState.AddModelError("", "Passwords must match.");
                    return View("Edit", viewModel);
                }
                var id = viewModel.Id;
                var password = _userService.GetPasswod(id);
                if (!Crypto.VerifyHashedPassword(password, viewModel.OldPassword)) {
                    ModelState.AddModelError("", "Incorrect old password.");
                    return View("Edit", viewModel);
                }
                newPassword = Crypto.HashPassword(newPassword);
                _userService.EditPassword(id, newPassword);
                if(Request.IsAjaxRequest()) {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Edit", "User");
                }
            return View("Edit",viewModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult UserBlocked() {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int userId) {
            var userDetailsModel = _userService.GetUserEntity(userId).ToUserDetailsModel();
            return View(userDetailsModel);
        }
        private UserEntity CurrentUser => _userService.GetUserEntity(User.Identity.Name);
        private bool IsImageCorrect(HttpPostedFileBase photo) => photo != null && !photo.IsImage();

        private void EditPhoto(int userId, HttpPostedFileBase photo) {
            var image = photo.HttpPostedFileBaseToImage();
            _userService.EditPhoto(userId, image);
            Session["Photo"] = image.ImageToByteArray();
        }

    }
}