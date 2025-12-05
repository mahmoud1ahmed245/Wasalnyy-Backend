using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using Wasalnyy.BLL.Service.Abstraction;
using Wasalnyy.DAL.Enum;

namespace Wasalnyy.PL.Controllers.Admin
{
    [Route("api/admin/trips")]
    [ApiController]
    public class AdminTripsController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminTripsController(IAdminService adminService)
        {
            _adminService = adminService;
        }

       

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(Guid id)
        {
            var trip = await _adminService.GetTripByIdAsync(id);
            if (trip == null) return NotFound("Trip not found.");
            return Ok(trip);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTripsByStatus(TripStatus status)
        {
            var trips = await _adminService.GetTripsByStatusAsync(status);
            return Ok(trips);
        }
        [HttpGet("drivertrips/{id}")]
        public async Task<IActionResult> GetDriverTrips(string id, int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Driver ID is required");

            try
            {
                var driverTrips = await _adminService.GetDriverTripsAsync(id,pageNumber,pageSize);
                return Ok(driverTrips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
        [HttpGet("ridertrips/{id}")]
        public async Task<IActionResult> GetRiderTrips(string id, int pageNumber = 1, int pageSize = 10)
        {
            if(string.IsNullOrWhiteSpace(id))
                return BadRequest("Driver ID is required");

            try
            {
                var driverTrips = await _adminService.GetRiderTripsAsync(id, pageNumber, pageSize);
                return Ok(driverTrips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }


        [HttpGet("ridertripsbyphone/{phone}")]
        public async Task<IActionResult> GetRiderTripsByPhone(string phone)
        {
            var trips = await _adminService.GetRiderTripsAsyncByphone(phone);
            return Ok(trips);
        }




    }
}