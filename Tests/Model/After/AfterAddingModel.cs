using Com.Setarit.Ramses.LifecycleListener;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Model.After
{
    public class AfterAddingModel : TestModel, IAfterAddingListener
    {        
        public void AfterAdding()
        {
            CallbackCalled = true;
            CallbackCalledAt = DateTime.UtcNow;
        }
    }
}
