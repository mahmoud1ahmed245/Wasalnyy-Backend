using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasalnyy.DAL.Enum;

namespace Wasalnyy.BLL.DTO.ReviewandReports
{
    public class ReturnComplaintDto
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public string SubmittedById { get; set; }
        public string AgainstUserId { get; set; }
        public string Description { get; set; }
        public ComplaintCategory Category { get; set; }
        public ComplaintStatus Status { get; set; }
        public ReviewerType ComplainerType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}
