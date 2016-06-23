
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MvcPL.Models {
    public class CreateFileViewModel {
        public int UserId { get; set; }
        [Display(Name = "Is Shared?")]
        public bool IsShared { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}