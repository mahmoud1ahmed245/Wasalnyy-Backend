
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Wasalnyy.BLL.Common;
using Wasalnyy.BLL.Settings;
using Wasalnyy.DAL.Common;
using Wasalnyy.DAL.Database;
using Wasalnyy.DAL.Entities;
using Wasalnyy.PL.Hubs;
namespace Wasalnyy.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSignalR();

			builder.Services.AddDbContext<WasalnyyDbContext>(options =>
	            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
             options =>
    {
             options.LoginPath = new PathString("/Account/Login");
             options.AccessDeniedPath = new PathString("/Account/Login");
             });

            builder.Services.Configure<PricingSettings>(builder.Configuration.GetSection("PricingSettings"));
            builder.Services.AddScoped<PricingSettings>(sp =>
                sp.GetRequiredService<IOptions<PricingSettings>>().Value);


            builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<WasalnyyDbContext>()
                            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddBussinessInPL();
            builder.Services.AddBussinessInDAL();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            app.UseBussinessEventSubscriptions();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<WasalnyyHub>("/Wasalnyy");


            app.MapControllers();

            app.Run();
        }
    }
}
