using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.BLL.DTO.ReviewandReports
{
    public class ComplaintStatisticsDto
    {
        public string UserId { get; set; }
        public int CriticalComplaintsCount { get; set; }
        public int NonCriticalComplaintsCount { get; set; }
        public bool IsBanned { get; set; }
        public bool NeedsWarning { get; set; }
        public string BanReason { get; set; }
        public string WarningMessage { get; set; }
    }
}
