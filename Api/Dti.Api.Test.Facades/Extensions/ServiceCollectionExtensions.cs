using System;
using Dti.Api.Test.Facades.Services;
using Dti.Api.Test.Models;
using Dti.Api.Test.Models.UI;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Exceptions;

namespace Dti.Api.Test.Facades.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string APPLICATION_KEY = "Application";
        private const string SETTINGS_SECTION = "Settings";

        /// <summary>
        /// Registers project's specific services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSingletons(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(SETTINGS_SECTION).Get<ApiSettings>();
            #if DEBUG
            settings.ConnectionString = $"Data Source={Environment.CurrentDirectory}\\db.sqlite";
            #endif

            //// Dependency injection
            services.AddSingleton(settings);
            services.AddSingleton<IDBFacade, DBFacade>();

            // SERILOG settings
            services.AddSingleton<ILogger>(new LoggerConfiguration()
                     .ReadFrom.Configuration(configuration)
                     .Enrich.WithMachineName()
                     .Enrich.WithProperty(APPLICATION_KEY, Constants.PROJECT_NAME)
                     .Enrich.WithExceptionDetails()
                     .CreateLogger());
        }
    }
}
