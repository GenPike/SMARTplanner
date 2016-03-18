namespace SMARTplanner.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectIssueHistoryTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IssueLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateModified = c.DateTime(nullable: false),
                        ActionDescription = c.String(nullable: false),
                        ActionType = c.Int(nullable: false),
                        IssueId = c.Long(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Issues", t => t.IssueId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.IssueId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProjectLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateModified = c.DateTime(nullable: false),
                        ActionDescription = c.String(nullable: false),
                        ActionType = c.Int(nullable: false),
                        ProjectId = c.Long(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectLogs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectLogs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.IssueLogs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.IssueLogs", "IssueId", "dbo.Issues");
            DropIndex("dbo.ProjectLogs", new[] { "UserId" });
            DropIndex("dbo.ProjectLogs", new[] { "ProjectId" });
            DropIndex("dbo.IssueLogs", new[] { "UserId" });
            DropIndex("dbo.IssueLogs", new[] { "IssueId" });
            DropTable("dbo.ProjectLogs");
            DropTable("dbo.IssueLogs");
        }
    }
}
