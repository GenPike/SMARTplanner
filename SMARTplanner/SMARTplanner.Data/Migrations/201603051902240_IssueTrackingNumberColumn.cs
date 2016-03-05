namespace SMARTplanner.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IssueTrackingNumberColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issues", "IssueTrackingNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Issues", "IssueTrackingNumber");
        }
    }
}
