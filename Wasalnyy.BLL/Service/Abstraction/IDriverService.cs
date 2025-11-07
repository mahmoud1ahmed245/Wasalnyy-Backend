using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.BLL.Service.Abstraction
{
    public interface IDriverService
    {
        delegate void DriverAvailableDel(string driverId);
        delegate void DriverLocationUpdatedDel(string driverId, decimal lng, decimal lat);

        event DriverAvailableDel? DeriverAvailable;
        event DriverLocationUpdatedDel? DriverLocationUpdated;

        Task ToggleStatusAsync(string driverId);
        Task UpdateLocationAsync(string driverId, decimal lng, decimal lat);
    }
}
