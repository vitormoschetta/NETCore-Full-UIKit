using Domain.SubDomains.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Context.Maps
{
    public class UserAuthMap : IEntityTypeConfiguration<UserAuth>
    {
        public void Configure(EntityTypeBuilder<UserAuth> builder)
        {
            builder.ToTable("UserAuth");

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("varchar(120)");

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            builder.Property(x => x.Salt)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            builder.HasKey(x => x.Id);
        }
    }
}