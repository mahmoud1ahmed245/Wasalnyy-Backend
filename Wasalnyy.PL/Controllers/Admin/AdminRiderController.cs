using Microsoft.AspNetCore.Mvc;
using Wasalnyy.BLL.Service.Abstraction;

namespace Wasalnyy.PL.Controllers.Admin
{
    [Route("api/admin/riders")]
    [ApiController]
    public class AdminRidersController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminRidersController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRiders()
        {
            var riders = await _adminService.GetAllRidersAsync();
            return Ok(riders);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Getriderbyid(string id)
        {
            var drivers = await _adminService.GetriderByIdAsync(id);
            return Ok(drivers);
        }

        [HttpGet("phone/{phone}")]
        public async Task<IActionResult> GetRiderByPhone(string phone)
        {
            var rider = await _adminService.GetRiderByPhoneAsync(phone);
            if (rider == null) return NotFound("Rider not found.");
            return Ok(rider);
        }

        [HttpGet("{riderId}/trips/count")]
        public async Task<IActionResult> GetRiderTripCount(string riderId)
        {
            var count = await _adminService.GetRiderTripCountAsync(riderId);
            return Ok(new { riderId, tripCount = count });
        }

       
       
        [HttpGet("phone/{phone}/complaints")]
        public async Task<IActionResult> GetRiderComplaintsByPhone(string phone)
        {
            var complaints = await _adminService.GetRiderComplainsByPhoneAsync(phone);
            return Ok(complaints);
        }

        [HttpGet("phone/{phone}/againstcomplaints")]
        public async Task<IActionResult> GetRiderAgainstComplaintsByPhone(string phone)
        {
            var complaints = await _adminService.GetRiderAganinstComplainsByPhoneAsync(phone);
            return Ok(complaints);
        }

        [HttpPut("{id}/suspend")]
        public async Task<IActionResult> SuspendRider(string id)
        {
            await _adminService.SuspendAccountRider(id);
            return Ok($"Rider {id} suspended");
        }

       
    }
}