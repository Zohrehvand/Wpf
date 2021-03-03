using System.Threading.Tasks;

namespace Crm.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id);
        bool HasChanges { get; set; }
    }
}