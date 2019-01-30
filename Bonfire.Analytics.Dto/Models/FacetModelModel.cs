using Sitecore.XConnect.Schema;

namespace Bonfire.Analytics.Dto.Models
{
    public class FacetModelModel
    {
        public string FacetName { get; set; }
        public string FacetModelName { get; set; }
        public XdbModelVersion Version { get; set; }
    }
}