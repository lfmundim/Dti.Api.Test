using System;
using System.IO;
using System.Reflection;

using Dti.Api.Test.Facades.Extensions;
using Dti.Api.Test.Middleware;
using Dti.Api.Test.Models;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Take.Api.Health.Eir.Extensions;

namespace Dti.Api.Test
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Startup
    {
        private const string SWAGGERFILE_PATH = "./swagger/v1/swagger.json";
        private const string API_VERSION = "v1";
        private const string LOCALHOST = "http://localhost:80";
        private const string HEALTH_CHECK_ENDPOINT = "/health";
        private const string CSS = "healthcheck.css";
        private const string API_CHECK_KEY = "API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
            }));
            services.AddMvc();

            services.AddSingletons(Configuration);

            AddSwagger(services);

            services.AddControllers();
            services.AddApiHealthCheck();

            AddHealthCheckUI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseCors("ApiCorsPolicy");

            // Swagger
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.RoutePrefix = string.Empty;
                   c.SwaggerEndpoint(SWAGGERFILE_PATH, Constants.PROJECT_NAME + API_VERSION);
               });

            app.UseHttpsRedirection()
               .UseAuthentication()
               .UseRouting()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();
                   MapHealthCheck(endpoints);
               })
               .UseJsonResponseHealthChecks();
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(API_VERSION, new OpenApiInfo { Title = Constants.PROJECT_NAME, Version = API_VERSION });
                var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + Constants.XML_EXTENSION;
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private void AddHealthCheckUI(IServiceCollection services)
        {
            services.AddHealthChecksUI(setupSettings: settings =>
            {
                settings.AddHealthCheckEndpoint(API_CHECK_KEY, LOCALHOST + HEALTH_CHECK_ENDPOINT);
            });
        }

        private void MapHealthCheck(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecksUI(settings =>
            {
                settings.AddCustomStylesheet(CSS);
            });

            endpoints.MapHealthChecks(HEALTH_CHECK_ENDPOINT, new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
