using LaporteAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace LaporteAPI.Persistente.Data
{
    public class DataContext : DbContext
    {

        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<FuncionarioTelefone> Telefones { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tabela: Funcionario
            modelBuilder.Entity<Funcionario>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.CPF).IsUnique();

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Sobrenome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CPF)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CargoId)
                   .HasConversion<string>()
                   .IsRequired();

                //entity.Property(f => f.DataNascimento)
                //.HasConversion(v => v.ToString("yyyy-MM-dd");

                entity.Property(e => e.Senha)
                    .IsRequired();

                entity.Property(e => e.NomeGerente)
                    .IsRequired()
                    .HasMaxLength(100);

            });

            // Tabela: Telefone
            modelBuilder.Entity<FuncionarioTelefone>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Numero)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(p => p.Funcionario)
                        .WithMany(f => f.Telefones)
                        .HasForeignKey(p => p.FuncionarioID)
                        .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
