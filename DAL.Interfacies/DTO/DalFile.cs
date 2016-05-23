using System;

namespace DAL.Interfacies.DTO {
    public class DalFile : IEntity {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime TimeAdded { get; set; }
        public byte[] FileBytes { get; set; }
        public bool IsPublic { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}