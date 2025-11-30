using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasalnyy.BLL.DTO.ReviewandReports;

namespace Wasalnyy.BLL.Service.Abstraction
{
    public interface IReviewService
    {
        
        Task<ReturnReviewDto> AddReviewAsync(CreateReviewDto dto, string currentUserId);
        Task<ReturnReviewDto> UpdateReviewAsync(UpdateReviewDto dto, string currentUserId);
        Task<bool> DeleteReviewAsync(Guid reviewId, string currentUserId);
        Task<IEnumerable<ReturnReviewDto>> GetDriverReviewsAsync(string driverId);
        Task<double> GetDriverAverageRatingAsync(string driverId);
        Task<IEnumerable<ReturnReviewDto>> GetRiderReviewsAsync(string riderId);
        Task<double> GetRiderAverageRatingAsync(string riderId);
        Task<ReturnReviewDto> GetReviewByIdAsync(Guid reviewId);
        // Get all reviews written by the current user (driver or rider).
        Task<IEnumerable<ReturnReviewDto>> GetMyReviewsAsync(string currentUserId, ReviewerType reviewerType);
        // Get comprehensive review statistics for a user (total reviews, average, distribution).
        Task<ReviewStatisticsDto> GetReviewStatisticsAsync(string userId, ReviewerType userType);
        // Get a review for viewing (with authorization checks if needed).
        Task<ReturnReviewDto> GetReviewForViewAsync(Guid reviewId, string currentUserId);
    }
}
