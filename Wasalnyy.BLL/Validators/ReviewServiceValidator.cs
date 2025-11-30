using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasalnyy.BLL.DTO.ReviewandReports;
using Wasalnyy.BLL.DTO.Trip;


namespace Wasalnyy.BLL.Validators
{
    public class ReviewServiceValidator
    {
        public void CreateReviewValidator(CreateReviewDto dto)
        {
            if (dto == null)
                throw new ValidationException("Review data cannot be null");

            if (dto.TripId == Guid.Empty)
                throw new ValidationException("Trip ID is required");

            if (dto.Stars < 1 || dto.Stars > 5)
                throw new ValidationException("Stars must be between 1 and 5");

            if (!string.IsNullOrEmpty(dto.Comment) && dto.Comment.Length > 500)
                throw new ValidationException("Comment cannot exceed 500 characters");
        }

        public void UpdateReviewValidator(UpdateReviewDto dto)
        {
            if (dto == null)
                throw new ValidationException("Review data cannot be null");

            if (dto.Id == Guid.Empty)
                throw new ValidationException("Review ID is required");

            if (dto.Stars < 1 || dto.Stars > 5)
                throw new ValidationException("Stars must be between 1 and 5");

            if (!string.IsNullOrEmpty(dto.Comment) && dto.Comment.Length > 500)
                throw new ValidationException("Comment cannot exceed 500 characters");
        }
        public void ValidateUserInTripDto(TripDto tripDto, string currentUserId, ReviewerType reviewerType)
        {
            if (reviewerType == ReviewerType.Rider)
            {
                if (tripDto.RiderId != currentUserId)
                    throw new UnauthorizedAccessException("You can only review trips you participated in as a rider");
            }
            else if (reviewerType == ReviewerType.Driver)
            {
                if (tripDto.DriverId != currentUserId)
                    throw new UnauthorizedAccessException("You can only review trips you participated in as a driver");
            }
        }
    }
}
