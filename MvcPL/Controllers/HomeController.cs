using System.Linq;
using System.Web.Mvc;
using BLL.Interfacies.Services;
using MvcPL.Infrastructure.Mappers;
using MvcPL.Models;

namespace MvcPL.Controllers {
    public class HomeController : Controller {
        private readonly IFileService _fileService;
        public HomeController(IFileService fileService) {
            this._fileService = fileService;
        }

        public ActionResult Index(int page = 1) {
            var tvm = CreateTableViewModel(page);
            if(Request.IsAjaxRequest()) {
                return PartialView("_FileTable", tvm);
            }
            return View(tvm);
        }
        public ActionResult GetFilesPage(int page = 1) {
            var tvm = CreateTableViewModel(page);
            if(Request.IsAjaxRequest()) {
                return PartialView("_FileTable", tvm);
            }
            return View("Index", tvm);
        }
        //TODO: CHANGE
        [NonAction]
        public TableViewModel CreateTableViewModel(int page) {
            int pageSize = 1;
            var publicfiles = _fileService.GetAllPublicFileEntities().ToList();

            var tvm = new TableViewModel() {
                PageInfo = new PageInfo {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = publicfiles.Count
                },
                Files = publicfiles.Skip((page - 1) * pageSize).Take(10).Select(f => f.ToMvcFile()).ToList()
            };
            ViewBag.IsEmpty = publicfiles.Count == 0;
            return tvm;
        }


    }
}