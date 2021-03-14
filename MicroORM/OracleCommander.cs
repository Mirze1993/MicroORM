
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OracleClient;


namespace MicroORM
{
    class OracleCommander : CommanderBase
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
                    parametrs.Add(new OracleParameter($"@{item.Name}", value));
                }
            return parametrs;
        }

        public override DbParameter SetParametr(string paramName, object value)
        {
            return new OracleParameter(paramName, value);
        }
        public override DbParameter SetOutputParametr()
        {
            OracleParameter p = new OracleParameter();
            p.Direction = System.Data.ParameterDirection.ReturnValue;
            return p;
        }
        public OracleCommander()
        {
            connectionString = ORMConfig.ConnectionString;
            try
            {
                connection = new OracleConnection(connectionString);
            }
            catch (Exception e)
            {
                //LogManager.GetLogger(GetType().Name).Error(e.Message);
            }
        }
    }
}
