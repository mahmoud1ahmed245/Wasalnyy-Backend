using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasalnyy.DAL.Enum;

namespace Wasalnyy.DAL.Entities
{
    public class Complaint
    {
        public Guid Id { get; set; }

        // Who submitted the complaint
        public string SubmittedById { get; set; }
        public User SubmittedBy { get; set; }

        // Who is the complaint against
        public string AgainstUserId { get; set; }
        public User AgainstUser { get; set; }

        
        public Guid TripId { get; set; }
        public Trip Trip { get; set; }
     
        public string Description { get; set; }
        public ComplaintCategory Category { get; set; }
        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;
        public ReviewerType ComplainerType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
    }
}
