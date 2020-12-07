using CrossCutting.Ioc;
using Domain.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Interfaces;
using Api.Context;
using Api.Swagger;
using Microsoft.Extensions.Logging;

namespace Api
{

    public class Startup
    {
        public readonly IConfiguration _configuration;
        private readonly AppSettings _settings;
        private readonly IWebHostEnvironment _env;

        private static readonly ILoggerFactory DbLoggerFactory
   = LoggerFactory.Create(builder =>
   {
       builder
           .AddConsole();
   });
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _settings = _configuration.GetSection(AppSettings.SECTION).Get<AppSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor()              
              .AddServices(_configuration, _env, DbLoggerFactory)
              .AddSwagger(_settings)
              .AddScoped<IWorkContext, WorkContext>();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                                                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services
                .AddMvcCore(options => { })
                .AddApiExplorer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMiddleware<RequestMiddleware>();
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (_settings.RequireHttpsMetadata)
            {
                app.UseHttpsRedirection();
            }
            app.UseRouting();

            app.UseAppSwagger(_settings);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
