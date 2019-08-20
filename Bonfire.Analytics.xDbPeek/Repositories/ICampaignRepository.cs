using System.Collections.Generic;
using Bonfire.Analytics.XdbPeek.Models;

namespace Bonfire.Analytics.XdbPeek.Repositories
{
    public interface ICampaignRepository
    {
        Campaign GetCurrent();
        IEnumerable<Campaign> GetHistoric();
    }
}
