using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wasalnyy.BLL.DTO.Rider;
using Wasalnyy.DAL.Repo.Abstraction;
using Wasalnyy.DAL.Repo.Implementation;

namespace Wasalnyy.BLL.Service.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IDriverRepo _driverRepo;
        private readonly IRiderRepo _riderRepo;
        private readonly ITripRepo _tripRepo;
        private readonly IComplaintRepo _complaintRepo;
        private readonly IReviewRepo _reviewRepo;
        private readonly ITripService _tripservice;
        private readonly IMapper _mapper;
        private readonly IDriverService _driverService;

        public AdminService(IDriverRepo driverRepo, IRiderRepo riderRepo, ITripRepo tripRepo, IComplaintRepo complaintRepo, IReviewRepo reviewRepo, ITripService tripService, IMapper mapper, IDriverService driverService) { 
            _driverRepo = driverRepo;
            _riderRepo = riderRepo;
            _tripRepo = tripRepo;
            _complaintRepo = complaintRepo;
            _reviewRepo = reviewRepo;
            _tripservice = tripService;
            _mapper = mapper;
            _driverService = driverService;
        }

        #region Driver Methods

        
        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _driverRepo.GetAllDriverAsync();
        }


        public async Task<Driver> GetDriverByIdAsync(string id)
        {
            return await _driverRepo.GetByIdAsync(id);
        }


        public async Task<Driver?> GetDriverByLicenseAsync(string licen)
        {
            return await _driverRepo.GetDriverByLicense(licen);
        }

        public async Task<int> GetDriverTripCountAsync(string driverId)
        {
            return await _tripRepo.GetDriverTripsCountAsync(driverId);
        }

        
        

        public Task<IEnumerable<Trip>> GetDriverTripsAsyncByLicen(string license)
        {
            throw new NotImplementedException();
        }

     
        public async Task<IEnumerable<Complaint>> GetDriverSubmitedComplainsBylicenAsync(string licen)
        {
            return await _complaintRepo.DriverComplains(licen);
        }

        public async Task<IEnumerable<Complaint>> GetDriverAgainstComplainsBylicenAsync(string licen)
        {
            return await _complaintRepo.DriverAgainstComplains(licen);
        }

        
        public async Task<double> GetDriverAvgRatingAsync(string licen)
        {
            var driver = await _driverRepo.GetDriverByLicense(licen);
            var rating = await _reviewRepo.GetDriverAverageRatingAsync(driver.Id);
            if(rating== 0)
            {
                rating = 0;
            }
            return rating;
        }

  
        public async Task SuspendAccountDriver(string lic)
        {
            var driver = await _driverRepo.GetDriverByLicense(lic);
            if (driver != null)
            {
                driver.Suspend();
                await _driverRepo.SaveChangesAsync();
            }
        }

        #endregion

        #region Rider Methods

        public async Task<ReturnRiderDto> GetriderByIdAsync(string id)
        {
            var rider =  await _riderRepo.GetByIdAsync(id);
            return _mapper.Map<ReturnRiderDto>(rider);
        }
        public async Task<IEnumerable<ReturnRiderDto>> GetAllRidersAsync()
        {
           var riders= await _riderRepo.GetAllRidersAsync();
            return _mapper.Map<IEnumerable<ReturnRiderDto>>(riders);
        }

        
        public async Task<ReturnRiderDto> GetRiderByPhoneAsync(string phone)
        {
            var rider = await _riderRepo.GetByPhoneAsync(phone);
            return _mapper.Map<ReturnRiderDto>(rider);
        }

       
        public Task<double> GetRidersCount()
        {
            var count = _riderRepo.GetCountAsync();
            return count;
        }

        
        public async Task<int> GetRiderTripCountAsync(string riderId)
        {
            return await _tripRepo.GetRiderTripsCountAsync(riderId);
        }

      
      


      
        public async Task<IEnumerable<Complaint>> GetRiderComplainsByPhoneAsync(string phonenum)
        {
            return await _riderRepo.GetRiderComplainsByPhone(phonenum);
        }
        public async Task<IEnumerable<Complaint>> GetRiderAganinstComplainsByPhoneAsync(string phonenum)
        {
            return await _riderRepo.GetRiderAgainstComplainsByPhone(phonenum);
        }


        public async Task<Complaint> GetRiderComplainByComplainsIdAsync(Guid id)
        {
            return await _complaintRepo.GetByIdAsync(id);
        }

      
        public async Task SuspendAccountRider(string id)
        {
            var rider = await _riderRepo.GetByIdAsync(id);
            if (rider != null)
            {
                rider.Suspend();
                await _riderRepo.SaveChangesAsync();
            }
        }

        #endregion

        #region Trip Methods

     
        public async Task<TripDto> GetTripByIdAsync(Guid id)
        {
            return await _tripservice.GetByIdAsync(id);
        }

        
        public async Task<IEnumerable<Trip>> GetTripsByStatusAsync(TripStatus status)
        {
            return await _tripRepo.GetTripsByStatusAsync(status);
        }

        public async Task<TripPaginationDto> GetRiderTripsAsync(string riderId, int pageNumber = 1, int pageSize = 10)
        {
            var totalCount = await _tripRepo.GetRiderTripsCountAsync(riderId);
            var trips = await _tripservice.GetAllRiderTripsPaginatedAsync(
                riderId,
                orderBy: x => x.RequestedDate,
                descending: false,
                pageNumber: pageNumber,
                pageSize: pageSize
            );

            return trips;
        }

        public async Task<TripPaginationDto> GetDriverTripsAsync(string driverId, int pageNumber = 1, int pageSize = 10)
        {
            var totalCount = await _tripRepo.GetDriverTripsCountAsync(driverId);
            var trips = await _tripservice.GetAllDriverTripsPaginatedAsync(
                driverId,
                orderBy: x => x.RequestedDate,
                descending: false,
                pageNumber: pageNumber,
                pageSize: pageSize
            );

            return trips;
        }

        public async Task<IEnumerable<TripDto>> GetRiderTripsAsyncByphone(string phonenum)
        {
            var trips = await _riderRepo.GetRiderTripsByPhone(phonenum);
            return _mapper.Map<IEnumerable<TripDto>>(trips);
        }



        #endregion

        #region Reports/Statistics Methods

       
        public async Task<int> GetTotalDriversAsync()
        {
            return await _driverRepo.GetCountAsync();
        }

        public async Task<int> GetTotalTripsAsync()
        {
            return await _tripRepo.GetCountAsync();
        }

        
        public async Task<Complaint> GetDriverComplainByComplainsIdAsync(Guid id)
        {
            return await _complaintRepo.GetByIdAsync(id);
        }

      


       

        #endregion
    }
}