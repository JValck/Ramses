using Com.Setarit.Ramses.LifecycleListener;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace Tests.Model.Before
{
    [Table(name: "ba_models")]
    public class BeforeAddingModel : TestModel, IBeforeAddingListener
    {
        public void BeforeAdding()
        {
            CallbackCalled = true;
            CallbackCalledAt = DateTime.UtcNow;
            Thread.Sleep(1000);
        }
    }
}
