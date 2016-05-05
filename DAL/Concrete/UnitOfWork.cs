using System.Data.Entity;
using DAL.Interfacies.Repository;

namespace DAL.Concrete {
    public class UnitOfWork : IUnitOfWork {
        public DbContext Context { get; private set; }

        public UnitOfWork(DbContext context) {
            Context = context;
        }

        public void Commit() {
            Context?.SaveChanges();
        }

        public void Dispose() {
            Context?.Dispose();
        }
    }

}