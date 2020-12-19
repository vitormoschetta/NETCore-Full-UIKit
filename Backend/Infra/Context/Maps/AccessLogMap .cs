using Domains.Log.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Context.Maps
{
    public class AccessLogMap : IEntityTypeConfiguration<AccessLog>
    {
        public void Configure(EntityTypeBuilder<AccessLog> builder)
        {
            builder.ToTable("AccessLog");

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Acao)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("varchar(120)");

            builder.Property(x => x.Data)
                .IsRequired()
                .HasColumnType("datetime");                

            builder.Property(x => x.Usuario)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("varchar(120)");

            builder.Property(x => x.TabelaModificada)
                .HasMaxLength(120)                
                .HasColumnType("varchar(120)");                         

            builder.Property(x => x.Dados)     
                .HasMaxLength(120)                
                .HasColumnType("varchar(120)");             

            builder.HasKey(x => x.Id);
        }
    }
}