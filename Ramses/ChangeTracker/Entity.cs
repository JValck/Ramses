using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Com.Setarit.Ramses.ChangeTracker
{
    public class Entity
    {
        public object Item { get; internal set; }
        public EntityState State { get; internal set; }
    }
}
