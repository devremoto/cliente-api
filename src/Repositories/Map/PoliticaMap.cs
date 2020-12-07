using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Map
{
	public class PoliticaMap : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(a => a.Id).ValueGeneratedOnAdd();
			builder.Property(a => a.Name);
			builder.Property(a => a.Email);
			builder.ToTable("User");

		}
	}
}
