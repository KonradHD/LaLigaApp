using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LaLiga.Models;

namespace LaLiga.Data
{
    public class LaLigaContext : DbContext
    {
        public LaLigaContext(DbContextOptions<LaLigaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mecz>()
               .HasOne(m => m.gospodarze)
               .WithMany(d => d.meczeUSiebie)
               .HasForeignKey(m => m.id_gospodarzy)
               .OnDelete(DeleteBehavior.Restrict); // ważne, by nie usuwać obu przy jednej drużynie

            modelBuilder.Entity<Mecz>()
                .HasOne(m => m.goscie)
                .WithMany(d => d.meczeNaWyjezdzie)
                .HasForeignKey(m => m.id_gosci)
                .OnDelete(DeleteBehavior.Restrict); // dodanie dwóch kluczy obcych w tabeli Mecz

            modelBuilder.Entity<Statystyki>()
                .HasKey(s => s.id_meczu); // klucz główny

            modelBuilder.Entity<Statystyki>()
                .HasOne(s => s.mecz)
                .WithOne(m => m.stats)
                .HasForeignKey<Statystyki>(s => s.id_meczu)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Zawodnik>()
                .HasKey(z => new { z.id_druzyny, z.numer });

            modelBuilder.Entity<Zawodnik>()
                .HasOne(z => z.druzyna)
                .WithMany(d => d.zawodnicy)
                .HasForeignKey(z => z.id_druzyny)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Strzelec>()
                .HasKey(s => new { s.id_druzyny, s.numer, s.id_meczu });

            modelBuilder.Entity<Strzelec>()
                .HasOne(s => s.mecz)
                .WithMany(m => m.strzelcy)
                .HasForeignKey(s => s.id_meczu)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Strzelec>()
                .HasOne(s => s.zawodnik)
                .WithMany(z => z.strzelcy)
                .HasForeignKey(s => new { s.id_druzyny, s.numer })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Druzyna>()
                .Property(d => d.gole)
                .HasDefaultValue(0);

            modelBuilder.Entity<Druzyna>()
                .Property(d => d.punkty)
                .HasDefaultValue(0);
        }


        public DbSet<Strzelec> Strzelec { get; set; } = default!;
        public DbSet<Druzyna> Druzyna { get; set; } = default!;
        public DbSet<Mecz> Mecz { get; set; } = default!;
        public DbSet<Statystyki> Statystyki { get; set; } = default!;
        public DbSet<Uzytkownik> Uzytkownik { get; set; } = default!;
        public DbSet<Zawodnik> Zawodnik { get; set; } = default!;
    }
}
