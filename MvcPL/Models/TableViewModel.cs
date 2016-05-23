using System.Collections.Generic;

namespace MvcPL.Models {
    public class TableViewModel {
        public ICollection<FileViewModel> Files { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}