using FinancasApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancasApp.Data.Mappings
{
    public class ContaMap : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("CONTA");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder.Property(c => c.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.Data)
                .HasColumnName("DATA")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(c => c.Valor)
                .HasColumnName("VALOR")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.Observacoes)
                .HasColumnName("OBSERVACOES")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(c => c.UsuarioId)
                .HasColumnName("USUARIOID")
                .IsRequired();

            builder.Property(c => c.CategoriaId)
                .HasColumnName("CATEGORIAID")
                .IsRequired();

            builder.Property(c => c.Tipo)
                .HasColumnName("TIPO")
                .IsRequired();

            #region Relacionamentos

            builder.HasOne(c => c.Usuario) //Conta TEM 1 Usuário
                .WithMany(u => u.Contas) //Usuário TEM Muitas Contas
                .HasForeignKey(c => c.UsuarioId) //Chave estrangeira
                .OnDelete(DeleteBehavior.Restrict); //regra de exclusão

            builder.HasOne(c => c.Categoria) //Conta TEM 1 Categoria
                .WithMany(c => c.Contas) //Categoria TEM Muitas Contas
                .HasForeignKey(c => c.CategoriaId) //Chave estrangeira
                .OnDelete(DeleteBehavior.Restrict); //regra de exclusão

            #endregion
        }
    }
}
