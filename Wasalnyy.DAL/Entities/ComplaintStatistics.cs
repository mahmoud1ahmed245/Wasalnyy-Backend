using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.DAL.Entities
{
    //helper moder
    public class UserComplaintStatistics
    {
        public string UserId { get; set; }
        public int CriticalComplaintsCount { get; set; }
        public int NonCriticalComplaintsCount { get; set; }
    }
}
