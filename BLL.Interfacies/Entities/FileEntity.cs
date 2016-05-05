using System;

namespace BLL.Interfacies.Entities {
    public class FileEntity {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime TimeAdded { get; set; }
        public bool IsPublic { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}