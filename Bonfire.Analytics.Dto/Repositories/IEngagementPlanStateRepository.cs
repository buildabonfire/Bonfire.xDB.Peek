using System;
namespace Bonfire.Analytics.Dto.Repositories
{
    using System.Collections.Generic;
    using Bonfire.Analytics.Dto.Models;

    public interface IEngagementPlanStateRepository
    {
        IEnumerable<EngagementPlanState> GetCurrent();
    }
}
