using Crm.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.UI.Data.Lookups
{
    public interface ICampaignLookupDataService
    {
        Task<List<LookupItem>> GetMeetingLookupAsync();
    }
}