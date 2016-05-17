using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Interfacies.Entities;
using BLL.Interfacies.Services;
using MvcPL.Infrastructure.Mappers;
using MvcPL.Models;

namespace MvcPL.Controllers
{
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

        public ActionResult Index(string search = null, bool allFiles = true, int page = 1) {
            var tvm = CreateTableViewModel(allFiles,page,search);
            if(Request.IsAjaxRequest()) {
                return PartialView("_FileTable", tvm);
            }
            return View(tvm);
        }

       
        //TODO: CHANGE
        [NonAction]
        public TableViewModel CreateTableViewModel(bool allFiles, int page, string searchstring = null) {
           int pageSize = 3;
            var userEmail = User.Identity.Name;
            var currentUser = _userService.GetUserEntityByEmail(userEmail);
            var userId = currentUser.Id;
            var files = new List<FileEntity>();
            if (!String.IsNullOrEmpty(searchstring)) {
                files = _fileService.FindFilesBySubstring(searchstring).ToList();
            } else {
                files.AddRange(allFiles ? _fileService.GetAllPublicFileEntities().ToList() : _fileService.GetAllFileEntities(userId).ToList());
            }

            var tvm = new TableViewModel() {
                AllFiles = allFiles,
                PageInfo = new PageInfo {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = files.Count
                },
                Files = files.Skip((page - 1) * pageSize).Take(10).Select(f => f.ToMvcFile()).ToList()
            };
            ViewBag.IsEmpty = files.Count == 0;
            return tvm;
        }
    }

}
