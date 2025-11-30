using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.BLL.DTO.ReviewandReports
{
    public class UpdateComplaintStatusDto
    {
        public ComplaintStatus Status { get; set; }

        // explanation from admin about complain
        public string? ResolutionNote { get; set; }
    }
}
