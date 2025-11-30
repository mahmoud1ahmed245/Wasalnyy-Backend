using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.BLL.DTO.ReviewandReports
{
    public class ReturnReviewDto
    {
        public Guid Id { get; set; }
        public string RiderId { get; set; }
        public string DriverId { get; set; }
        public Guid TripId { get; set; }
        public string? Comment { get; set; }
        public double Stars { get; set; }  
        public ReviewerType ReviewerType { get; set; }  
        public DateTime CreatedAt { get; set; }
    }
}
