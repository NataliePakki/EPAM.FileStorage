﻿using System.Linq;
using System.Web.Mvc;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using MvcPL.Infrastructure.Mappers;
using MvcPL.Models;

namespace MvcPL.Controllers {
    [Authorize]
    public class FileController : Controller {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public FileController(IFileService fileService, IUserService userService) {
            _fileService = fileService;
            _userService = userService;
        }

        [AllowAnonymous]
        public ActionResult Index(int? userId= null, string search = null, int page = 1) {
            if ((userId != null && !IsCurrentUser(userId)) && !UserIsAdministrator) {
                userId = CurrentUserId;
            }

            var files = _fileService.GetFiles(search,userId).ToList().OrderByDescending(f => f.TimeAdded).ToList();
            var tvm = files.ToTableViewModel(page, search, userId);

            if(Request.IsAjaxRequest()) {
              return PartialView("_FileTable", tvm);
            }
            return View("Index", tvm);
        }

        [HttpGet]
        public ActionResult UserFiles() {
            return RedirectToAction("Index", new {userId = CurrentUserId});
        }
        

        [HttpGet]
        public ActionResult Create(int userId) {
            if(!UserIsAdministrator && !IsCurrentUser(userId))
                userId = CurrentUserId.Value;
            ViewBag.IsDialog = Request.IsAjaxRequest();
            var cvm = new CreateFileViewModel {UserId = userId, Description = string.Empty};
            if (Request.IsAjaxRequest()) {
                return PartialView("_CreateForm", cvm);
            } else
                return View(cvm);
        }

        [HttpPost]
        public ActionResult Create(CreateFileViewModel createFileViewModel) {
            if (ModelState.IsValid) {
                var fileBase = createFileViewModel.File;
                if (fileBase != null) {
                    var file = createFileViewModel.ToFileEntity();
                    _fileService.CreateFile(file);
                }
                return RedirectToAction("Index", "File", new {userId = createFileViewModel.UserId});
            }
            ViewBag.IsDialog = Request.IsAjaxRequest();
            return View(createFileViewModel);
        }

    
        [HttpPost]
        public ActionResult ConfirmDelete(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null) {
                _fileService.DeleteFile(file.Id);
                return RedirectToAction("Index", new { userId = file.UserId });
            }
            return View("FileNotFoundError");
        }
        public ActionResult Delete(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null && (IsCurrentUser(file.UserId) || UserIsAdministrator)) {
                if (Request.IsAjaxRequest()) {
                    _fileService.DeleteFile(file.Id);
                    return RedirectToAction("Index", "File", new {userId = file.UserId});
                }
                return View("ConfirmDelete", file.ToDeleteFileViewModel());
            }
            return View("FileNotFoundError");
        }
        
        [HttpGet]
        public ActionResult Edit(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null && (UserIsAdministrator || IsCurrentUser(file.UserId))) {
                var fvm = file.ToEditFileViewModel();
                ViewBag.IsDialog = Request.IsAjaxRequest();
                if(Request.IsAjaxRequest()) 
                    return PartialView("_EditForm", fvm);
                 return View("Edit", fvm);
                }
            return View("FileNotFoundError");
        }

        [HttpPost]
        public ActionResult Edit(EditFileViewModel model) {
            if (ModelState.IsValid) {
                var file = model.ToFileEntity();
                _fileService.UpdateFile(file);
                return RedirectToAction("Index", new {userId = model.UserId });
            }
            return View("Edit", model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Download(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null) {
                if (UserIsAdministrator || IsCurrentUser(file.UserId) || file.IsShared) {
                    var physicalFile = _fileService.GetPhysicalFile(file.Id);
                    if (physicalFile != null)
                        return File(physicalFile, file.ContentType, file.Name);
                }
            }
            return View("FileNotFoundError");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Share(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null && file.IsShared) {
                var fvm = file.ToMvcFile();
                if (Request.IsAjaxRequest()) {
                    return PartialView("_ShareLinkDialog", fvm);
                }
                return View("ShareLink", fvm);
            }
            return View("FileNotFoundError");
        }

        [AllowAnonymous]
        public ActionResult GetShared(int id) {
            return Download(id);
        }
        private int? CurrentUserId => CurrentUser?.Id;
        private UserEntity CurrentUser => _userService.GetUserEntity(User.Identity.Name);
        private bool UserIsAdministrator => User.IsInRole("Administrator");
        private bool IsCurrentUser(int? userId) => userId != null && userId == CurrentUserId;
    }
}