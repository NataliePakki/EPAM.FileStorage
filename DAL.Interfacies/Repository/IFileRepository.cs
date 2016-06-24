using System.Collections.Generic;
using DAL.Interfacies.DTO;

namespace DAL.Interfaces.Repository {
    public interface IFileRepository : IRepository<DalFile> {
        IEnumerable<DalFile> GetFilesBySubstring(string subsrting);
        IEnumerable<DalFile> GetFilesByUserId(int userId);
        IEnumerable<DalFile> GetPublicFiles();
        byte[] GetPhysicalFile(int id);
    }
}