using Microsoft.Extensions.DependencyInjection;

namespace Wasalnyy.DAL.Common
{
    public static class ModularDataAccessLayer
    {
        public static IServiceCollection AddBussinessInDAL(this IServiceCollection services)
        {
            return services;
        }
    }
}