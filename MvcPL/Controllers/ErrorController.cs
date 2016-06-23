using System.Web.Mvc;

namespace MvcPL.Controllers { 
    public class ErrorController : Controller {
        public ActionResult Error404() {
            return View();
        }
        public ActionResult TooLargeFileError() {
            if(Request.IsAjaxRequest())
                return Json(true, JsonRequestBehavior.AllowGet);
            return View();
        }
    }
}