using Com.Setarit.Ramses.LifecycleListener;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading;

namespace Tests.Model.After
{
    [Table(name: "aa_models")]
    public class AfterAddingModel : TestModel, IAfterAddingListener
    {        
        public void AfterAdding()
        {
            CallbackCalled = true;
            CallbackCalledAt = DateTime.UtcNow;
            Thread.Sleep(1000);
        }
    }
}
