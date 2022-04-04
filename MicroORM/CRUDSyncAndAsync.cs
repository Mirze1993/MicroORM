using System;
using System.Collections.Generic;
using System.Text;

namespace MicroORM
{
    public abstract class CRUDSyncAndAsync<T> where T : class, new()
    {
        private CRUD<T> syncMetod;

        public CRUD<T> SyncMetod
        {
            get { return syncMetod??new Csync<T>(); }
            private set { syncMetod = value; }
        }

        private CRUDAsync<T> asyncMetod;

        public CRUDAsync<T> AsyncMetod
        {
            get { return asyncMetod ?? new Casync<T>(); }
            private set { asyncMetod = value; }
        }
    }
}
