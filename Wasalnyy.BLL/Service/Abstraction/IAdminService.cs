using Wasalnyy.BLL.DTO.Rider;
using Wasalnyy.BLL.DTO.Update;
using Wasalnyy.DAL.Entities;
using Wasalnyy.DAL.Enum;

namespace Wasalnyy.BLL.Service.Abstraction
{
    public interface IAdminService
    {

        // Riders
        Task<ReturnRiderDto> GetriderByIdAsync(string id);


        Task<IEnumerable<ReturnRiderDto>> GetAllRidersAsync();
        Task<double> GetRidersCount();
        Task<ReturnRiderDto> GetRiderByPhoneAsync(string phone);


        Task<int> GetRiderTripCountAsync(string riderId);

        
        
        Task<IEnumerable<TripDto>> GetRiderTripsAsyncByphone(string phonenum);
        Task<IEnumerable<Complaint>> GetRiderComplainsByPhoneAsync(string phonenum);
        Task<IEnumerable<Complaint>> GetRiderAganinstComplainsByPhoneAsync(string phonenum);
        Task<Complaint> GetRiderComplainByComplainsIdAsync(Guid id);



        // Drivers
        Task<Driver> GetDriverByIdAsync(string id);
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        Task<Driver?> GetDriverByLicenseAsync(string license);
       
        Task<int> GetDriverTripCountAsync(string driverId);
        

   
       
        Task<IEnumerable<Trip>> GetDriverTripsAsyncByLicen(string license);
        Task<IEnumerable<Complaint>> GetDriverSubmitedComplainsBylicenAsync(string licen);
        Task<IEnumerable<Complaint>> GetDriverAgainstComplainsBylicenAsync(string licen);
        Task<Complaint> GetDriverComplainByComplainsIdAsync(Guid id);
        Task<double> GetDriverAvgRatingAsync(string licen);




        
        // Trips
        

        Task<TripDto> GetTripByIdAsync(Guid id);

        Task<IEnumerable<Trip>> GetTripsByStatusAsync(TripStatus status);

        Task<TripPaginationDto> GetDriverTripsAsync(string driverId, int pageNumber , int pageSize );

        Task<TripPaginationDto> GetRiderTripsAsync(string riderId, int pageNumber , int pageSize );








        // Dashboard / Reports

        Task<int> GetTotalTripsAsync();
        Task<int> GetTotalDriversAsync();
       

        
       

        Task SuspendAccountDriver(string lic);
        Task SuspendAccountRider(string id);
    }
}
