namespace Crm.DataAccess.Migrations
{
  using Crm.Model;
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;

  internal sealed class Configuration : DbMigrationsConfiguration<CrmDbContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(CrmDbContext context)
    {
      context.Customers.AddOrUpdate(
        f => f.Name,
        new Customer { Name = "Thomas", Code = "Huber" },
        new Customer { Name = "Urs", Code = "Meier" },
        new Customer { Name = "Erkan", Code = "Egin" },
        new Customer { Name = "Sara", Code = "Huber" }
        );
    }
  }
}
