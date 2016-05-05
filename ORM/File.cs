

using System;

namespace ORM {

    public class File {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsPublic { get; set; }
        public DateTime TimeAdded { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public virtual User User { get; set; }
    }
}