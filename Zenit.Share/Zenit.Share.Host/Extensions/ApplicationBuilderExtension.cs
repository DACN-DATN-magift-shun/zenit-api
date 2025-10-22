using System.Reflection;

using Microsoft.AspNetCore.Builder;

using Zenit.Share.Common.Constants;
using Zenit.Share.Common.Interfaces;
using Zenit.Share.Host.Interfaces;
using Zenit.Share.Host.Middlewares;


namespace Zenit.Share.Host.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseCurrentAccount<T>(this IApplicationBuilder app)
            where T : ICurrentAccount
        {
            var appName = Environment.GetEnvironmentVariable(EnvConstants.APP_NAME) ??
                throw new Exception("App name is not set.");

            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetReferencedAssemblies())
                .Where(a => a.FullName?.StartsWith(appName) ?? false)
                .DistinctBy(a => a.FullName)
                .Select(Assembly.Load);

            var types = assemblies.SelectMany(t => t.GetExportedTypes());

            var applicationBuilders = types
                .Where(t => t.IsAssignableTo(typeof(ICurrentAccountMiddleware)) && !t.IsInterface && !t.IsAbstract);


            foreach (var applicationBuilder in applicationBuilders)
            {
                app.UseMiddleware(applicationBuilder);
            }

            return app;
        }
    }
}
