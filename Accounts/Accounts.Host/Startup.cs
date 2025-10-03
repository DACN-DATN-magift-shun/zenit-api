using MongoDB.Bson;

using Accounts.Common.Models;
using Accounts.Data;
using Share.Common.Constants;
using Share.Data.Interfaces;
using Share.Host.Extensions;
using Mapster;
using Accounts.Host.Extensions;


namespace Accounts.Host
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddDbContext();

            services.AddSwagger();

            services.AddCors();
            services.AddHttpClient();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

            services.AddApplicationService();
            services.AddDomainService();

            services.AddAuthenticationService();

            services.AddCurrentAccount<ObjectId>();
            services.AddScoped(typeof(IRepository<>), typeof(AccountRepository<>));
            services.AddMapster();
        }

        public void Configure(IApplicationBuilder app)
        {
            var isProduction = Environment.GetEnvironmentVariable(EnvConstants.IS_PRODUCTION);
            if (isProduction == "false")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseGlobalExceptionHandler();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCurrentAccount<CurrentAccount, ObjectId>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}