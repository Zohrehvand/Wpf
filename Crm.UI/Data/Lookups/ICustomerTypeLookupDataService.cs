using Crm.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.UI.Data.Lookups
{
    public interface ICustomerTypeLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetCustomerTypeLookupAsync();
    }
}