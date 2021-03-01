namespace Crm.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Code = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        CustomerTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerType", t => t.CustomerTypeId)
                .Index(t => t.CustomerTypeId);
            
            CreateTable(
                "dbo.CustomerType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer", "CustomerTypeId", "dbo.CustomerType");
            DropIndex("dbo.Customer", new[] { "CustomerTypeId" });
            DropTable("dbo.CustomerType");
            DropTable("dbo.Customer");
        }
    }
}
