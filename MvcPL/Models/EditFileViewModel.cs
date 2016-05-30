using System.Web.Mvc;

namespace MvcPL.Models {
    public class EditFileViewModel {
        [HiddenInput]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}