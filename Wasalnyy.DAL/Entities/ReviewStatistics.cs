using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.DAL.Entities
{
    //helper model i use to pass data between layers 
    public class ReviewStatistics
    {
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<int, int> RatingsDistribution { get; set; } // 5 stars -> count
    }
}
