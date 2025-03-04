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

    }
}
