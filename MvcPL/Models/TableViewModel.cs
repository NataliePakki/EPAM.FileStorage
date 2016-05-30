using System.Collections.Generic;

namespace MvcPL.Models {
    public class TableViewModel {
        public int UserId { get; set; }
        public string SearchSubString { get; set; }
        public ICollection<FileViewModel> Files { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}