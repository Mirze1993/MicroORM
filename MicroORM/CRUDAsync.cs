using MicroORM.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace MicroORM
{
    public abstract class CRUDAsync<T> : ICRUDAsync<T> where T : class, new()
    {
        IQuery query;
        public DBContext DBContext { get; set; }

        public CRUDAsync()
        {
            this.DBContext = new DBContext();
            query = DBContext.CreateQuary();
        }


        public virtual async Task<(int, bool)> InsertAsync(T t, DbTransaction transaction = null)
        {
            if (t == null) throw new ArgumentNullException();
            return await InsertAsync<T>(t, transaction);
        }
        public virtual async Task<(int, bool)> InsertAsync(Action<T> item, DbTransaction transaction = null)
        {
            if (item == null) throw new ArgumentNullException();
            T t = new T();
            item(t);
            return await InsertAsync<T>(t, transaction);
        }
        public virtual async Task<(int, bool)> InsertAsync<M>(M t, DbTransaction transaction = null) where M : class, new()
        {
            string cmtext = query.Insert<M>();

            await using (CommanderBaseAsync commander = DBContext.CreateCommanderAsync())
            {
                var p = commander.SetParametrs(t);
                var (id, b) = await commander.ScallerAsync(cmtext, parameters: p, transaction: transaction);

                if (b && id != null) return (Convert.ToInt32(id), b);
                else return (0, b);
            }
        }


        public virtual async Task<bool> DeletAsync(int id)
        {
            return await DeletAsync<T>(id);
        }
        public virtual async Task<bool> DeletAsync<M>(int id) where M : class, new()
        {
            string cmtext = query.Delete<M>(id.ToString());
            await using CommanderBaseAsync commander = DBContext.CreateCommanderAsync();
            return await commander.NonQueryAsync(cmtext);
        }


        public virtual async Task<(List<T>, bool)> GetByColumNameAsync(string columName, object value, params string[] selectColumn)
        {
            return await GetByColumNameAsync<T>(columName, value, selectColumn);
        }
        public virtual async Task<(List<M>, bool)> GetByColumNameAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.GetByColumName<M>(columName, selectColumn);
            await using (CommanderBaseAsync commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderAsync<M>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }

        }

        public virtual async Task<(T, bool)> GetByColumNameFistAsync(string columName, object value, params string[] selectColumn)
        {
            return await GetByColumNameFistAsync<T>(columName, value, selectColumn);
        }
        public virtual async Task<(M, bool)> GetByColumNameFistAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.GetByColumName<M>(columName, selectColumn);
            await using (CommanderBaseAsync commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderFistAsync<M>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }
        }


        public virtual async Task<(List<T>, bool)> GetWithConditionAsync(string condition, params string[] selectColumn)
        {
            return await GetWithConditionAsync<T>(condition, selectColumn);
        }
        public virtual async Task<(List<M>, bool)> GetWithConditionAsync<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.Condition<M>(condition, selectColumn);
            await using (CommanderBaseAsync commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderAsync<M>(cmtext);
            }

        }

        public virtual async Task<(T, bool)> GetWithConditionFistAsync(string condition, params string[] selectColumn)
        {
            return await GetWithConditionFistAsync<T>(condition, selectColumn);
        }
        public virtual async Task<(M, bool)> GetWithConditionFistAsync<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.Condition<M>(condition, selectColumn);
            await using (CommanderBaseAsync commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderFistAsync<M>(cmtext);
            }

        }

        public virtual async Task<(List<T>, bool)> GetAllAsync(params string[] column)
        {
            return await GetAllAsync<T>(column);
        }
        public virtual async Task<(List<M>, bool)> GetAllAsync<M>(params string[] column) where M : class, new()
        {
            string cmtext = query.GetAll<M>(column);
            await using (CommanderBaseAsync commander = DBContext.CreateCommanderAsync())
                return await commander.ReaderAsync<M>(cmtext);
        }


        public virtual async Task<bool> UpdateAsync(T t, int id)
        {
            return await UpdateAsync<T>(t, id);
        }
        public virtual async Task<bool> UpdateAsync<M>(M t, int id) where M : class, new()
        {
            string cmtext = query.Update<M>(id.ToString());
            await using (CommanderBaseAsync commander = DBContext.CreateCommanderAsync())
                return await commander.NonQueryAsync(cmtext, commander.SetParametrs(t));
        }
        public virtual async Task<bool> UpdateAsync(Action<Dictionary<string, object>> items, int id)
        {
            if (items == null) throw new ArgumentNullException();
            var d = new Dictionary<string, object>();
            items(d);
            string[] columns = new string[d.Count];
            object[] values = new object[d.Count];
            d.Keys.CopyTo(columns, 0); d.Values.CopyTo(values, 0);
            return await UpdateAsync(columns, values, id);
        }
        public virtual async Task<bool> UpdateAsync(string[] columns, object[] values, int id)
        {
            return await UpdateAsync<T>(columns, values, id);
        }
        public virtual async Task<bool> UpdateAsync<M>(string[] columns, object[] values, int id) where M : class, new()
        {
            string cmtext = query.Update<M>(id.ToString(), columns);
            var p = new List<DbParameter>();
            await using (CommanderBaseAsync commander = DBContext.CreateCommanderAsync())
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    p.Add(commander.SetParametr(columns[i], values[i]));
                }
                return await commander.NonQueryAsync(cmtext, p);
            }
        }
    }
}
