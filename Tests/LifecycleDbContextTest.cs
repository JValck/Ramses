using Moq;
using System;
using Xunit;
using Com.Setarit.Ramses.LifecycleListener;
using Com.Setarit.Ramses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Tests.Model.Before;

namespace Tests
{
    public class LifecycleDbContextTest
    {
        private readonly DbContextOptions<TestContext> options;
        private BeforeAddingModel beforeAddingModel;        

        public LifecycleDbContextTest()
        {
            beforeAddingModel = new BeforeAddingModel();
            options = new DbContextOptionsBuilder<TestContext>()
                    .UseInMemoryDatabase(databaseName: "Test")
                    .Options;            
        }

        [Fact]
        public void SaveWithLifecyclesCallsLifecycleHookBeforeSaving()
        {
            using (var dbContext = new TestContext(options))
            {
                dbContext.BeforeAddingModels.Add(beforeAddingModel);
                Assert.False(beforeAddingModel.CallbackCalled);

                var saved = dbContext.SaveWithLifecycles();
                Assert.Equal(1, saved);

                Assert.True(beforeAddingModel.CallbackCalled);
                Assert.True(beforeAddingModel.CallbackCalledAt < beforeAddingModel.SavedAt);
            }
        }

    }
}
