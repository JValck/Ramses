using Com.Setarit.Ramses;
using Microsoft.EntityFrameworkCore;
using Tests.Model.Before;

namespace Tests
{
    public class TestContext : LifecycleDbContext
    {
        public DbSet<BeforeAddingModel> BeforeAddingModels { get; set; }

        public TestContext(DbContextOptions options):base(options)
        {
        }

    }
}
