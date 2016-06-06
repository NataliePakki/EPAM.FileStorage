using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfacies.Entities;
using BLL.Interfacies.Services;
using MvcPL.Infrastructure.Mappers;
using MvcPL.Models;

namespace MvcPL.Controllers {
    [Authorize]
    public class FileController : Controller {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public FileController(IFileService fileService, IUserService userService) {
            this._fileService = fileService;
            this._userService = userService;
        }

        private int CurrentUserId => CurrentUser.Id;

        private UserEntity CurrentUser => _userService.GetUserEntity(User.Identity.Name);
        private bool UserIsAdministrator => User.IsInRole("Administrator");
        
        [AllowAnonymous]
        public ActionResult AllPublicFiles(string search = null, int page = 1) {
            var files = new List<FileEntity>();
            if(!String.IsNullOrEmpty(search)) {
                files = _fileService.GetFileEntitiesBySubstring(search).Where(f => f.IsShared).ToList();
            } else {
                files.AddRange(_fileService.GetPublicFileEntities().ToList());
            }
            var pageInfo = new PageInfo {
                PageNumber = page,
                PageSize = 3,
                TotalItems = files.Count
            };
            var tvm = files.ToMvcTable(pageInfo, search);
            ViewBag.IsEmpty = files.Count == 0;
            if(Request.IsAjaxRequest()) {
                return PartialView("_AllPublicFileTable", tvm);
            }
            return View("AllPublicFiles",tvm);
        }
        public ActionResult UserFiles(int userId = 0, string search = null, int page = 1) {
            if (!User.IsInRole("Administrator") || userId == 0) {
                userId = CurrentUserId;
            }
            var files = new List<FileEntity>();
            if(!String.IsNullOrEmpty(search)) {
                files = _fileService.GetFileEntitiesBySubstring(search).ToList();
                files = files.Where(file => file.UserId == userId).ToList();
            } else {
                files.AddRange( _fileService.GetAllFileEntities(userId).ToList());
            }
            var pageInfo = new PageInfo {
                PageNumber = page,
                PageSize = 3,
                TotalItems = files.Count
            };
            ViewBag.IsEmpty = files.Count == 0;
            var tvm = files.ToMvcTable(pageInfo,search,userId);
            if(Request.IsAjaxRequest()) {
                return PartialView("_UserFileTable", tvm);
            }

            return View(tvm);
        }
       
        [HttpGet]
        public ActionResult Create(int userId) {
            return View(new CreateFileViewModel() {UserId = userId, Description = String.Empty });
        }
        
        [HttpPost]
        public ActionResult Create(CreateFileViewModel model, HttpPostedFileBase fileBase) {
            if(ModelState.IsValid && fileBase != null && fileBase.ContentLength > 0) {
                byte[] fileBytes;
                using(MemoryStream ms = new MemoryStream()) {
                    fileBase.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                if(!UserIsAdministrator)
                    model.UserId = CurrentUserId;
                _fileService.CreateFile(new FileEntity() {
                    Name = fileBase.FileName,
                    Description = model.Description,
                    ContentType = fileBase.ContentType,
                    Size = fileBase.ContentLength,
                    IsShared = model.IsShared,
                    UserId = model.UserId,
                    TimeAdded = DateTime.Now,
                    FileBytes = fileBytes
                } );
                return RedirectToAction("UserFiles","File", new {userId = model.UserId});
            }
            ModelState.AddModelError("", "Choose file.");
            return View(model);
        }

        [HttpGet]
        public ActionResult ConfirmDelete(DeleteViewModel dvm) {
            var file = _fileService.GetAllFileEntities().FirstOrDefault(f => f.Id == dvm.Id);
            if (file != null && (file.UserId == CurrentUserId || UserIsAdministrator)) {
                return View(dvm);
            }
            return RedirectToAction("UserFiles");
        }

        public ActionResult DeleteFile(int id) {
            var file = _fileService.GetAllFileEntities().FirstOrDefault(f => f.Id == id);
            if(file != null) {
                _fileService.DeleteFile(file.Id);
                return RedirectToAction("UserFiles", new {userId = file.UserId});
            }
            return RedirectToAction("UserFiles");
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            var file = _fileService.GetAllFileEntities().FirstOrDefault(f => f.Id == id);
            if(file == null)
                return RedirectToAction("UserFiles","File");
            if (Request.IsAjaxRequest()) {
                _fileService.DeleteFile(file.Id);
                return RedirectToAction("UserFiles","File", new {userId = file.UserId});
            }
            return RedirectToAction("ConfirmDelete", new DeleteViewModel() { Id = file.Id, Name = file.Name });

        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null) {
                if (UserIsAdministrator || file.UserId == CurrentUserId) {
                    var fvm = new EditFileViewModel() {
                        Id = id,
                        Description = file.Description,
                        IsShared = file.IsShared
                    };
                    return View("Edit", fvm);
                }
            }
            return View("Error");
        }
        [HttpPost]
        public ActionResult Edit(EditFileViewModel model) {
            if(ModelState.IsValid) {
                var file = _fileService.GetFileEntity(model.Id);
                file.Description = model.Description;
                file.IsShared = model.IsShared;
                _fileService.UpdateFile(file);
                int userId = file.UserId;
                return RedirectToAction("UserFiles", new { userId = userId});
            }
            return View("Edit", model);
        }
        [AllowAnonymous]
        [HttpGet]
        public FileContentResult Download(int id) {
            var file = _fileService.GetAllFileEntities().FirstOrDefault(w => w.Id == id);
            if(file != null) {
                if(UserIsAdministrator || file.UserId == CurrentUserId || file.IsShared)
                    return File(_fileService.GetPhysicalFile(file.Id), file.ContentType, file.Name);
            }
            return null;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Share(int id) {
            var file = _fileService.GetAllFileEntities().FirstOrDefault(w => w.Id == id);
            if (file != null && file.IsShared) {
                var fvm = file.ToMvcFile();
                if (Request.IsAjaxRequest()) {
                    return PartialView("_ShareLinkDialog", fvm);
                } else return View("ShareLink", fvm);
            }
            return View("Error");

        }

        [AllowAnonymous]
        public FileContentResult GetShared(int id) {
            var file = _fileService.GetAllFileEntities().FirstOrDefault(w => w.Id == id);
            if(file != null && file.IsShared) {
                return File(_fileService.GetPhysicalFile(file.Id), file.ContentType, file.Name);
            }
            return null;
        }

    }

}
