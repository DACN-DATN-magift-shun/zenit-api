using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using RabbitMQ.Client;

using Zenit.Share.Business.Interfaces;
using Zenit.Share.Common.Constants;
using Zenit.Share.Common.Interfaces;
using Zenit.Share.Common.Services;
using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Host.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServicesWithAssignedInterface<TInterface>(this IServiceCollection services)
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

            var applicationServices = types
                .Where(t => t.IsAssignableTo(typeof(TInterface)) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            foreach (var applicationService in applicationServices)
            {
                services.AddScoped(applicationService);
            }

            return services;
        }

        public static IServiceCollection AddCurrentAccount(this IServiceCollection services)
        {
            services.AddServicesWithAssignedInterface<ICurrentAccount>();
            return services;
        }

        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddServicesWithAssignedInterface<IApplicationService>();
            return services;
        }

        public static IServiceCollection AddDomainService(this IServiceCollection services)
        {
            services.AddServicesWithAssignedInterface<IDomainService>();
            return services;
        }

        public static IServiceCollection AddCacheService(this IServiceCollection services)
        {
            return services.AddServicesWithAssignedInterface<ICache>();
        }

        public static IServiceCollection AddDapperQuery(this IServiceCollection services)
        {
            return services.AddServicesWithAssignedInterface<IDapperQuery>();
        }

        public static IServiceCollection AddEmailService(this IServiceCollection services)
        {
            return services.AddServicesWithAssignedInterface<IEmailService>();
        }

        public static IServiceCollection AddAuthenticationService(this IServiceCollection services)
        {
            var jwtSecret = Environment.GetEnvironmentVariable(EnvConstants.JWT_SECRET) ??
                throw new Exception("JWT secret is not set.");

            services.AddAuthentication(configs =>
            {
                configs.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configs.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                configs.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSecret!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    }
                );

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddRabbitmqService(this IServiceCollection services)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable(EnvConstants.RABBITMQ_HOST) ?? "localhost",
                UserName = Environment.GetEnvironmentVariable(EnvConstants.RABBITMQ_USERNAME) ?? "guest",
                Password = Environment.GetEnvironmentVariable(EnvConstants.RABBITMQ_PASSWORD) ?? "guest",
                Port = int.Parse(Environment.GetEnvironmentVariable(EnvConstants.RABBITMQ_PORT) ?? "5672"),

                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(60)
            };

            services.AddSingleton<IConnectionFactory>(connectionFactory);
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IConnectionFactory>();
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });

            return services;
        }

        public static IServiceCollection AddRabbitmqProducerService(this IServiceCollection services)
        {
            services.AddScoped<RabbitmqProducerService>();
            return services;
        }

        public static IServiceCollection AddRabbitmqConsumerService(this IServiceCollection services)
        {
            services.AddSingleton<RabbitmqConsumerService>();
            return services;
        }
    }
}
