using Com.Setarit.Ramses.LifecycleListener;
using System;

namespace Tests.Model.Before
{
    public class BeforeAddingModel : TestModel, IBeforeAddingListener
    {
        public void BeforeAdding()
        {
            CallbackCalled = true;
            CallbackCalledAt = DateTime.UtcNow;
        }
    }
}
