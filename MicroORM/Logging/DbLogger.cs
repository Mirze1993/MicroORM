using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroORM.Logging
{
    public static class DbLogger
    {

        public static async Task<Result> WriteDbAsync(string txt)
        {

            if (string.IsNullOrEmpty(txt) || !DBLoggerOptions.IsDbLogger)
                return new Result().ErrorResult("txt is null or IsDbLogger false");

            var sql = $"INSERT INTO {DBLoggerOptions.LogDbName} VALUES (@LogDate, @Text)";

            await using (var commander = new DBContext().CreateCommanderAsync())
            {
                List<System.Data.Common.DbParameter> p = new List<System.Data.Common.DbParameter>() {
                    commander.SetParametr("LogDate",DateTime.Now),
                    commander.SetParametr("Text",txt)
                    };
                var result = await commander
                    .NonQueryAsync(
                     commandText: sql
                    , commandType: System.Data.CommandType.Text
                    , parameters: p
                    );
                return result;
            }
        }

        static readonly object _object = new object();
        public static  Result WriteDb(string txt)
        {
            if (string.IsNullOrEmpty(txt) || !DBLoggerOptions.IsDbLogger)
                return new Result().ErrorResult("txt is null or IsDbLogger false");

            var sql = $"INSERT INTO {DBLoggerOptions.LogDbName} VALUES (@LogDate, @Text)";
            lock (_object)
            {
                using (var commander = new DBContext().CreateCommander())
                {
                    List<System.Data.Common.DbParameter> p = new List<System.Data.Common.DbParameter>() {
                    commander.SetParametr("LogDate",DateTime.Now),
                    commander.SetParametr("Text",txt)
                    };
                    var result = commander
                        .NonQuery(
                         commandText: sql
                        , commandType: System.Data.CommandType.Text
                        , parameters: p
                        );
                    return result;
                }
            }
        }
    }

}

