using System.Data.Entity;
using System.Threading.Tasks;

namespace Crm.UI.Data.Repositories
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        protected readonly TContext Context;
        protected Repository(TContext context)
        {
            this.Context = context;
        }



        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
