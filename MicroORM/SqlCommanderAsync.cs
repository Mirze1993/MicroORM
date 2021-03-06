using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace MicroORM
{
    public class SqlCommanderAsync:CommanderBaseAsync
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
        public override DbParameter SetParametr()
        {
            return new SqlParameter();
        }

        public override DbParameter SetOutputParametr(string paramName)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = paramName;
            p.Direction = System.Data.ParameterDirection.ReturnValue;
            return p;
        }

        public  SqlCommanderAsync()
        {
            connectionString = ORMConfig.ConnectionString;
            try
            {
                connection = new SqlConnection(connectionString);
            }
            catch (Exception e)
            {
                new Logging.LogWriteFile().WriteFile(e.Message, Logging.LogLevel.Error);
            }
        }
    }
}
