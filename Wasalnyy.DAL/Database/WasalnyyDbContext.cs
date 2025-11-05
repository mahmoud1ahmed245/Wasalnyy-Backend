

namespace Wasalnyy.DAL.Database
{
    public class WasalnyyDbContext: IdentityDbContext
    {
        public WasalnyyDbContext(DbContextOptions<WasalnyyDbContext> options) : base(options)
        {
        }


    }
}
