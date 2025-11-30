using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.BLL.DTO.ReviewandReports
{
    public class UpdateReviewDto
    { 
        public Guid Id { get; set; }
        public string? Comment { get; set; }
        public double? Stars { get; set; }
    }
}
