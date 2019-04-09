using Bonfire.Analytics.Dto.Models;

namespace Bonfire.Analytics.Dto.Repositories
{
    public interface IContactRepository
    {
        TrackerDto GetTrackerDto();
    }
}