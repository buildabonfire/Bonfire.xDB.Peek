using System.Collections.Generic;
using Bonfire.Analytics.XdbPeek.Models;

namespace Bonfire.Analytics.XdbPeek.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<PageEvent> GetHistoricGoals();
        IEnumerable<PageEvent> GetCurrentGoals();
    }
}