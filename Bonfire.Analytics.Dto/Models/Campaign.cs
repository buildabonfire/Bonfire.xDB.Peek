using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonfire.Analytics.Dto.Models
{
    public class Campaign
    {
        public DateTime? Date { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Channel { get; set; }
    }
}
