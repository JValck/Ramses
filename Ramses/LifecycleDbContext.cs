using Com.Setarit.Ramses.ChangeTracker;
using Com.Setarit.Ramses.LifecycleListener;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Com.Setarit.Ramses
{
    public class LifecycleDbContext : DbContext
    {
        /// <summary>
        /// Parameterless constructor, required for allowing migrations
        /// </summary>
        public LifecycleDbContext() { }

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

        private void HandleAfterSaving(IEnumerable<Entity> changedEntries)
        {            
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Added && e.Item.GetType().GetInterface(typeof(IAfterAddingListener).FullName) != null))
            {
                IAfterAddingListener listener = e.Item as IAfterAddingListener;
                listener.AfterAdding();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Deleted && e.Item.GetType().GetInterface(typeof(IAfterDeletionListener).FullName) != null))
            {
                IAfterDeletionListener listener = e.Item as IAfterDeletionListener;
                listener.AfterDeletion();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Modified && e.Item.GetType().GetInterface(typeof(IAfterUpdateListener).FullName) != null))
            {
                IAfterUpdateListener listener = e.Item as IAfterUpdateListener;
                listener.AfterUpdate();
            }
        }

        /// <summary>
        /// Gets the changed entries according to the change tracker and returns them as a copy
        /// so they won't get updated if the changetracker updates the entity state
        /// </summary>
        /// <returns>The changed entities</returns>
        public IEnumerable<Entity> GetChangedEntries()
        {
            base.ChangeTracker.DetectChanges();
            var entries = base.ChangeTracker.Entries();
            var clone = new ConcurrentBag<Entity>();
            Parallel.ForEach(entries, (entry) =>
            {
                clone.Add(new Entity
                {
                    State = entry.State,
                    Item = entry.Entity,
                });
            });
            return clone;
        }

        private void HandleBeforeSaving(IEnumerable<Entity> changedEntries)
        {
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Added && e.Item.GetType().GetInterface(typeof(IBeforeAddingListener).FullName) != null))
            {
                IBeforeAddingListener listener = e.Item as IBeforeAddingListener;
                listener.BeforeAdding();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Deleted && e.Item.GetType().GetInterface(typeof(IBeforeDeletionListener).FullName) != null))
            {
                IBeforeDeletionListener listener = e.Item as IBeforeDeletionListener;
                listener.BeforeDeletion();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Modified && e.Item.GetType().GetInterface(typeof(IBeforeUpdateListener).FullName) != null))
            {
                IBeforeUpdateListener listener = e.Item as IBeforeUpdateListener;
                listener.BeforeUpdate();
            }
        }

    }
}
