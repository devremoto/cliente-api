using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Map
{
	public class ClienteMap : IEntityTypeConfiguration<Cliente>
	{
		public void Configure(EntityTypeBuilder<Cliente> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(a => a.Id).HasColumnType("char(36)").ValueGeneratedOnAdd();
			builder.Property(a => a.Nome).HasColumnType("varchar(100)").HasMaxLength(100);
			builder.Property(a => a.Idade);
			builder.ToTable("Cliente");
		}
	}
}