using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcPL.Models {
    public class EditFileViewModel {
        [HiddenInput]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Is Shared??")]
        public bool IsShared { get; set; }

    }
}