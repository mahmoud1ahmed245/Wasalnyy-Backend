using Microsoft.Extensions.DependencyInjection;

namespace EmployeeCrud.BLL.Common
{
    public static class ModularBussinessLogicLayer
    {
        public static IServiceCollection AddBussinessInPL(this IServiceCollection services)
        {
            
            //services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
            return services;
        }
    }
}
