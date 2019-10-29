using Com.Setarit.Ramses.LifecycleListener;

namespace Tests.Model.Before
{
    public class BeforeAddingModel : TestModel, IBeforeAddingListener
    {
        public void BeforeAdding()
        {
            CallbackCalled = true;
        }
    }
}
