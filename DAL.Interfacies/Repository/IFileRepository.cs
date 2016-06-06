using System.Collections.Generic;
using DAL.Interfacies.DTO;

namespace DAL.Interfacies.Repository {
    public interface IFileRepository : IRepository<DalFile> {
        int GetId(DalFile dalFile);
        IEnumerable<DalFile> GetFilesBySubstring(string subsrting);
        ICollection<DalFile> GetFilesByUserEmail(string userEmail);
        ICollection<DalFile> GetFilesByUserId(int userId);
        IEnumerable<DalFile> GetPublicFiles();
    }
}