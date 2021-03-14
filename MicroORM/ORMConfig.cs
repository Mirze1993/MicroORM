
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroORM
{
    public static class ORMConfig
    {
        public static string ConnectionString="";

        public static DbType DbType;

        
    }

    public enum DbType
    {
        MSSQL,
        Oracle,
    }
}
