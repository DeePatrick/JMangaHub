namespace JMangaHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BroughtModelAsWasPreviously : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Photo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Photo", c => c.String());
        }
    }
}
