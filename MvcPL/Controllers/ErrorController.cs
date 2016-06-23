using System.Web.Mvc;

namespace MvcPL.Controllers
{
    public class ErrorController : Controller {
        // GET: Error
        public ActionResult Error404() {
            return View();
        }
    }
}