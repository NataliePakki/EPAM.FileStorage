
using System.ComponentModel.DataAnnotations;

namespace MvcPL.Models {
    public class CreateFileViewModel {
        public int UserId { get; set; }
        [Display(Name = "Is Public?")]
        public bool IsPublic { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}