using MicroORM.Logging;
using MicroORM.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroORM
{
    public abstract class CommanderBaseAsync:IAsyncDisposable 
    {
        protected DbConnection connection;
        public DbDataReader reader;
        protected DbCommand command;


        protected string connectionString;

        public abstract List<DbParameter> SetParametrs<T>(T t);


        public abstract DbParameter SetParametr(string paramName, object value);


        public abstract DbParameter SetOutputParametr();

        protected async Task ConnectionOpenAsync()
        {
            if (connection.State != ConnectionState.Open)await connection.OpenAsync();
        }


        public async Task<DbTransaction> TransactionStartAsync()
        {
            return await connection.BeginTransactionAsync(IsolationLevel.Serializable);
        }

        protected void CommandStart(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {
            command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            if (parameters != null) command.Parameters.AddRange(parameters.ToArray());
            if (transaction != null) command.Transaction = transaction;
        }



        public async Task<bool> NonQueryAsync(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {

            CommandStart(commandText, parameters, commandType, transaction);
            await ConnectionOpenAsync();
            bool b = false;
            try
            {
                b =await command.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception e)
            {
                await new LogWriteFile().WriteFileAsync(e.Message, LogLevel.Error);
            }
            return b;
        }


        public async Task<(object, bool)> ScallerAsync(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {

            CommandStart(commandText, parameters, commandType, transaction);
            await ConnectionOpenAsync();
            object b = null;
            try
            {
                b =await command.ExecuteScalarAsync();
                return (b, true);
            }
            catch (Exception e)
            {
                await new LogWriteFile().WriteFileAsync(e.Message, LogLevel.Error);
                return (0, false);
            }
        }

        //reader
        public async Task<(T, bool)> ReaderAsync<T>( Func<DbDataReader, Task<T>> readMetod, string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {

            CommandStart(commandText, parameters, commandType, transaction);
            await ConnectionOpenAsync();
            try
            {
                reader = await command.ExecuteReaderAsync();
            }
            catch (Exception e)
            {
                await new LogWriteFile().WriteFileAsync(e.Message, LogLevel.Error);
                return (default(T), false);
            }
            var t = await readMetod(reader);
            return (t, true);
        }


        public async Task<(List<T>, bool)> ReaderAsync<T>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new()
        {
            
            return await ReaderAsync(GetListAsync<T>, commandText, parameters, commandType, transaction);
        }

        public async Task<(T, bool)> ReaderFistAsync<T>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new()
        {
            return await ReaderAsync(GetFistAsync<T>, commandText, parameters, commandType, transaction);
        }


        protected async Task<List<T>> GetListAsync<T>(DbDataReader r) where T : class, new()
        {
            List<T> list = new List<T>();
            if (r == null) return list;
            var fieldNames = Enumerable.Range(0, r.FieldCount).Select(i => r.GetName(i)).ToArray();
            while (await r.ReadAsync())
            {
                T t = new T();
                foreach (var item in typeof(T).GetProperties())
                {
                    if (!fieldNames.Contains(item.Name)) continue;
                    var attribute = (DbMapingAttribute)Attribute.GetCustomAttribute(item, typeof(DbMapingAttribute));
                    if (attribute != null) if (attribute.Map == DbMap.noMaping) continue;
                    try
                    {
                        var value = r[item.Name];
                        if (value == null) continue;
                        if (item.PropertyType.IsEnum)
                            item.SetValue(t, Enum.Parse(item.PropertyType, value.ToString()), null);

                        else if (isNullableEnum(item.PropertyType))
                        {
                            item.SetValue(t, Enum.Parse(Nullable.GetUnderlyingType(item.PropertyType), value.ToString()));
                        }
                        else item.SetValue(t, value);
                    }
                    catch { }
                }
                list.Add(t);
            }
            if (!r.IsClosed) await r.CloseAsync();
            return list;
        }
        bool isNullableEnum(Type t)
        {
            if (Nullable.GetUnderlyingType(t) != null)
                if (Nullable.GetUnderlyingType(t).IsEnum)
                    return true;
            return false;
        }
        protected async Task<T> GetFistAsync<T>(DbDataReader r) where T : class, new()
        {
            if (r == null) return null;
            if (!r.HasRows) return null;
            T t = new T();
            while (await r.ReadAsync())
            {
                foreach (var item in typeof(T).GetProperties())
                {
                    var attribute = (DbMapingAttribute)Attribute.GetCustomAttribute(item, typeof(DbMapingAttribute));
                    if (attribute != null) if (attribute.Map == DbMap.noMaping) continue;
                    try
                    {
                        var value = r[item.Name];
                        if (item.PropertyType.IsEnum)
                            item.SetValue(t, Enum.Parse(item.PropertyType, value.ToString()), null);

                        else if (isNullableEnum(item.PropertyType))
                        {
                            item.SetValue(t, Enum.Parse(Nullable.GetUnderlyingType(item.PropertyType), value.ToString()));
                        }
                        else item.SetValue(t, value);
                    }
                    catch { }
                }
                break;
            }
            if (!r.IsClosed)await r.CloseAsync();
            return t;
        }


        public async ValueTask DisposeAsync()
        {
            if (reader != null)
                if (!reader.IsClosed) await reader.CloseAsync();
            command?.DisposeAsync();
            if (connection == null) return;
            if (connection.State != ConnectionState.Closed)await connection.CloseAsync();
            await connection.DisposeAsync();
        }

    }
}
