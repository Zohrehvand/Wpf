using System.Threading.Tasks;

namespace Crm.UI.Data.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int Id);
        Task SaveAsync();
        bool HasChanges();
        void Remove(T model);
        void Add(T model);
    }
}