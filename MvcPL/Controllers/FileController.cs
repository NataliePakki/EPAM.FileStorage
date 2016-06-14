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
            _fileService = fileService;
            _userService = userService;
        }

        private int? CurrentUserId => CurrentUser?.Id;

        private UserEntity CurrentUser => _userService.GetUserEntity(User.Identity.Name);
        private bool UserIsAdministrator => User.IsInRole("Administrator");


        [AllowAnonymous]
        public ActionResult AllPublicFiles(string search = null, int page = 1) {
           var files = _fileService.GetFiles(search).ToList();
           var tvm = files.ToTableViewModel(page, search);

            ViewBag.IsEmpty = files.Count == 0;
            if (Request.IsAjaxRequest()) {
                return PartialView("_AllPublicFileTable", tvm);
            }
            return View("AllPublicFiles", tvm);
        }

        public ActionResult UserFiles(int? userId = null, string search = null, int page = 1) {
            if (!User.IsInRole("Administrator") || !userId.HasValue) 
                userId = CurrentUserId;
            var files = _fileService.GetFiles(search, userId).ToList();
            var tvm = files.ToTableViewModel(page, search, userId.Value);

            ViewBag.IsEmpty = files.Count == 0;
            if (Request.IsAjaxRequest()) {
                return PartialView("_UserFileTable", tvm);
            }
            return View(tvm);
        }

        [HttpGet]
        public ActionResult Create(int userId) {
            ViewBag.IsDialog = Request.IsAjaxRequest();
            if (Request.IsAjaxRequest()) {
                return PartialView("_CreateForm", new CreateFileViewModel {UserId = userId, Description = string.Empty});
            } else
                return View(new CreateFileViewModel { UserId = userId, Description = string.Empty });
        }

        [HttpPost]
        public ActionResult Create(CreateFileViewModel createFileViewModel, HttpPostedFileBase fileBase) {
            if (ModelState.IsValid) {
                if (!UserIsAdministrator)
                    createFileViewModel.UserId = CurrentUserId.Value;
                var file = createFileViewModel.ToFileEntity(fileBase);
                _fileService.CreateFile(file);
                return RedirectToAction("UserFiles", "File", new {userId = createFileViewModel.UserId});
            }
            ViewBag.IsDialog = Request.IsAjaxRequest();
            return View(createFileViewModel);
        }

        [HttpGet]
        public ActionResult ConfirmDelete(DeleteViewModel dvm) {
            var file = _fileService.GetFileEntity(dvm.Id);
            if (file != null && (file.UserId == CurrentUserId || UserIsAdministrator)) {
                return View(dvm);
            }
            return RedirectToAction("UserFiles");
        }
        
        [HttpPost]
        public ActionResult ConfirmDelete(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null) {
                _fileService.DeleteFile(file.Id);
                return RedirectToAction("UserFiles", new {userId = file.UserId});
            }
            return RedirectToAction("UserFiles");
        }

        public ActionResult Delete(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file == null)
                return RedirectToAction("UserFiles", "File");
            if (Request.IsAjaxRequest()) {
                _fileService.DeleteFile(file.Id);
                return RedirectToAction("UserFiles", "File", new {userId = file.UserId});
            }
            return RedirectToAction("ConfirmDelete", new DeleteViewModel {Id = file.Id, Name = file.Name});
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null && (UserIsAdministrator || file.UserId == CurrentUserId)) {
                    var fvm = file.ToEditFileViewModel();
                    return View("Edit", fvm);
                }
            return View("Error");
        }

        [HttpPost]
        public ActionResult Edit(EditFileViewModel model) {
            if (ModelState.IsValid) {
                _fileService.UpdateFile(model.ToFileEntity());
                var userId = _fileService.GetFileEntity(model.Id).UserId;
                return RedirectToAction("UserFiles", new {userId});
            }
            return View("Edit", model);
        }

        [AllowAnonymous]
        [HttpGet]
        public FileContentResult Download(int id) {
            var file = _fileService.GetFileEntity(id);
            if (file != null) {
                if (UserIsAdministrator || file.UserId == CurrentUserId || file.IsShared)
                    return File(_fileService.GetPhysicalFile(file.Id), file.ContentType, file.Name);
            }
            return null;
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
            return View("Error");
        }

        [AllowAnonymous]
        public FileContentResult GetShared(int id) {
            var file = _fileService.GetAllFiles().FirstOrDefault(w => w.Id == id);
            if (file != null && file.IsShared) {
                return File(_fileService.GetPhysicalFile(file.Id), file.ContentType, file.Name);
            }
            return null;
        }
        

    }
}