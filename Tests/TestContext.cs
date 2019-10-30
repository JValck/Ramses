using Com.Setarit.Ramses;
using Microsoft.EntityFrameworkCore;
using System;
using Tests.Model.Before;

namespace Tests
{
    public class TestContext : LifecycleDbContext
    {
        public DbSet<BeforeAddingModel> BeforeAddingModels { get; set; }

        public TestContext(DbContextOptions options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BeforeAddingModel>()
                .Property(m => m.SavedAt)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
