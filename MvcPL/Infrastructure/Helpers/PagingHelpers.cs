using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcPL.Models;

namespace MvcPL.Infrastructure.Helpers {
    public static class PagingHelpers {
        public static MvcHtmlString PageLinks(this AjaxHelper html, PageInfo pageInfo, string actionName, string controllerName, string updateTargetId) {
            StringBuilder result = new StringBuilder();
            for(int i = 1; i <= pageInfo.TotalPages; i++) {
                string cl = i == pageInfo.PageNumber ? "btn btn-primary selected" : "btn btn-default";
                string link = html.ActionLink(i.ToString(),actionName, controllerName,new {page = i}, new AjaxOptions {UpdateTargetId = updateTargetId}, new {@class = cl}).ToString();
                result.Append(link);
            }
           return MvcHtmlString.Create(result.ToString());
        }
    }
}