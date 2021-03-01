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
              f => f.Name,
              new Customer { Name = "Aseman", Code = "800321" },
              new Customer { Name = "Sohrab", Code = "600231" },
              new Customer { Name = "Simorgh", Code = "400596" },
              new Customer { Name = "Ghoghnous", Code = "986000" }
              );
        }
    }
}
