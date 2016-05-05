using System.Linq;
using System.Web.Mvc;
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

        public ActionResult Index(int page = 1) {
            return View(page);
        }
        
        public PartialViewResult GetFilesPage(int page = 1) {
            int pageSize = 1;

            var userEmail = User.Identity.Name;
            var currentUser = _userService.GetUserEntityByEmail(userEmail);
            var userId =  currentUser.Id;

            var publicfiles = _fileService.GetAllPublicFileEntities().ToList();
            var userfiles = _fileService.GetAllFileEntities(userId).ToList();

            publicfiles.AddRange(userfiles);

            var tvm = new TableViewModel() {
                PageInfo = new PageInfo {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = publicfiles.Count
                },
                Files = publicfiles.Skip((page - 1) * pageSize).Take(10).Select(f => f.ToMvcFile()).ToList()
            };

            ViewBag.IsEmpty = publicfiles.Count == 0;
            return PartialView("_FileTable", tvm);
        }
    }

}