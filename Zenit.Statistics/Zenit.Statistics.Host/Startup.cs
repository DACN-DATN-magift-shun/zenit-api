using Microsoft.AspNetCore.Builder;

namespace Zenit.Statistics.Host
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddMediatR((configs) =>
            {
                configs.RegisterServicesFromAssemblyContaining<Startup>();
            });

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            app.UseRouting();
            app.UseGlobalExceptionHandler();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
