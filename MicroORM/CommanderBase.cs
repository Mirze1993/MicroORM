﻿
using MicroORM.Attributes;
using MicroORM.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace MicroORM
{
    public abstract class CommanderBase : IDisposable
    {
        protected DbConnection connection;
        public DbDataReader reader;
        protected DbCommand command;


        protected string connectionString;

        public abstract List<DbParameter> SetParametrs<T>(T t);


        public abstract DbParameter SetParametr(string paramName, object value);
        public abstract DbParameter SetParametr();

        public abstract DbParameter SetOutputParametr();
        public abstract DbParameter SetOutputParametr(string paramName);

        protected void ConnectionOpen()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
        }



        public DbTransaction TransactionStart()
        {
            return connection.BeginTransaction(IsolationLevel.Serializable);
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
                return new Result(false,e.Message);
            }
            return new Result();
        }

        public Result NonQuery(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {
            var cs = CommandStart(commandText, parameters, commandType, transaction);
            if (!cs.Success)
                return cs;
            ConnectionOpen();
            bool b = false;
            try
            {
                b = command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                new Logging.LogWriteFile().WriteFile(e.Message, LogLevel.Error);
                return new Result(false, e.Message);
            }
            return  new Result();
        }


        public Result<object> Scaller(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {
            var cs = CommandStart(commandText, parameters, commandType, transaction);
            if (!cs.Success)
                return new Result<object>{Success=false,Message= cs.Message };
            ConnectionOpen();
            object b = null;
            try
            {
                b = command.ExecuteScalar();
                return new Result<object> {Value=b };
            }
            catch (Exception e)
            {
                new Logging.LogWriteFile().WriteFile(e.Message, LogLevel.Error);
                return new Result<object> { Success = false, Message = e.Message,Value=0 };
            }
        }



        //reader
        public Result<T> Reader<T>(Func<DbDataReader, T> readMetod, string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {
            var cs = CommandStart(commandText, parameters, commandType, transaction);
            if (!cs.Success)
                return new Result<T> { Success = false, Message = cs.Message };
            ConnectionOpen();
            T t = default(T);
            try
            {
                reader = command.ExecuteReader();
                t = readMetod(reader);
            }
            catch (Exception e)
            {
                new Logging.LogWriteFile().WriteFile(e.Message, LogLevel.Error);
                return  new Result<T> { Success = false, Message = e.Message };
            }
            
            return new Result<T>().SuccessResult(t);
        }



        public Result<List<T>> Reader<T>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new()
        {
            return Reader(GetList<T>, commandText, parameters, commandType, transaction);
        }

        public Result<T> ReaderFist<T>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new()
        {
            return Reader(GetFist<T>, commandText, parameters, commandType, transaction);
        }


        public Result<List<T>> ReaderLeftJoin<T,M>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new() where M : class, new()
        {
            return Reader(GetListLeftJoin<T,M>, commandText, parameters, commandType, transaction);
        }

        public Result<T> ReaderLeftJoinFist<T, M>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new() where M : class, new()
        {
            return Reader(GetFistLeftJoin<T, M>, commandText, parameters, commandType, transaction);
        }

        protected List<T> GetList<T>(DbDataReader r) where T : class, new()
        {
            List<T> list = new List<T>();
            if (r == null) return list;
            var fieldNames = Enumerable.Range(0, r.FieldCount).Select(i => r.GetName(i)).ToArray();
            while (r.Read())
            {
                var t = GetValues<T>(r);
                list.Add(t);
            }
            if (!r.IsClosed) r.Close();
            return list;
        }
        protected List<T> GetListLeftJoin<T, M>(DbDataReader r) where T : class, new() where M : class, new()
        {
            Dictionary<int, T> d = new Dictionary<int, T>();
            if (r == null) return new List<T>();

            while (r.Read())
            {
                try
                {
                    T t = null;
                    int id;
                    bool b = false; ;
                    if (int.TryParse(r["Id"].ToString(), out id))
                        b = d.TryGetValue(id, out t);
                    if (t == null)
                    {
                        t = GetValues<T>(r);
                    }
                    var m = GetValues<M>(r);
                    typeof(T).GetMethod("Join").Invoke(t, new[] { m });
                    if (!b) d.Add(id, t);
                }
                catch(Exception e)
                {
                    new LogWriteFile().WriteFile(e.Message, LogLevel.Error);
                }                
            }
            if (!r.IsClosed) r.Close();
            return d.Values.ToList();
        }
               
        protected T GetFist<T>(DbDataReader r) where T : class, new()
        {
            if (r == null) return null;
            if (!r.HasRows) return null;
            T t = new T();
            while (r.Read())
            {
                 t = GetValues<T>(r);
                break;
            }
            if (!r.IsClosed) r.Close();
            return t;
        }
        protected T GetFistLeftJoin<T,M>(DbDataReader r) where T : class, new() where M : class, new()
        {
            if (r == null) return null;
            if (!r.HasRows) return null;
           
            Dictionary<int, T> d = new Dictionary<int, T>();
            T result = null;
            while (r.Read())
            {
                T t = null;
                int id;
                bool b = false; ;
                if (int.TryParse(r["Id"].ToString(), out id))
                    b = d.TryGetValue(id, out t);
                if (t == null)
                {
                    t = GetValues<T>(r);
                }
                var m = GetValues<M>(r);
                typeof(T).GetMethod("Join").Invoke(t, new[] { m });
                if (!b) d.Add(id, t);
                if (d.Count > 1)
                {
                    result = d.Values.ToList().FirstOrDefault();
                    break;
                }
            }
            if (!r.IsClosed) r.Close();
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

                    else if (isNullableEnum(item.PropertyType))
                    {
                        item.SetValue(t, Enum.Parse(Nullable.GetUnderlyingType(item.PropertyType), value.ToString()));
                    }
                    else item.SetValue(t, value);
                }
                catch { }
            }
            return t;
        }
        bool isNullableEnum(Type t)
        {
            if (Nullable.GetUnderlyingType(t) != null)
                if (Nullable.GetUnderlyingType(t).IsEnum)
                    return true;
            return false;
        }

        public void Dispose()
        {
            if (reader != null)
                if (!reader.IsClosed) reader.Close();
            command?.Dispose();
            if (connection == null) return;
            if (connection.State != ConnectionState.Closed) connection.Close();
            connection.Dispose();
        }

    }
}
