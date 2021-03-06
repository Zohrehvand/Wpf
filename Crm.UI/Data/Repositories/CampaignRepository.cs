using Crm.DataAccess;
using Crm.Model;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Crm.UI.Data.Repositories
{
    public class CampaignRepository : Repository<Campaign, CrmDbContext>, ICampaignRepository
    {
        public CampaignRepository(CrmDbContext context) : base(context)
        {
        }

        public async override Task<Campaign> GetByIdAsync(int id)
        {
            var result = await Context.Campaigns
                 .Include(x => x.Customers)
                 .SingleAsync(x => x.Id == id);
            return result;
        }
    }
}
