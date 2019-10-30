using Com.Setarit.Ramses.LifecycleListener;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Com.Setarit.Ramses
{
    public class LifecycleDbContext : DbContext
    {
        public LifecycleDbContext(DbContextOptions options): base(options) { }

        public int SaveWithLifecycles() => this.SaveWithLifecycles(acceptAllChangesOnSuccess: true);
        public Task<int> SaveWithLifecyclesAsync(CancellationToken cancellationToken = default)
            => this.SaveWithLifecyclesAsync(acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);

        public int SaveWithLifecycles(bool acceptAllChangesOnSuccess)
        {
            var changedEntries = GetChangedEntries();

            //before saving
            HandleBeforeSaving(changedEntries);

            //save
            var entitiesSaved = base.SaveChanges(acceptAllChangesOnSuccess);

            //after saving
            HandleAfterSaving(changedEntries);

            return entitiesSaved;
        }
        public async Task<int> SaveWithLifecyclesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var changedEntries = GetChangedEntries();

            //before saving
            HandleBeforeSaving(changedEntries);

            //save
            var entitiesSaved = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            //after saving
            HandleAfterSaving(changedEntries);

            return entitiesSaved;
        }

        private void HandleAfterSaving(IEnumerable<EntityEntry> changedEntries)
        {            
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Added && e.Entity.GetType().GetInterface(typeof(IAfterAddingListener).FullName) != null))
            {
                IAfterAddingListener listener = e.Entity as IAfterAddingListener;
                listener.AfterAdding();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Deleted && e.Entity.GetType().GetInterface(typeof(IAfterDeletionListener).FullName) != null))
            {
                IAfterDeletionListener listener = e.Entity as IAfterDeletionListener;
                listener.AfterDeletion();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Modified && e.Entity.GetType().GetInterface(typeof(IAfterUpdateListener).FullName) != null))
            {
                IAfterUpdateListener listener = e.Entity as IAfterUpdateListener;
                listener.AfterUpdate();
            }
        }

        /// <summary>
        /// Gets the changed entries according to the change tracker
        /// </summary>
        /// <returns>The changed entities</returns>
        public IEnumerable<EntityEntry> GetChangedEntries()
        {
            base.ChangeTracker.DetectChanges();
            return base.ChangeTracker.Entries(); 
        }

        private void HandleBeforeSaving(IEnumerable<EntityEntry> changedEntries)
        {
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Added && e.Entity.GetType().GetInterface(typeof(IBeforeAddingListener).FullName) != null))
            {
                IBeforeAddingListener listener = e.Entity as IBeforeAddingListener;
                listener.BeforeAdding();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Deleted && e.Entity.GetType().GetInterface(typeof(IBeforeDeletionListener).FullName) != null))
            {
                IBeforeDeletionListener listener = e.Entity as IBeforeDeletionListener;
                listener.BeforeDeletion();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Modified && e.Entity.GetType().GetInterface(typeof(IBeforeUpdateListener).FullName) != null))
            {
                IBeforeUpdateListener listener = e.Entity as IBeforeUpdateListener;
                listener.BeforeUpdate();
            }
        }

    }
}
