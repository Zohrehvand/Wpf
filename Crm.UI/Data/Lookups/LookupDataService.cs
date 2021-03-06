using Crm.DataAccess;
using Crm.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Crm.UI.Data.Lookups
{
    public class LookupDataService : ICustomerLookupDataService,
        ICustomerTypeLookupDataService, ICampaignLookupDataService
    {
        private Func<CrmDbContext> _contextCreator;

        public LookupDataService(Func<CrmDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetCustomerLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Customers.AsNoTracking()
                  .Select(f =>
                  new LookupItem
                  {
                      Id = f.Id,
                      DisplayMember = f.Name + " - " + f.Code
                  })
                  .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetCustomerTypeLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.CustomerTypes.AsNoTracking()
                  .Select(f =>
                  new LookupItem
                  {
                      Id = f.Id,
                      DisplayMember = f.Name
                  })
                  .ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                var items = await ctx.Campaigns.AsNoTracking()
                    .Select(m =>
                    new LookupItem
                    {
                        Id = m.Id,
                        DisplayMember = m.Title
                    })
                    .ToListAsync();
                return items;
            }
        }
    }
}
