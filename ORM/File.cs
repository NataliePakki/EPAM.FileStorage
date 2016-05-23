

using System;

namespace ORM {

    public class File {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public bool IsPublic { get; set; }
        public DateTime TimeAdded { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public virtual User User { get; set; }
    }
}