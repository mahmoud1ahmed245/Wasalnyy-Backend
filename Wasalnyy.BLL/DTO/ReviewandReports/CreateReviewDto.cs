using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.BLL.DTO.ReviewandReports
{
    public class CreateReviewDto
    {
        public Guid TripId { get; set; }

       
        public string? Comment { get; set; }

        // 1–5 stars don't forget
        public int Stars { get; set; }
        // i don't send driver id or rider id we take it from the trip 


       
    }
}
