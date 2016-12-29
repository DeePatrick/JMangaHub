namespace JMangaHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredPhotoProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Photo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Photo", c => c.String(nullable: false));
        }
    }
}
