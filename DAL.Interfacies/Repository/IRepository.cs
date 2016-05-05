using System.Collections.Generic;
using DAL.Interfacies.DTO;

namespace DAL.Interfacies.Repository {
    public interface IRepository<TEntity> where TEntity : IEntity {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        //TEntity GetByPredicate(Expression<Func<TEntity, bool>> f);
        void Create(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }

}