namespace ORM.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ORM.EntityModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ORM.EntityModel";
        }

        protected override void Seed(ORM.EntityModel context) {
        }
    }
}
