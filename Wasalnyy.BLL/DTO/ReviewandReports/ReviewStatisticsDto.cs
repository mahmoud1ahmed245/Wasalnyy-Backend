using System;
using System.Collections.Generic;
using System.Linq;

namespace Wasalnyy.BLL.DTO.ReviewandReports
{
    
    // DTO for returning review statistics for a user (driver or rider)
    
    public class ReviewStatisticsDto
    {
        
        /// Total number of reviews received
       
        public int TotalReviews { get; set; }

       
        /// Average rating (0-5 stars)
       
        public double AverageRating { get; set; }

        /// <summary>
        /// Distribution of ratings: Key = Stars (1-5), Value = Count of reviews
        /// Example: {5: 80, 4: 15, 3: 3, 2: 1, 1: 1}
        /// </summary>
        public Dictionary<int, int> RatingsDistribution { get; set; } = new Dictionary<int, int>();

        
        // Percentage of 5-star reviews
        
        public double FiveStarPercentage => TotalReviews > 0
            ? Math.Round((RatingsDistribution.ContainsKey(5) ? RatingsDistribution[5] : 0) * 100.0 / TotalReviews, 2)
            : 0;

      
        public double FourStarPercentage => TotalReviews > 0
            ? Math.Round((RatingsDistribution.ContainsKey(4) ? RatingsDistribution[4] : 0) * 100.0 / TotalReviews, 2)
            : 0;

       
        public double ThreeStarPercentage => TotalReviews > 0
            ? Math.Round((RatingsDistribution.ContainsKey(3) ? RatingsDistribution[3] : 0) * 100.0 / TotalReviews, 2)
            : 0;

     
        public double TwoStarPercentage => TotalReviews > 0
            ? Math.Round((RatingsDistribution.ContainsKey(2) ? RatingsDistribution[2] : 0) * 100.0 / TotalReviews, 2)
            : 0;

        
        public double OneStarPercentage => TotalReviews > 0
            ? Math.Round((RatingsDistribution.ContainsKey(1) ? RatingsDistribution[1] : 0) * 100.0 / TotalReviews, 2)
            : 0;

        
        // Helper method to get rating category (Excellent, Good, Okay, Poor, Bad)
        
        public string GetRatingCategory()
        {
            if (AverageRating >= 4.5)
                return "Excellent";
            else if (AverageRating >= 4.0)
                return "Very Good";
            else if (AverageRating >= 3.5)
                return "Good";
            else if (AverageRating >= 3.0)
                return "Okay";
            else if (AverageRating >= 2.0)
                return "Poor";
            else
                return "Needs Improvement";
        }

       
        // Helper method to determine if rider/driver should be penalized
        
        public bool ShouldBePenalized()
        {
            // Penalize if average is below 3.0
            return AverageRating < 3.0 && TotalReviews >= 5;
        }

        
        // Helper method to determine if rider/driver needs education
        
        public bool RequiresEducation()
        {
            // Requires education if average is below 3.5 and has enough reviews
            return AverageRating < 3.5 && TotalReviews >= 5;
        }

        
        // Helper method to determine if soft suspension should apply
        
        public bool ShouldBeSuspended()
        {
            // Soft suspension if average is below 2.5
            return AverageRating < 2.5 && TotalReviews >= 10;
        }
    }
}