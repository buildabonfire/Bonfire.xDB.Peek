using Sitecore.XConnect.Schema;

namespace Bonfire.Analytics.XdbPeek.Models
{
    public class FacetModelModel
    {
        public string FacetName { get; set; }
        public string FacetModelName { get; set; }
        public XdbModelVersion Version { get; set; }
    }
}