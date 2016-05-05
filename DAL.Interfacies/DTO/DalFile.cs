using System;

namespace DAL.Interfacies.DTO {
    public class DalFile : IEntity {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime TimeAdded { get; set; }
        public bool IsPublic { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}