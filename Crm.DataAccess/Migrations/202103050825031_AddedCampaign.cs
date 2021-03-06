namespace Crm.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCampaign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerCampaign",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false),
                        Campaign_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Customer_Id, t.Campaign_Id })
                .ForeignKey("dbo.Customer", t => t.Customer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Campaign", t => t.Campaign_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.Campaign_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerCampaign", "Campaign_Id", "dbo.Campaign");
            DropForeignKey("dbo.CustomerCampaign", "Customer_Id", "dbo.Customer");
            DropIndex("dbo.CustomerCampaign", new[] { "Campaign_Id" });
            DropIndex("dbo.CustomerCampaign", new[] { "Customer_Id" });
            DropTable("dbo.CustomerCampaign");
            DropTable("dbo.Campaign");
        }
    }
}
