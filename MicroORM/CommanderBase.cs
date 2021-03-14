
using CommonTool;
using MicroORM.Logging;
using System;
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

        public LogWriteFile logManager;

        public CommanderBase()
        {
            logManager = new LogWriteFile();
        }

        public abstract List<DbParameter> SetParametrs<T>(T t);


        public abstract DbParameter SetParametr(string paramName, object value);


        public abstract DbParameter SetOutputParametr();

        protected void ConnectionOpen()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
        }



        public DbTransaction TransactionStart()
        {
            return connection.BeginTransaction(IsolationLevel.Serializable);
        }

        protected void CommandStart(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {
            command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            if (parameters != null) command.Parameters.AddRange(parameters.ToArray());
            if (transaction != null) command.Transaction = transaction;
        }



        public bool NonQuery(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {

            CommandStart(commandText, parameters, commandType, transaction);
            ConnectionOpen();
            bool b = false;
            try
            {
                b = command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                logManager.WriteFile(e.Message, LogLevel.Error);
            }
            return b;
        }




        public (object, bool) Scaller(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {

            CommandStart(commandText, parameters, commandType, transaction);
            ConnectionOpen();
            object b = null;
            try
            {
                b = command.ExecuteScalar();
                return (b, true);
            }
            catch (Exception e)
            {
                logManager.WriteFile(e.Message, LogLevel.Error);
                return (0, false);
            }
        }



        //reader
        public (T, bool) Reader<T>(Func<DbDataReader, T> readMetod, string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)
        {

            CommandStart(commandText, parameters, commandType, transaction);
            ConnectionOpen();
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                logManager.WriteFile(e.Message, LogLevel.Error);
                return (default(T), false);
            }
            var t = readMetod(reader);
            return (t, true);
        }


        public (List<T>, bool) Reader<T>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new()
        {
            return Reader(GetList<T>, commandText, parameters, commandType, transaction);
        }

        public (T, bool) ReaderFist<T>(string commandText, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null) where T : class, new()
        {
            return Reader(GetFist<T>, commandText, parameters, commandType, transaction);
        }


        protected List<T> GetList<T>(DbDataReader r) where T : class, new()
        {
            List<T> list = new List<T>();
            if (r == null) return list;
            var fieldNames = Enumerable.Range(0, r.FieldCount).Select(i => r.GetName(i)).ToArray();
            while (r.Read())
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
            if (!r.IsClosed) r.Close();
            return list;
        }
        bool isNullableEnum(Type t)
        {
            if (Nullable.GetUnderlyingType(t) != null)
                if (Nullable.GetUnderlyingType(t).IsEnum)
                    return true;
            return false;
        }
        protected T GetFist<T>(DbDataReader r) where T : class, new()
        {
            if (r == null) return null;
            if (!r.HasRows) return null;
            T t = new T();
            while (r.Read())
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
            if (!r.IsClosed) r.Close();
            return t;
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
