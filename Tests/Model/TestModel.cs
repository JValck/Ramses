using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Model
{
    public abstract class TestModel
    {
        /// <summary>
        /// Indicates if the lifecycle callback method is called
        /// </summary>
        public bool CallbackCalled
        {
            protected set;
            get;
        }
    }
}
