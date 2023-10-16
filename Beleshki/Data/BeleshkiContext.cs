using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Beleshki.Areas.Identity.Data;
using Beleshki.Models;
using System.Numerics;

namespace Beleshki.Models
{
    public class BeleshkiContext : IdentityDbContext<BeleshkiStudent>
    {
        public BeleshkiContext (DbContextOptions<BeleshkiContext> options)
            : base(options)
        {
        }

        public DbSet<Beleshki.Models.Beleshka> Beleshka { get; set; } = default!;

        public DbSet<Beleshki.Models.Predmet>? Predmet { get; set; }

        public DbSet<Beleshki.Models.Fakultet>? Fakultet { get; set; }

        public DbSet<Beleshki.Models.Komentar>? Komentar { get; set; }

        public DbSet<Beleshki.Models.StudentBeleshki>? StudentBeleshki { get; set; }

        public DbSet<Beleshki.Models.PredmetFakultet?> PredmetFakultet { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /*
        protected override void OnModelCreating(ModelBuilder builder)
            {
            
            builder.Entity<PredmetFakultet>()
                .HasOne<Predmet>(p => p.Predmet)
                .WithMany(p => p.predmetFakulteti)
                .HasForeignKey(p => p.PredmetId);

            builder.Entity<PredmetFakultet>()
            .HasOne<Fakultet>(f => f.Fakultet)
            .WithMany(f => f.predmetiFakultet)
            .HasForeignKey(f => f.FakultetId);

            builder.Entity<Beleshka>()
                .HasOne(b => b.Predmet)
                .WithMany(b => b.Beleshki)
                .HasForeignKey(b => b.PredmetId);

            builder.Entity<Komentar>()
                .HasOne(k => k.Beleshka)
                .WithMany(k => k.komentari)
                .HasForeignKey(k => k.BeleshkaId);

            builder.Entity<StudentBeleshki>()
                .HasOne(s => s.Beleshka)
                .WithMany(s => s.studentiBeleshki)
                .HasForeignKey(s => s.BeleshkaId);
        }*/
        }
    }
