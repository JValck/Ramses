using Com.Setarit.Ramses.LifecycleListener;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Com.Setarit.Ramses
{
    public class LifecycleDbContext : DbContext
    {
        public int SaveWithLifecycles(bool acceptAllChangesOnSuccess)
        {
            base.ChangeTracker.DetectChanges();
            var changedEntries = base.ChangeTracker.Entries();

            //before saving
            HandleBeforeSaving(changedEntries);

            //save
            var entitiesSaved = base.SaveChanges(acceptAllChangesOnSuccess);

            //after saving
            HandleAfterSaving(changedEntries);

            return entitiesSaved;
        }

        private void HandleAfterSaving(System.Collections.Generic.IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> changedEntries)
        {
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Added && e.Entity.GetType().IsSubclassOf(typeof(IAfterAddingListener))))
            {
                IAfterAddingListener listener = e.Entity as IAfterAddingListener;
                listener.AfterAdding();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Deleted && e.Entity.GetType().IsSubclassOf(typeof(IAfterDeletionListener))))
            {
                IAfterDeletionListener listener = e.Entity as IAfterDeletionListener;
                listener.AfterDeletion();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Modified && e.Entity.GetType().IsSubclassOf(typeof(IAfterUpdateListener))))
            {
                IAfterUpdateListener listener = e.Entity as IAfterUpdateListener;
                listener.AfterUpdate();
            }
        }

        private void HandleBeforeSaving(System.Collections.Generic.IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> changedEntries)
        {
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Added && e.Entity.GetType().IsSubclassOf(typeof(IBeforeAddingListener))))
            {
                IBeforeAddingListener listener = e.Entity as IBeforeAddingListener;
                listener.BeforeAdding();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Deleted && e.Entity.GetType().IsSubclassOf(typeof(IBeforeDeletionListener))))
            {
                IBeforeDeletionListener listener = e.Entity as IBeforeDeletionListener;
                listener.BeforeDeletion();
            }
            foreach (var e in changedEntries.Where(e => e.State == EntityState.Modified && e.Entity.GetType().IsSubclassOf(typeof(IBeforeUpdateListener))))
            {
                IBeforeUpdateListener listener = e.Entity as IBeforeUpdateListener;
                listener.BeforeUpdate();
            }
        }

        public async System.Threading.Tasks.Task<int> SaveWithLifecyclesAsync(bool acceptAllChangesOnSuccess)
        {
            base.ChangeTracker.DetectChanges();
            var changedEntries = base.ChangeTracker.Entries();

            //before saving
            HandleBeforeSaving(changedEntries);

            //save
            var entitiesSaved = await base.SaveChangesAsync(acceptAllChangesOnSuccess);

            //after saving
            HandleAfterSaving(changedEntries);

            return entitiesSaved;
        }
    }
}
