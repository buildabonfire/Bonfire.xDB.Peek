using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bonfire.Analytics.Dto.Models;

namespace Bonfire.Analytics.Dto.Repositories
{
    public interface ICampaignRepository
    {
        Campaign GetCurrent();
        IEnumerable<Campaign> GetHistoric();
    }
}
