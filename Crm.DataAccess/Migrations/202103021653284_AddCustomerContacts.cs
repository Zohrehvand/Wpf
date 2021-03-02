namespace Crm.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerContacts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerContact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Number = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerContact", "CustomerId", "dbo.Customer");
            DropIndex("dbo.CustomerContact", new[] { "CustomerId" });
            DropTable("dbo.CustomerContact");
        }
    }
}
