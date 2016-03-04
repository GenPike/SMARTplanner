namespace SMARTplanner.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Issues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Summary = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        ProjectId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .Index(t => t.CreatorId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProjectUserAccesses",
                c => new
                    {
                        ProjectId = c.Long(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ProjectAccess = c.Int(nullable: false),
                        CanGrantAccess = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.UserId })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CodeName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkTime = c.Double(nullable: false),
                        ReportDate = c.DateTime(nullable: false),
                        WorkDescription = c.String(),
                        GroupId = c.Long(nullable: false),
                        IssueId = c.Long(nullable: false),
                        WorkItemId = c.Long(nullable: false),
                        ReporterId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Issues", t => t.IssueId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ReporterId)
                .ForeignKey("dbo.WorkItems", t => t.WorkItemId)
                .Index(t => t.IssueId)
                .Index(t => t.WorkItemId)
                .Index(t => t.ReporterId);
            
            CreateTable(
                "dbo.WorkItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Idea = c.String(nullable: false),
                        TaskType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        EstimatedTime = c.Double(nullable: false),
                        IssueId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Issues", t => t.IssueId)
                .Index(t => t.IssueId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Reports", "WorkItemId", "dbo.WorkItems");
            DropForeignKey("dbo.WorkItems", "IssueId", "dbo.Issues");
            DropForeignKey("dbo.Reports", "ReporterId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reports", "IssueId", "dbo.Issues");
            DropForeignKey("dbo.Issues", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectUserAccesses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectUserAccesses", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Issues", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.WorkItems", new[] { "IssueId" });
            DropIndex("dbo.Reports", new[] { "ReporterId" });
            DropIndex("dbo.Reports", new[] { "WorkItemId" });
            DropIndex("dbo.Reports", new[] { "IssueId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Projects", new[] { "CreatorId" });
            DropIndex("dbo.ProjectUserAccesses", new[] { "UserId" });
            DropIndex("dbo.ProjectUserAccesses", new[] { "ProjectId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Issues", new[] { "ProjectId" });
            DropIndex("dbo.Issues", new[] { "CreatorId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.WorkItems");
            DropTable("dbo.Reports");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectUserAccesses");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Issues");
        }
    }
}
