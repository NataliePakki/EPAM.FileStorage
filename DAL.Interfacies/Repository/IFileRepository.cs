using System.Collections.Generic;
using DAL.Interfacies.DTO;

namespace DAL.Interfacies.Repository {
    public interface IFileRepository : IRepository<DalFile> {
        int GetId(DalFile dalFile);
        IEnumerable<DalFile> GetFilesBySubstring(string subsrting);
        //IEnumerable<DalFile> GetFilesByUserEmail(string userEmail);
        IEnumerable<DalFile> GetFilesByUserId(int userId);
        IEnumerable<DalFile> GetPublicFiles();
    }
}