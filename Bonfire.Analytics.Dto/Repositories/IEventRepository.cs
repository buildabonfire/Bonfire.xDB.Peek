using System.Collections.Generic;
using Bonfire.Analytics.Dto.Models;

namespace Bonfire.Analytics.Dto.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<PageEvent> GetHistoricGoals();
        IEnumerable<PageEvent> GetCurrentGoals();
    }
}