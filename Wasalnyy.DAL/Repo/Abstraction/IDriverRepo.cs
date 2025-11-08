using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasalnyy.DAL.Entities;
using Wasalnyy.DAL.Enum;

namespace Wasalnyy.DAL.Repo.Abstraction
{
    public interface IDriverRepo
    {
        Task UpdateAsync(Driver driver);
        Task<Driver?> GetByIdAsync(string driverId);
        Task<IEnumerable<Driver>> GetAvailableDriversByZoneAsync(Guid zoneId);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
