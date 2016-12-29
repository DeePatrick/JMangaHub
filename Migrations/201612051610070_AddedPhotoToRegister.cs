namespace JMangaHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPhotoToRegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Photo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Photo");
        }
    }
}
