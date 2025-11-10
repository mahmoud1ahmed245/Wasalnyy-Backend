using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wasalnyy.BLL.Enents;
using Wasalnyy.BLL.EventHandlers.Abstraction;
using Wasalnyy.BLL.EventHandlers.Implementation;
using Wasalnyy.BLL.Mapper;
using Wasalnyy.BLL.Service.Abstraction;
using Wasalnyy.BLL.Service.Implementation;
using Wasalnyy.BLL.Settings;

namespace Wasalnyy.BLL.Common
{
    public static class ModularBussinessLogicLayer
    {
        public static IServiceCollection AddBussinessInPL(this IServiceCollection services)
        {

            services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            // Register services
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IZoneService, ZoneService>();
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<IRiderService, RiderService>();
            services.AddScoped<IRouteService, RouteService>();



            // Register events and notifiers
            services.AddSingleton<DriverEvents>();
            services.AddSingleton<RiderEvents>();
            services.AddSingleton<TripEvents>();
            services.AddSingleton<IDriverNotifier, DriverNotifier>();
            services.AddSingleton<IRiderNotifier, RiderNotifier>();
            services.AddSingleton<ITripNotifier, TripNotifier>();


            return services;
        }
        public static IApplicationBuilder UseBussinessEventSubscriptions(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var tripEvents = scope.ServiceProvider.GetRequiredService<TripEvents>();
            var driverEvents = scope.ServiceProvider.GetRequiredService<DriverEvents>();

            var tripHandler = scope.ServiceProvider.GetRequiredService<ITripNotifier>();
            var DriverHandler = scope.ServiceProvider.GetRequiredService<IDriverNotifier>();

            tripEvents.TripRequested += tripHandler.OnTripRequested;
            tripEvents.TripAccepted += tripHandler.OnTripAccepted;
            tripEvents.TripStarted += tripHandler.OnTripStarted;
            tripEvents.TripEnded += tripHandler.OnTripEnded;
            tripEvents.TripCanceled += tripHandler.OnTripCanceled;


            //driverEvents.DriverStatusChanged += DriverHandler.OnDriverStatusChanged;
            driverEvents.DriverLocationUpdated += DriverHandler.OnDriverLocationUpdated;


            return app;
        }
    }
}
