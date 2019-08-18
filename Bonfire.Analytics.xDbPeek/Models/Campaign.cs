using System;

namespace Bonfire.Analytics.XdbPeek.Models
{
    public class Campaign
    {
        public DateTime? Date { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Channel { get; set; }
    }
}
