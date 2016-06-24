using System.Collections.Generic;
using DAL.Interfaces.DTO;

namespace DAL.Interfaces.Repository {
    public interface IRepository<TEntity> where TEntity : IEntity {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        void Create(TEntity entity);
        void Delete(int id);
        void Update(TEntity entity);
    }

}