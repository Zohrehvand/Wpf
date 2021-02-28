using Crm.Model;
using System.Threading.Tasks;

namespace Crm.UI.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(int customerId);
        Task SaveAsync();
        bool HasChanges();
        void Remove(Customer model);
        void Add(Customer customer);
    }
}