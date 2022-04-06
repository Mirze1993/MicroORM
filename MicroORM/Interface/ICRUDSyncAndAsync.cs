using System;
using System.Collections.Generic;
using System.Text;

namespace MicroORM.Interface
{
    public interface ICRUDSyncAndAsync<T>:ICRUD<T>,ICRUDAsync<T> where T: class, new ()
    {
    }
}
