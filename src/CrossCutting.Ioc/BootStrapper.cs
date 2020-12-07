using Application.AutoMapper;
using AutoMapper;
using Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Infra.Data.Context;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces.Services;
using Services;
using System.Linq;
using Domain.Interfaces.Repository;
using Infra.Data.Repository;
using Domain.Interfaces;
using Data.UoW;
using Microsoft.Extensions.Logging;
using Application.Validations;

namespace CrossCutting.Ioc
{
    public static class BootStrapper
    {
        private static IConfiguration _configuration;
        private static AppSettings _settings;
        private static IHostEnvironment _env;
        private static ServiceProvider _provider;


        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            services
            .AddConfiguration(configuration, env)
            .AddCors()
            .AddCrudServices()
            .AddDatabase<AppDbContext>(loggerFactory)
            .AddAutoMapper()
            .SeedData();

            return services;

        }
        private static IServiceCollection AddCrudServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseAppService<,>), typeof(BaseAppService<,>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ClienteValidation>();

            #region CrudServices           

            #region Client
            services.AddTransient<IClienteAppService, ClienteAppService>();
            services.AddTransient<IClienteService, ClientService>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            #endregion Client

            #endregion CrudServices
            return services;
        }
        private static IServiceCollection AddCors(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    if (_env.IsDevelopment())
                    {
                        builder.AllowAnyMethod().AllowAnyHeader().WithOrigins(_settings.Cors.Origins).AllowCredentials();
                    }
                    else
                    {
                        builder.WithOrigins(_settings.Cors.Origins).AllowAnyHeader().AllowAnyMethod();
                    }
                });
            });

            return services;
        }
        private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();



            configuration = _configuration;
            services.AddOptions();
            _settings = _configuration.GetSection(AppSettings.SECTION).Get<AppSettings>();
            services.AddSingleton(_settings);
            return services;
        }

        #region database
        private static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, ILoggerFactory factory = null) where TContext : DbContext
        {
            _ = _settings.DbType switch
            {
                DbType.SQL => services.AddSql<TContext>(factory),
                DbType.LOCALDB => services.AddLocalDb<TContext>(factory),
                DbType.MYSQL => services.AddMysql<TContext>(factory),
                DbType.SQLLITE => services.AddSqlite<TContext>(factory),
                DbType.INMEMORY => services.AddSqlInMemory<TContext>(factory),
                DbType.POSTGRESQL => services.AddPostgres<TContext>(factory),
                _ => services.AddSql<TContext>(factory)
            };

            return services;
        }
        private static IServiceCollection AddSql<TContext>(this IServiceCollection services, ILoggerFactory factory = null) where TContext : DbContext

        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(_settings.ConnectionStrings.Sql, opts =>
                {
                    opts.EnableRetryOnFailure();
                });
                options.EnableSensitiveDataLogging();
                if (factory != null)
                    options.UseLoggerFactory(factory);
            });
            return services;
        }

        private static IServiceCollection AddLocalDb<TContext>(this IServiceCollection services, ILoggerFactory factory = null) where TContext : DbContext

        {
            services.AddDbContext<TContext>(options =>
            {
                var connectionString = _settings.ConnectionStrings.LocalDb;
                if (connectionString.Contains("%CONTENTROOTPATH%"))
                {
                    connectionString = connectionString.Replace("%CONTENTROOTPATH%", _env.ContentRootPath);
                }
                options.UseSqlServer(connectionString, opts =>
                {
                    opts.EnableRetryOnFailure();
                });
                options.EnableSensitiveDataLogging();
                if (factory != null)
                    options.UseLoggerFactory(factory);
            });
            return services;
        }

        private static IServiceCollection AddMysql<TContext>(this IServiceCollection services, ILoggerFactory factory = null) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                var loggerFactory = new LoggerFactory();
                options.UseMySql(_settings.ConnectionStrings.MySql, opts =>
                {
                    opts.EnableRetryOnFailure();
                });
                options.EnableSensitiveDataLogging();
                if (factory != null)
                    options.UseLoggerFactory(factory);
            });
            return services;
        }

        public static IServiceCollection AddSqlInMemory<TContext>(this IServiceCollection services, ILoggerFactory factory = null) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseInMemoryDatabase(_settings.ConnectionStrings.InMemory, opts =>
                {
                })
                .EnableSensitiveDataLogging();
                if (factory != null)
                    options.UseLoggerFactory(factory);
            });
            return services;

        }

        public static IServiceCollection AddSqlInMemory<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseInMemoryDatabase(connectionString, opts =>
                {
                })
                .EnableSensitiveDataLogging();
            });
            return services;

        }

        public static IServiceCollection AddSqlite<TContext>(this IServiceCollection services, ILoggerFactory factory = null) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlite(_settings.ConnectionStrings.Sqlite, opts =>
                {
                });
                options.EnableSensitiveDataLogging();
                if (factory != null)
                    options.UseLoggerFactory(factory);
            });
            return services;

        }

        public static IServiceCollection AddPostgres<TContext>(this IServiceCollection services, ILoggerFactory factory = null) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseNpgsql(_settings.ConnectionStrings.PostgreSql, opts =>
                {
                    opts.EnableRetryOnFailure();
                });
                options.EnableSensitiveDataLogging();
                if (factory != null)
                    options.UseLoggerFactory(factory);
            });
            return services;

        }
        #endregion

        public static IServiceCollection ConfigureTestServices<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services
            .AddSqlInMemory<TContext>("memory")
            .AddCrudServices();
            return services;
        }
        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ModelToViewModel).GetTypeInfo().Assembly);
            return services;

        }

        private static IServiceCollection SeedData(this IServiceCollection services)
        {
            _provider = services.BuildServiceProvider();
            services.Where(x => x.ServiceType == typeof(AppDbContext)).FirstOrDefault();
            using (var serviceScope = _provider.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<AppDbContext>().EnsureSeedData();
            }

            return services;
        }

        public static T GetConfiguration<T>(this IConfiguration configuration) where T : class
        {
            return configuration.GetSection(typeof(T).Name).Get<T>();
        }

        public static ServiceProvider GetProvider(this IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}
