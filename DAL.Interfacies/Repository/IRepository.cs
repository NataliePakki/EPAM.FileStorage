using System.Collections.Generic;
using DAL.Interfacies.DTO;

namespace DAL.Interfacies.Repository {
    public interface IRepository<TEntity> where TEntity : IEntity {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        void Create(TEntity entity);
        void Delete(int id);
        void Update(TEntity entity);
    }

}