
using Domain.Entities;
using Infra.Data.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        #region DBSet
        DbSet<Cliente> Client { get; set; }
        #endregion DbSet
        public AppDbContext()
        {
        }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region MAP
            modelBuilder.ApplyConfiguration(new ClienteMap());
            #endregion MAP

        }
    }

    public static class AppDbContextExtensions
    {
        public static void EnsureSeedData(this AppDbContext context)
        {
            context.Check();
        }


        private static void EnsureSeedDbContextData(this AppDbContext context)
        {
           // context.SaveChanges();
        }

        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void Check(this DbContext context)
        {
            ((AppDbContext)context).EnsureSeedDbContextData();
            if (!context.AllMigrationsApplied())
            {
                context.Database.Migrate();
            }
        }
    }
}
