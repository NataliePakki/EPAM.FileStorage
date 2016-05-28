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
    [Authorize(Roles = "User")]
    public class FileController : Controller {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public FileController(IFileService fileService, IUserService userService) {
            this._fileService = fileService;
            this._userService = userService;
        }

        //public ActionResult Index(TableViewModel tvm) {
        //    return View(tvm);
        //}
        // GET: File
        [AllowAnonymous]
        public ActionResult AllPublicFiles(string search = null, int page = 1) {
            var files = new List<FileEntity>();
            if(!String.IsNullOrEmpty(search)) {
                files = _fileService.FindFilesBySubstring(search).ToList();
            } else {
                files.AddRange(_fileService.GetAllPublicFileEntities().ToList());
            }
            var pageInfo = new PageInfo {
                PageNumber = page,
                PageSize = 3,
                TotalItems = files.Count
            };
            var tvm = files.ToMvcTable(pageInfo);
            ViewBag.IsEmpty = files.Count == 0;
            if(Request.IsAjaxRequest()) {
                return PartialView("_AllPublicFileTable", tvm);
            }
            return View("AllPublicFiles",tvm);
        }
        public ActionResult UserFiles(string search = null, int page = 1) {
            var userEmail = User.Identity.Name;
            var currentUser = _userService.GetUserEntityByEmail(userEmail);
            var userId = currentUser.Id;
            var files = new List<FileEntity>();
            if(!String.IsNullOrEmpty(search)) {
                files = _fileService.FindFilesBySubstring(search).ToList();
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
            var tvm = files.ToMvcTable(pageInfo);
            if(Request.IsAjaxRequest()) {
                return PartialView("_UserFileTable", tvm);
            }

            return View(tvm);
        }

        [HttpGet]
        public ActionResult Create() {
            return View(new CreateFileViewModel() { Description = String.Empty });
        }
        
        [HttpPost]
        public ActionResult Create(CreateFileViewModel model, HttpPostedFileBase fileBase) {
            if(ModelState.IsValid && fileBase != null && fileBase.ContentLength > 0) {
                byte[] fileBytes;
                using(MemoryStream ms = new MemoryStream()) {
                    fileBase.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                var userEmail = User.Identity.Name;
                var currentUser = _userService.GetUserEntityByEmail(userEmail);
                var userId = currentUser.Id;
                _fileService.CreateFile(new FileEntity() {
                    Name = fileBase.FileName,
                    Description = model.Description,
                    ContentType = fileBase.ContentType,
                    Size = fileBase.ContentLength,
                    IsPublic = false,
                    UserId = userId,
                    TimeAdded = DateTime.Now,
                    FileBytes = fileBytes
                } );
                return RedirectToAction("UserFiles","File");
            }
            ModelState.AddModelError("", "Choose file.");
            return View(model);
        }

       
        [HttpGet]
        public ActionResult ConfirmDelete(int id) {
            var userEmail = User.Identity.Name;
            var currentUser = _userService.GetUserEntityByEmail(userEmail);
            var userId = currentUser.Id;
            var file = _fileService.GetAllFileEntities(userId).FirstOrDefault(f => f.Id == id);
            if(file != null) {
                _fileService.DeleteFile(file.Id);
            }
            return RedirectToAction("UserFiles");
        }

        public ActionResult Delete(int id) {
            var userEmail = User.Identity.Name;
            var currentUser = _userService.GetUserEntityByEmail(userEmail);
            var userId = currentUser.Id;
            var file = _fileService.GetAllFileEntities(userId).FirstOrDefault(f => f.Id == id);
            if(file == null)
                return RedirectToAction("UserFiles");
            if (Request.IsAjaxRequest()) {
                _fileService.DeleteFile(file.Id);
                return RedirectToAction("UserFiles");
            }
            return View("ConfirmDelete", new DeleteViewModel() {Id = file.Id, Name = file.Name});
        }
        public FileContentResult Download(int id) {
            var userEmail = User.Identity.Name;
            var currentUser = _userService.GetUserEntityByEmail(userEmail);
            var userId = currentUser.Id;
            var file = _fileService.GetAllFileEntities(userId).FirstOrDefault(w => w.Id == id);
            if(file != null) {
                return File(_fileService.GetPhysicalFile(file.Id), file.ContentType, file.Name);
            }
            return null;
        }

        



    }

}
