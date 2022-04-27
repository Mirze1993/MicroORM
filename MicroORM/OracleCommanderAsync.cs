using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace MicroORM
{
    public class OracleCommanderAsync:CommanderBaseAsync
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

        public override DbParameter SetParametr()
        {
            return new OracleParameter();
        }
        public override DbParameter SetParametr(string paramName, object value)
        {
            return new OracleParameter(paramName, value ?? DBNull.Value);
        }



        public override DbParameter SetReturnParametr()
        {
            OracleParameter p = new OracleParameter();
            p.Direction = System.Data.ParameterDirection.ReturnValue;
            return p;
        }

        public override DbParameter SetReturnParametr(string paramName)
        {
            OracleParameter p = new OracleParameter();
            p.ParameterName = paramName;
            p.Direction = System.Data.ParameterDirection.ReturnValue;
            return p;
        }



        public override DbParameter SetOutParametr(string paramName, System.Data.DbType dbType)
        {
            OracleParameter p = new OracleParameter();
            p.ParameterName = "@" + paramName;
            p.DbType = dbType;
            p.Direction = System.Data.ParameterDirection.Output;
            return p;
        }


        public override DbParameter SetInputOutputParametr(string paramName, object value)
        {
            OracleParameter p = new OracleParameter("@" + paramName, value);
            p.Direction = System.Data.ParameterDirection.InputOutput;
            return p;
        }

        public OracleCommanderAsync()
        {
            connectionString = ORMConfig.ConnectionString;
            try
            {
                connection = new OracleConnection(connectionString);
            }
            catch (Exception e)
            {
                new Logging.LogWriteFile().WriteFile(e.Message, Logging.LogLevel.Error);
            }
        }
    }
}
