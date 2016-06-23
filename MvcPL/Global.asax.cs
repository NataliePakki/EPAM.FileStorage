using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using ORM;

namespace MvcPL {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            Database.SetInitializer<EntityModel>(new DbInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }

        private void Application_EndRequest(object sender, EventArgs e) {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;

            if((request.HttpMethod == "POST") &&
                (response.StatusCode == 404 && response.SubStatusCode == 13)) {
                Response.Clear();
                Response.RedirectToRoute("TooLargeFileError");
                
            }
        }
        private void Application_Error(object sender, EventArgs e) {
            Exception ex = Server.GetLastError();
            if ((ex as HttpException)?.GetHttpCode() == 404) {
                Response.Clear();
                Server.ClearError();
                Response.RedirectToRoute("Error404");

            }
        }
    }
}
