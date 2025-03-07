using System;
using System.Collections.Generic;
using System.Text;
using FundoNotes.Data;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class FundoContext : DbContext
    {
        public FundoContext(DbContextOptions<FundoContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<NotesEntity> Notestable { get; set; }
        public DbSet<LabelEntity> Labeltable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabelEntity>()
                .HasOne(l => l.LabelUser)   
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LabelEntity>()
                .HasOne(l => l.LabelNote)
                .WithMany()
                .HasForeignKey(l => l.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder); 
        }

    }
}
