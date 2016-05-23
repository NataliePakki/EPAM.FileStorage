namespace ORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dfd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Description", c => c.String());
            AddColumn("dbo.Files", "ContentType", c => c.String());
            AddColumn("dbo.Files", "Size", c => c.Long(nullable: false));
            DropColumn("dbo.Files", "Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "Path", c => c.String());
            DropColumn("dbo.Files", "Size");
            DropColumn("dbo.Files", "ContentType");
            DropColumn("dbo.Files", "Description");
        }
    }
}
