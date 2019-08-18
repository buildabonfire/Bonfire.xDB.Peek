using System;

namespace Bonfire.Analytics.XdbPeek.Models
{
    public class PatternProfile
    {
        public string ProfileName { get; set; }
        public double Score { get; set; }
        public int Count { get; set; }
        public string PatternName { get; set; }
        public Guid? PatternId { get; set; }
        public string PatternLabel { get; set; }
    }
}