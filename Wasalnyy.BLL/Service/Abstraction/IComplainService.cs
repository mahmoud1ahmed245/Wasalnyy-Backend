using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wasalnyy.BLL.DTO.ReviewandReports;
using Wasalnyy.DAL.Entities;

namespace Wasalnyy.BLL.Service.Abstraction
{
    public interface IComplaintService
    {
        /// <summary>
        /// Submit a new complaint about a user from a specific trip
        /// </summary>
        Task<Guid> AddComplaintAsync(CreateComplaintDto dto, string currentUserId);

        /// <summary>
        /// Get all complaints against a specific user
        /// </summary>
        Task<IEnumerable<ReturnComplaintDto>> GetComplaintsAgainstUserAsync(string userId);

        /// <summary>
        /// Get all complaints submitted by a specific user
        /// </summary>
        Task<IEnumerable<ReturnComplaintDto>> GetComplaintsByUserAsync(string userId);

        /// <summary>
        /// Get critical complaints against a user
        /// </summary>
        Task<IEnumerable<ReturnComplaintDto>> GetCriticalComplaintsAsync(string userId);

        /// <summary>
        /// Get non-critical complaints against a user
        /// </summary>
        Task<IEnumerable<ReturnComplaintDto>> GetNonCriticalComplaintsAsync(string userId);

        /// <summary>
        /// Get a specific complaint by ID
        /// </summary>
        Task<ReturnComplaintDto> GetComplaintByIdAsync(Guid complaintId);

        /// <summary>
        /// Get complaint statistics and ban status for a user
        /// </summary>
        Task<ComplaintStatisticsDto> GetComplaintStatisticsAsync(string userId);

        /// <summary>
        /// Check if user should be banned (business logic)
        /// </summary>
        bool ShouldBeBanned(UserComplaintStatistics stats);

        /// <summary>
        /// Check if user needs warning (business logic)
        /// </summary>
        bool NeedsWarning(UserComplaintStatistics stats);

        /// <summary>
        /// Get ban reason for user (business logic)
        /// </summary>
        string GetBanReason(UserComplaintStatistics stats);

        /// <summary>
        /// Get warning message for user (business logic)
        /// </summary>
        string GetWarningMessage(UserComplaintStatistics stats);

        /// <summary>
        /// Change complaint status (Admin only)
        /// </summary>
        Task UpdateComplaintStatusAsync(Guid complaintId, ComplaintStatus status);
    }
}