using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasalnyy.DAL.Enum;

namespace Wasalnyy.BLL.DTO.ReviewandReports
{
    public class CreateComplaintDto
    {
        public Guid TripId { get; set; }
        public string Description { get; set; }
        public ComplaintCategory Category { get; set; }
        public ReviewerType ComplainerType { get; set; }
    }
}
