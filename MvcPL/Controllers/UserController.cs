using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interfacies.Services;
using MvcPL.Infrastructure.Mappers;
using MvcPL.Models;

namespace MvcPL.Controllers
{
    [Authorize]
    public class UserController : Controller {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        [Authorize]
        public ActionResult Edit() {
            var user = _userService.GetUserEntityByEmail(User.Identity.Name).ToMvcEditUserModel();
            return View(user);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var userWithSameEmail = _userService.GetUserEntityByEmail(viewModel.Email);
                if(userWithSameEmail != null && userWithSameEmail.Id != viewModel.Id) {
                    ModelState.AddModelError("", "User with this email already exist.");
                    return View(viewModel);
                }
                var user = _userService.GetUserEntityByEmail(User.Identity.Name);
                user.UserEmail = viewModel.Email;
                if (viewModel.Photo != null) {
                    user.Photo = viewModel.Photo.HttpPostedFileBaseToImage();
                }
                _userService.UpdateUser(user);
                FormsAuthentication.SetAuthCookie(viewModel.Email, true);
                Session["Photo"] = user.Photo.ImageToByteArray();
                return RedirectToAction("Index", "File");
            }
            return View(viewModel);
        }

        public ActionResult EditPassword(UserEditViewModel viewModel) {
            if(ModelState.IsValid) {
                var user = _userService.GetUserEntityByEmail(User.Identity.Name);
                if(Crypto.VerifyHashedPassword(user.Password, viewModel.OldPassword)
                    && String.CompareOrdinal(viewModel.Password, viewModel.ConfirmPassword) == 0) {
                    user.Password = Crypto.HashPassword(viewModel.Password);
                    _userService.UpdateUser(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Edit", "User");
        }
    }
}