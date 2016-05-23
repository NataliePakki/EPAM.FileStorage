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

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file) {
            if(file != null && file.ContentLength > 0)
                try {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
                    string filename = Path.GetFileName(file.FileName);
                    if (filename != null) {
                        file.SaveAs(Path.Combine(path, filename));
                    }
                } catch(Exception ex) {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                } else {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("UserFiles");
        }

       
        }

}
