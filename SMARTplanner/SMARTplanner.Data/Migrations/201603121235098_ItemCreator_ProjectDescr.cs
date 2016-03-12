namespace SMARTplanner.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemCreator_ProjectDescr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Description", c => c.String());
            AddColumn("dbo.WorkItems", "CreatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.WorkItems", "CreatorId");
            AddForeignKey("dbo.WorkItems", "CreatorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkItems", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.WorkItems", new[] { "CreatorId" });
            DropColumn("dbo.WorkItems", "CreatorId");
            DropColumn("dbo.Projects", "Description");
        }
    }
}
