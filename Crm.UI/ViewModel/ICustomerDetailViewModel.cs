using System.Threading.Tasks;

namespace Crm.UI.ViewModel
{
    public interface ICustomerDetailViewModel
    {
        Task LoadAsync(int? customerId);
        bool HasChanges { get; set; }
    }
}