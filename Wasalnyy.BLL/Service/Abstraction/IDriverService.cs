using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasalnyy.BLL.DTO.Driver;
using Wasalnyy.DAL.Entities;
using Wasalnyy.DAL.Enum;

namespace Wasalnyy.BLL.Service.Abstraction
{
    public interface IDriverService
    {
        delegate void DriverAvailableDel(string driverId);
        delegate void DriverLocationUpdatedDel(string driverId, decimal lng, decimal lat);

        event DriverAvailableDel? DeriverAvailable;
        event DriverLocationUpdatedDel? DriverLocationUpdated;

        Task ChangeStatusAsync(string driverId, DriverStatus status);
        Task UpdateLocationAsync(string driverId, Coordinate coordinate);
        Task<ReturnDriver> GetById(string driverId);
        Task<IEnumerable<ReturnDriver>> GetAvailableDriversByZone(Guid zoneId);
    }
}
