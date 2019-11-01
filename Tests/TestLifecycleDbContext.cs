using Com.Setarit.Ramses;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Tests.Model.After;
using Tests.Model.Before;

namespace Tests
{
    public class TestLifecycleDbContext : LifecycleDbContext
    {
        public DbSet<BeforeAddingModel> BeforeAddingModels { get; set; }
        public DbSet<AfterAddingModel> AfterAddingModels { get; set; }

        public TestLifecycleDbContext() { }

        public TestLifecycleDbContext(DbContextOptions options):base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=test.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
