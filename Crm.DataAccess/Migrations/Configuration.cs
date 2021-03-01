namespace Crm.DataAccess.Migrations
{
    using Crm.Model;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CrmDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CrmDbContext context)
        {
            context.Customers.AddOrUpdate(
              p => p.Name,
              new Customer { Name = "Andrew Peters", Code = "800236", Email = "Andrew.Peters@gmail.com", CustomerType = new CustomerType { Name = "Student" } },
              new Customer { Name = "Brice Lambson", Code = "800325", Email = "Brice.Lambson@gmail.com", CustomerType = new CustomerType { Name = "Doctor" } },
              new Customer { Name = "Rowan Miller", Code = "800369", Email = "Rowan.Miller@gamil.com", CustomerType = new CustomerType { Name = "Mechanic" } },
              new Customer { Name = "Rick Morgan", Code = "800145", Email = "Rick.Morgan@gmail.com", CustomerType = new CustomerType { Name = "Worker" } }
            );
        }
    }
}
