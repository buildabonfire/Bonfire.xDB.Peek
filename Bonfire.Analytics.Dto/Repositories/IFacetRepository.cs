using System.Collections.Generic;
using Sitecore.XConnect.Schema;

namespace Bonfire.Analytics.Dto.Repositories
{
    public interface IFacetRepository
    {
        IEnumerable<XdbFacetDefinition> GetAllContactFacetModels();
        IEnumerable<XdbFacetDefinition> GetAllInteractionFacetModels();
    }
}