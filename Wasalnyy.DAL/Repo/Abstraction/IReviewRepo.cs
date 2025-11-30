using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wasalnyy.DAL.Repo.Implementation;

namespace Wasalnyy.DAL.Repo.Abstraction
{
    public interface IReviewRepo
    {
       
        Task AddAsync(Review review);

        Task<double> GetDriverAverageRatingAsync(string driverId);

        Task<IEnumerable<Review>> GetDriverReviewsAsync(string driverId);

        Task<IEnumerable<Review>> GetRiderReviewsAsync(string riderId);

        Task<double> GetRiderAverageRatingAsync(string riderId);

       
        Task<bool> HasReviewedTripAsync(Guid tripId, string userId, ReviewerType reviewerType);

        Task<Review> GetReviewById(Guid reviewId);

       
        Task<Review> UpdateAsync(Review review);

        
        Task DeleteAsync(Guid reviewId);

        Task<int> SaveChangesAsync();

        Task<IEnumerable<Review>> GetReviewsByReviewerAsync(string reviewerId, ReviewerType reviewerType);

        Task<ReviewStatistics> GetReviewStatisticsAsync(string userId, ReviewerType userType);



    }
}