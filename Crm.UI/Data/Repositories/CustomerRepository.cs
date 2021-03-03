using Crm.DataAccess;
using Crm.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Crm.UI.Data.Repositories
{

    public class CustomerRepository : Repository<Customer, CrmDbContext>, ICustomerRepository
    {
        public CustomerRepository(CrmDbContext context) : base(context)
        {
        }

        public override async Task<Customer> GetByIdAsync(int customerId)
        {
            return await Context.Customers
                .Include(x => x.CustomerContacts)
                .SingleAsync(f => f.Id == customerId);
        }

        public void RemovePhoneNumber(CustomerContact model)
        {
            Context.CustomerContacts.Remove(model);
        }
    }
}
