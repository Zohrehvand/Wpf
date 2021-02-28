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
              new Customer { Name = "Volvo", Code = "800321" },
              new Customer { Name = "EWork", Code = "600231" },
              new Customer { Name = "2MNordic", Code = "400596" },
              new Customer { Name = "Adfenix", Code = "986000" }
              );
        }
    }
}
