namespace ORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class same : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "IsShared", c => c.Boolean(nullable: false));
            DropColumn("dbo.Files", "IsPublic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "IsPublic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Files", "IsShared");
        }
    }
}
