namespace Crm.DataAccess.Migrations
{
    using Crm.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Crm.DataAccess.CrmDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Crm.DataAccess.CrmDbContext context)
        {
            context.Customers.AddOrUpdate(
              p => p.Name,
              new Customer { Name = "Andrew Peters", Code = "800236", Email = "Andrew.Peters@gmail.com", CustomerTypeId = 1 },
              new Customer { Name = "Brice Lambson", Code = "800325", Email = "Brice.Lambson@gmail.com", CustomerTypeId = 2 },
              new Customer { Name = "Rowan Miller", Code = "800369", Email = "Rowan.Miller@gamil.com", CustomerTypeId = 3 },
              new Customer { Name = "Rick Morgan", Code = "800145", Email = "Rick.Morgan@gmail.com", CustomerTypeId = 4 }
            );

            context.CustomerTypes.AddOrUpdate(
              p => p.Name,
              new CustomerType { Name = "Student" },
              new CustomerType { Name = "Doctor" },
              new CustomerType { Name = "Mechanic" },
              new CustomerType { Name = "Worker" }
            );

        }
    }
}
