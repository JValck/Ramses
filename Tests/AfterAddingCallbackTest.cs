using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Model.After;
using Xunit;

namespace Tests
{
    public class AfterAddingCallbackTest
    {
        private AfterAddingModel afterAddingModel;

        public AfterAddingCallbackTest()
        {
            afterAddingModel = new AfterAddingModel();
            using (var dbContext = new TestLifecycleDbContext())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.Database.ExecuteSqlCommand(
                @"
                    CREATE TRIGGER SetSavedAtAfterAddingTrigger
                    AFTER INSERT ON aa_models
                    BEGIN
                        UPDATE aa_models
                        SET SavedAt = strftime('%s','now')
                        WHERE Id = NEW.Id;
                    END
                ");
            }
        }

        [Fact]
        public void SaveWithLifecyclesCallsLifecycleHookAfterSaving()
        {
            using(var dbContext = new TestLifecycleDbContext())
            {
                dbContext.AfterAddingModels.Add(afterAddingModel);
                Assert.False(afterAddingModel.CallbackCalled);

                var saved = dbContext.SaveWithLifecycles();
                Assert.Equal(1, saved);

            }
            using (var dbContext = new TestLifecycleDbContext())
            {
                var refreshed = dbContext.AfterAddingModels.First(m => m.Id == afterAddingModel.Id);                
                Assert.True(afterAddingModel.CallbackCalled);
                Assert.True(afterAddingModel.CallbackCalledAt > refreshed.GetSavedAtInDateTime());
            }
        }
    }
}
