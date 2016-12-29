namespace JMangaHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolvingIssues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MangaFormViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MangaName = c.String(nullable: false),
                        Vendor = c.String(nullable: false),
                        Summary = c.String(),
                        DueDate = c.String(nullable: false),
                        DueTime = c.String(nullable: false),
                        Genre = c.Byte(nullable: false),
                        Heading = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MangaFormViewModels");
        }
    }
}