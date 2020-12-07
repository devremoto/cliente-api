
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
	public class AppDbContext : DbContext
	{
		public AppDbContext()
		{

		}

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}
}
