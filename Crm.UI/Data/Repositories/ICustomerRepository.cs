using Crm.Model;

namespace Crm.UI.Data.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void RemovePhoneNumber(CustomerContact model);
    }
}