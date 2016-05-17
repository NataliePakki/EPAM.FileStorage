using System.Collections.Generic;

namespace MvcPL.Models {
    public class TableViewModel {
        public bool AllFiles { get; set; }
        public ICollection<FileViewModel> Files { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}