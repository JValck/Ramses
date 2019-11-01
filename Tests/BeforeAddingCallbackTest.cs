using Xunit;
using Microsoft.EntityFrameworkCore;
using Tests.Model.Before;
using System.Linq;
using System;

namespace Tests
{
    public class BeforeAddingCallbackTest 
    {
        private BeforeAddingModel beforeAddingModel;        

        public BeforeAddingCallbackTest()
        {
            beforeAddingModel = new BeforeAddingModel();
            using (var dbContext = new TestLifecycleDbContext())
            {
                dbContext.Database.EnsureDeleted();
            }
            using (var dbContext = new TestLifecycleDbContext())
            {
                //dbContext.Database.EnsureDeleted();
                try
                {
                    dbContext.Database.EnsureCreated();
                    dbContext.Database.ExecuteSqlCommand(
                    @"
                    CREATE TRIGGER SetSavedAtBeforeAddingTrigger
                    AFTER INSERT ON ba_models
                    BEGIN
                        UPDATE ba_models
                        SET SavedAt = strftime('%s','now')
                        WHERE Id = NEW.Id;
                    END
                ");
                }
                catch  { }
            }
        }

        [Fact]
        public void SaveWithLifecyclesCallsLifecycleHookBeforeSaving()
        {
            using (var dbContext = new TestLifecycleDbContext())
            {
                dbContext.BeforeAddingModels.Add(beforeAddingModel);
                Assert.False(beforeAddingModel.CallbackCalled);

                var saved = dbContext.SaveWithLifecycles();
                Assert.Equal(1, saved);

                Assert.True(beforeAddingModel.CallbackCalled);

            }
            using (var dbContext = new TestLifecycleDbContext())
            {                
                var refreshed = dbContext.BeforeAddingModels.First(m => m.Id == beforeAddingModel.Id);
                var x = refreshed.GetSavedAtInDateTime();
                Assert.True(refreshed.CallbackCalledAt < refreshed.GetSavedAtInDateTime());
            }
        }

    }
}
