using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.DAL.Enum
{
    public enum ComplaintCategory
    {
        // Critical - Immediate action required
        Harassment = 1,
        DangerousDriving = 2,
        Theft = 3,
        IncorrectCharges = 4,
        PhysicalConflict = 5,

        // Non-Critical - Lower priority
        DriverRude = 6,
        CarIssue = 7,
        LateArrival = 8,
        RiderRude = 9,
        WrongCancellation = 10,
        Other = 11
    }
}
