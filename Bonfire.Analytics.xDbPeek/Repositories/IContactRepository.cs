using System;
using Bonfire.Analytics.XdbPeek.Models;

namespace Bonfire.Analytics.XdbPeek.Repositories
{
    public interface IContactRepository
    {
        TrackerDto GetTrackerDto();
        string GetListName(Guid id);
    }
}