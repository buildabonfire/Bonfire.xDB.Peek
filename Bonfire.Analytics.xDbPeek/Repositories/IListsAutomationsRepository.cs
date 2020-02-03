using System.Collections.Generic;
using Bonfire.Analytics.XdbPeek.Models;

namespace Bonfire.Analytics.XdbPeek.Repositories
{
    public interface IListsAutomationsRepository
    {
        IEnumerable<ListsAutomations> GetCurrent();
    }
}
