namespace Crm.DataAccess.Migrations
{
    using Crm.Model;
    using System.Collections.ObjectModel;
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
              new Customer
              {
                  Name = "Andrew Peters",
                  Code = "800236",
                  Email = "Andrew.Peters@gmail.com",
                  CustomerType = new CustomerType
                  {
                      Name = "Student"
                  },
                  CustomerContacts = new Collection<CustomerContact>()
                  {
                      new CustomerContact { Number = "+46720185426"},
                      new CustomerContact { Number = "+46720185427"},
                      new CustomerContact { Number = "+46720185428"},
                  }
              },
              new Customer
              {
                  Name = "Brice Lambson",
                  Code = "800325",
                  Email = "Brice.Lambson@gmail.com",
                  CustomerType = new CustomerType
                  {
                      Name = "Doctor"
                  },
                  CustomerContacts = new Collection<CustomerContact>()
                  {
                      new CustomerContact { Number = "+46720185326"},
                      new CustomerContact { Number = "+46720185327"},
                      new CustomerContact { Number = "+46720185328"},
                  }
              },
              new Customer
              {
                  Name = "Rowan Miller",
                  Code = "800369",
                  Email = "Rowan.Miller@gamil.com",
                  CustomerType = new CustomerType
                  {
                      Name = "Mechanic"
                  },
                  CustomerContacts = new Collection<CustomerContact>()
                  {
                      new CustomerContact { Number = "+46720187426"},
                      new CustomerContact { Number = "+46720187427"},
                      new CustomerContact { Number = "+46720187428"},
                  }
              },
              new Customer
              {
                  Name = "Rick Morgan",
                  Code = "800145",
                  Email = "Rick.Morgan@gmail.com",
                  CustomerType = new CustomerType
                  {
                      Name = "Worker"
                  },
                  CustomerContacts = new Collection<CustomerContact>()
                  {
                      new CustomerContact { Number = "+46720135426"},
                      new CustomerContact { Number = "+46720135427"},
                      new CustomerContact { Number = "+46720135428"},
                  }
              }
            );
        }
    }
}
