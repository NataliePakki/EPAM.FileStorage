
namespace ORM {
    using System.Data.Entity;

    public class EntityModel : DbContext {
        public EntityModel()
            : base("name=EntityModel") {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<File> Files { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                 .HasMany<Role>(u => u.Roles)
                 .WithMany(r => r.Users)
                 .Map(m => {
                     m.ToTable("UserRole");
                     m.MapLeftKey("UserId");
                     m.MapRightKey("RoleId");
                 });

        }
    }
}
