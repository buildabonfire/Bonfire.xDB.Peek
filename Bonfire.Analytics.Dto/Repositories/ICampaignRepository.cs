using System.Collections.Generic;
using Bonfire.Analytics.Dto.Models;

namespace Bonfire.Analytics.Dto.Repositories
{
    public interface ICampaignRepository
    {
        Campaign GetCurrent();
        IEnumerable<Campaign> GetHistoric();
    }
}
