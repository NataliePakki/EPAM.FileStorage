using System.Collections.Generic;
using DAL.Interfacies.DTO;

namespace DAL.Interfacies.Repository {
    public interface IFileRepository : IRepository<DalFile>{
        IEnumerable<DalFile> SearchBySubstring(string subsrting);
        ICollection<DalFile> GetFilesByUserName(string userName);
        ICollection<DalFile> GetFilesByUserId(int userId);
        IEnumerable<DalFile> GetPublicFiles();
    }
}