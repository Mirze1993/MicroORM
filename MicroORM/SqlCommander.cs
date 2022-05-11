
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
                    parametrs.Add(new SqlParameter($"@{item.Name}", value ?? DBNull.Value));
                }
            return parametrs;
        }
        public override DbParameter SetParametr()
        {
            return new SqlParameter();
        }

        public override DbParameter SetParametr(string paramName, object value)
        {
            return new SqlParameter($"@{paramName}", value ?? DBNull.Value);
        }
        public override DbParameter SetParametr(string paramName, object value, int size)
        {
            return new SqlParameter($"@{paramName}", value ?? DBNull.Value) { Size = size };
        }

        public override DbParameter SetReturnParametr()
        {
            return new SqlParameter() { Direction = System.Data.ParameterDirection.ReturnValue };
        }

        public override DbParameter SetReturnParametr(string paramName)
        {
            return new SqlParameter() { ParameterName = "@" + paramName, Direction = System.Data.ParameterDirection.ReturnValue };
        }


        public override DbParameter SetOutParametr(string paramName, System.Data.DbType dbType)
        {
            return new SqlParameter()
            {
                ParameterName = "@" + paramName,
                DbType = dbType,
                Direction = System.Data.ParameterDirection.Output
            };
        }

        public override DbParameter SetOutParametr(string paramName, System.Data.DbType dbType, int size)
        {
            return new SqlParameter()
            {
                ParameterName = "@" + paramName,
                DbType = dbType,
                Direction = System.Data.ParameterDirection.Output,
                Size = size
            };
        }
        public override DbParameter SetInputOutputParametr(string paramName, object value)
        {
            return new SqlParameter("@" + paramName, value) { Direction = System.Data.ParameterDirection.InputOutput };
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
                new Logging.LogWriteFile().WriteFile(e.Message, LogLevel.Error);
            }

        }


    }
}
