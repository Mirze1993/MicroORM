
using MicroORM.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace MicroORM
{
    public class SqlCommander : CommanderBase
    {      

        public override List<DbParameter> SetParametrs<T>(T t)
        {
            List<DbParameter> parametrs = new List<DbParameter>();
            if (t != null)
                foreach (var item in typeof(T).GetProperties())
                {
                    if (item.Name == "Id") continue;
                    object value = item.GetValue(t);
                    if (value == null) value = DBNull.Value;
                    parametrs.Add(new SqlParameter($"@{item.Name}", value));
                }
            return parametrs;
        }

        public override DbParameter SetParametr(string paramName, object value)
        {
            return new SqlParameter($"@{paramName}", value);
        }

        public override DbParameter SetOutputParametr()
        {
            SqlParameter p = new SqlParameter();
            p.Direction = System.Data.ParameterDirection.ReturnValue;
            return p;
        }


        public SqlCommander()
        {
            connectionString = ORMConfig.ConnectionString;
            try
            {
                connection = new SqlConnection(connectionString);
            }
            catch (Exception e)
            {
                logManager.WriteFile(e.Message, LogLevel.Error);
            }
                        
        }


    }
}
