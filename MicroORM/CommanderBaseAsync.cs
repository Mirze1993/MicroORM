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

        public abstract DbParameter SetParametr(string paramName, object value, int size);
        public abstract DbParameter SetParametr(string paramName, object value);
        public abstract DbParameter SetParametr();

        public abstract DbParameter SetReturnParametr();
        public abstract DbParameter SetReturnParametr(string paramName);


        public abstract DbParameter SetOutParametr(string paramName, System.Data.DbType dbType);
        public abstract DbParameter SetOutParametr(string paramName, System.Data.DbType dbType, int size);

        public abstract DbParameter SetInputOutputParametr(string paramName, object value);


        protected async Task ConnectionOpenAsync()
        {
            if (connection.State != ConnectionState.Open)await connection.OpenAsync();
        }
        public async Task<DbTransaction> TransactionStartAsync()
        {
            return await connection.BeginTransactionAsync(IsolationLevel.Serializable);
        }

        protected Result CommandStart(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {
            try
            {
                command = connection.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = commandType;
                if (parameters != null) command.Parameters.AddRange(parameters.ToArray());
                if (transaction != null) command.Transaction = transaction;
            }
            catch (Exception e)
            {
                 new Logging.LogWriteFile().WriteFile($"CommandStart error {e.Message}", LogLevel.Error);
                return new Result(false, e.Message);
            }
            return new Result();
        }

        public async Task<Result> NonQueryAsync(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {  
            try
            {
                var cs = CommandStart(commandText, parameters, commandType, transaction);
                if (!cs.Success)
                    return await Task.FromResult(cs);
                await ConnectionOpenAsync();               
                return new Result() { Success = await command.ExecuteNonQueryAsync() > 0 };
            }
            catch (Exception e)
            {
                new Logging.LogWriteFile().WriteFile(e.Message, LogLevel.Error);
                return new Result(false, e.Message);
            }
                       
        }
        public async Task<Result<object>> ScallerAsync(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {
            try
            {
                var cs = CommandStart(commandText, parameters, commandType, transaction);
                if (!cs.Success)
                    return new Result<object> { Success = false, Message = cs.Message };
                await ConnectionOpenAsync();
               
                return new Result<object> { Value = await command.ExecuteScalarAsync() };
            }
            catch (Exception e)
            {
                await new Logging.LogWriteFile().WriteFileAsync(e.Message, LogLevel.Error);
                return new Result<object> { Success = false, Message = e.Message, Value = 0 };
            }
        }

        //reader
        public async Task< Result<T>> ReaderAsync<T>(Func<DbDataReader, Task<T>> readMetod, string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {
            try
            {
                var cs = CommandStart(commandText, parameters, commandType, transaction);
                if (!cs.Success)
                    return new Result<T> { Success = false, Message = cs.Message };
                await ConnectionOpenAsync();
                reader =await command.ExecuteReaderAsync();              
                return new Result<T>().SuccessResult(await readMetod(reader));
            }
            catch (Exception e)
            {
                await new Logging.LogWriteFile().WriteFileAsync(e.Message, LogLevel.Error);
                return new Result<T> { Success = false, Message = e.Message };
            }
        }
        public async Task< Result<List<T>>> ReaderAsync<T>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new()
        {
            return await ReaderAsync(GetListAsync<T>, commandText, parameters, commandType, transaction);
        }
        public async Task<Result<T>> ReaderFistAsync<T>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new()
        {
            return await ReaderAsync(GetFistAsync<T>, commandText, parameters, commandType, transaction);
        }
        public async Task<Result<List<T>>> ReaderLeftJoinAsync<T, M>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new() where M : class, new()
        {
            return await ReaderAsync( GetListLeftJoinAsync<T, M>, commandText, parameters, commandType, transaction);
        }
        public async Task<Result<T>> ReaderLeftJoinFistAsync<T, M>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new() where M : class, new()
        {
            return await ReaderAsync(GetFistLeftJoinAsync<T, M>, commandText, parameters, commandType, transaction);
        }

        protected async Task<List<T>> GetListAsync<T>(DbDataReader r) where T : class, new()
        {
            List<T> list = new List<T>();
            if (r == null) return list;
            var fieldNames = Enumerable.Range(0, r.FieldCount).Select(i => r.GetName(i)).ToArray();
            while (await r.ReadAsync())
            {
                var t = GetValues<T>(r);
                list.Add(t);
            }
            if (!r.IsClosed)await r.CloseAsync();
            return list;
        }
        protected async Task<List<T>> GetListLeftJoinAsync<T, M>(DbDataReader r) where T : class, new() where M : class, new()
        {
            Dictionary<int, T> d = new Dictionary<int, T>();
            if (r == null) return new List<T>();

            while (await r.ReadAsync())
            {
                T t = null;               
                bool b = false; ;
                if (int.TryParse(r["Id"].ToString(), out int  id))
                    b = d.TryGetValue(id, out t);
                if (t == null)
                {
                    t = GetValues<T>(r);
                }
                var m = GetValues<M>(r);
                typeof(T).GetMethod("Join").Invoke(t, new[] { m });
                if (!b) d.Add(id, t);
            }
            if (!r.IsClosed)await r.CloseAsync();
            return d.Values.ToList();
        }

        protected async Task<T> GetFistAsync<T>(DbDataReader r) where T : class, new()
        {
            if (r == null) return null;
            if (!r.HasRows) return null;
            T t = new T();
            while (await r.ReadAsync())
            {
                t = GetValues<T>(r);
                break;
            }
            if (!r.IsClosed)await r.CloseAsync();
            return t;
        }
        protected async Task<T> GetFistLeftJoinAsync<T, M>(DbDataReader r) where T : class, new() where M : class, new()
        {
            if (r == null) return null;
            if (!r.HasRows) return null;

            Dictionary<int, T> d = new Dictionary<int, T>();           
            while (await r.ReadAsync())
            {
                try
                {
                    T t = null;
                    bool b = false; ;
                    if (int.TryParse(r["Id"].ToString(), out int id))
                        b = d.TryGetValue(id, out t);
                    if (t == null)
                    {
                        t = GetValues<T>(r);
                    }
                    var m = GetValues<M>(r);
                    typeof(T).GetMethod("Join").Invoke(t, new[] { m });
                    if (!b) d.Add(id, t);
                    if (d.Count > 1)                    
                        break;
                    
                }catch(Exception e)
                {
                    await new LogWriteFile().WriteFileAsync("Metod GetFistLeftJoin:" + e.Message, LogLevel.Error);
                }
            }
            if (!r.IsClosed)await r.CloseAsync();
            return d.Values?.ToList().FirstOrDefault();
        }

        public T GetValues<T>(DbDataReader r) where T : class, new()
        {
            T t = new T();
            foreach (var item in typeof(T).GetProperties())
            {
                if (item.PropertyType.GetInterfaces().Contains(typeof(IEnumerable<>)))
                    continue;
                var attribute = (DbMapingAttribute)Attribute.GetCustomAttribute(item, typeof(DbMapingAttribute));
                if (attribute != null) if (attribute.Map == DbMap.noMaping) continue;
                try
                {
                    var value = r[item.Name];
                    if (value == null) continue;
                    if (item.PropertyType.IsEnum)
                        item.SetValue(t, Enum.Parse(item.PropertyType, value.ToString()), null);

                    else if (IsNullableEnum(item.PropertyType))
                    {
                        item.SetValue(t, Enum.Parse(Nullable.GetUnderlyingType(item.PropertyType), value.ToString()));
                    }
                    else item.SetValue(t, value);
                }
                catch { }
            }
            return t;
        }
        bool IsNullableEnum(Type t)
        {
            if (Nullable.GetUnderlyingType(t) != null)
                if (Nullable.GetUnderlyingType(t).IsEnum)
                    return true;
            return false;
        }

        public async ValueTask DisposeAsync()
        {
            if (reader != null)
                if (!reader.IsClosed) await reader.CloseAsync();
            command?.DisposeAsync();
            if (connection == null) return;
            if (connection.State != ConnectionState.Closed) await connection.CloseAsync();
            await connection.DisposeAsync();
        }
    }
}
