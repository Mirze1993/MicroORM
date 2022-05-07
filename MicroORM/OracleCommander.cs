using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;



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

        public override DbParameter SetParametr()
        {
            return new OracleParameter();
        }
        public override DbParameter SetParametr(string paramName, object value)
        {
            return new OracleParameter(paramName, value ?? DBNull.Value);
        }

        public override DbParameter SetParametr(string paramName, object value, int size)
        {
            return new OracleParameter($"@{paramName}", value ?? DBNull.Value) { Size = size };
        }

        public override DbParameter SetReturnParametr()
        {
            return new OracleParameter() { Direction = System.Data.ParameterDirection.ReturnValue };
        }

        public override DbParameter SetReturnParametr(string paramName)
        {
            return new OracleParameter()
            {
                ParameterName = "@" + paramName,
                Direction = System.Data.ParameterDirection.ReturnValue
            };
        }



        public override DbParameter SetOutParametr(string paramName, System.Data.DbType dbType)
        {
            return new OracleParameter()
            {
                ParameterName = "@" + paramName,
                DbType = dbType,
                Direction = System.Data.ParameterDirection.Output
            };
        }


        public override DbParameter SetInputOutputParametr(string paramName, object value)
        {
            return new OracleParameter("@" + paramName, value) { Direction = System.Data.ParameterDirection.InputOutput };
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
                new Logging.LogWriteFile().WriteFile(e.Message, Logging.LogLevel.Error);
            }
        }
    }
}
