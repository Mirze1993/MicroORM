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


        public virtual async Task<Result<int>> InsertAsync(T t, DbTransaction transaction = null)
        {
            return await InsertAsync<T>(t, transaction);
        }
        public virtual async Task<Result<int>> InsertAsync(Action<T> item, DbTransaction transaction = null)
        {
            if (item == null) throw new ArgumentNullException();
            T t = new T();
            item(t);
            return await InsertAsync<T>(t, transaction);
        }
        public virtual async Task< Result<int>> InsertAsync<M>(M t, DbTransaction transaction = null) where M : class, new()
        {
            if (t == null) throw new ArgumentNullException();
            string cmtext = query.Insert<M>();
            await using (var commander = DBContext.CreateCommanderAsync())
            {

                var p = commander.SetParametrs(t);
                var result =await commander.ScallerAsync(cmtext, parameters: p, transaction: transaction);

                if (result.Success && result.Value != null)
                    return new Result<int>().SuccessResult(Convert.ToInt32(result.Value));
                else return new Result<int>() { Message = result.Message, Success = false };
            }
        }


        public virtual async Task<Result> DeletAsync(int id, DbTransaction transaction = null)
        {
            return await DeletAsync<T>(id);
        }
        public virtual async Task<Result> DeletAsync<M>(int id, DbTransaction transaction = null) where M : class, new()
        {
           string cmtext = query.Delete<M>(id.ToString());
           await using var commander = DBContext.CreateCommanderAsync();
            return await commander.NonQueryAsync(cmtext,transaction:transaction);
        }


        public virtual async Task<Result<List<T>>> GetByColumNameAsync(string columName, object value, params string[] selectColumn)
        {
            return await GetByColumNameAsync<T>(columName, value, selectColumn);
        }
        public virtual async Task<Result<List<T>>> GetByColumNameLeftJoinAsync<Join>(string columName, object value) where Join : class, new()
        {
            return await GetByColumNameLeftJoinAsync<T, Join>(columName, value);
        }
        public virtual async Task<Result<List<M>>> GetByColumNameAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.GetByColumName<M>(columName, selectColumn);
            await using (var commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderAsync<M>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }

        }
        public virtual async Task< Result<List<M>>> GetByColumNameLeftJoinAsync<M, Join>(string columName, object value) where M : class, new() where Join : class, new()
        {
            string cmtext = query.GetByColumNameLeftJoin<M, Join>(columName);
            await using (var commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderLeftJoinAsync<M, Join>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }

        }


        public virtual async Task<Result<T>> GetByColumNameFistAsync(string columName, object value, params string[] selectColumn)
        {
            return await GetByColumNameFistAsync<T>(columName, value, selectColumn);
        }
        public virtual async Task<Result<T>> GetByColumNameFistLeftJoinAsync<Join>(string columName, object value) where Join : class, new()
        {
            return await GetByColumNameFistLeftJoinAsync<T, Join>(columName, value);
        }
        public virtual async Task<Result<M>> GetByColumNameFistAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.GetByColumName<M>(columName, selectColumn);
            await using (var commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderFistAsync<M>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }
        }
        public virtual async Task<Result<M>> GetByColumNameFistLeftJoinAsync<M, Join>(string columName, object value) where M : class, new() where Join : class, new()
        {
            string cmtext = query.GetByColumNameLeftJoin<M, Join>(columName);
            await using (var commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderLeftJoinFistAsync<M, Join>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }

        }


        public virtual async Task<Result<List<T>>> GetWithConditionAsync(string condition, params string[] selectColumn)
        {
            return await GetWithConditionAsync<T>(condition, selectColumn);
        }
        public virtual async Task<Result<List<T>>> GetWithConditionLeftJoinAsync<Join>(string condition) where Join : class, new()
        {
            return await GetWithConditionLeftJoinAsync<T, Join>(condition);
        }
        public virtual async Task<Result<List<M>>> GetWithConditionAsync<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.Condition<M>(condition, selectColumn);
            await using(var commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderAsync<M>(cmtext);
            }

        }
        public virtual async Task<Result<List<M>>> GetWithConditionLeftJoinAsync<M, Join>(string condition) where M : class, new() where Join : class, new()
        {
            string cmtext = query.ConditionLeftJoin<M, Join>(condition);
            await using (var commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderLeftJoinAsync<M, Join>(cmtext);
            }

        }

        public virtual async Task<Result<T>> GetWithConditionFistAsync(string condition, params string[] selectColumn)
        {
            return await GetWithConditionFistAsync<T>(condition, selectColumn);
        }
        public virtual async Task<Result<T>> GetWithConditionFistLeftJoinAsync<Join>(string condition) where Join : class, new()
        {
            return await GetWithConditionFistLeftJoinAsync<T, Join>(condition);
        }
        public virtual async Task<Result<M>> GetWithConditionFistAsync<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.Condition<M>(condition, selectColumn);
            await using(var commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderFistAsync<M>(cmtext);
            }

        }
        public virtual async Task<Result<M>> GetWithConditionFistLeftJoinAsync<M, Join>(string condition) where M : class, new() where Join : class, new()
        {
            string cmtext = query.ConditionLeftJoin<M, Join>(condition);
            await using(var commander = DBContext.CreateCommanderAsync())
            {
                return await commander.ReaderLeftJoinFistAsync<M, Join>(cmtext);
            }

        }


        public virtual async Task<Result<List<T>>> GetAllAsync(params string[] column)
        {
            return await GetAllAsync<T>(column);
        }
        public virtual async Task<Result<List<T>>> GetAllLeftJoinAsync<Join>(params string[] column) where Join : class, new()
        {
            return await GetAllLeftJoinAsync<T, Join>();
        }
        public virtual async Task<Result<List<M>>> GetAllAsync<M>(params string[] column) where M : class, new()
        {
            string cmtext = query.GetAll<M>(column);
            await using (var commander = DBContext.CreateCommanderAsync())
                return await commander.ReaderAsync<M>(cmtext);
        }
        public virtual async Task<Result<List<M>>> GetAllLeftJoinAsync<M, Join>() where M : class, new() where Join : class, new()
        {
            string cmtext = query.GetAllLeftJoin<M, Join>();
            await using (var commander = DBContext.CreateCommanderAsync())
                return await commander.ReaderLeftJoinAsync<M, Join>(cmtext);
        }


        public virtual async Task<Result> UpdateAsync(T t, int id, DbTransaction transaction = null)
        {
            return await UpdateAsync<T>(t, id,transaction:transaction);
        }
        public virtual async Task<Result> UpdateAsync<M>(M t, int id, DbTransaction transaction = null) where M : class, new()
        {
            string cmtext = query.Update<M>(id.ToString());
            await using (var commander = DBContext.CreateCommanderAsync())
                return await commander.NonQueryAsync(cmtext, commander.SetParametrs(t), transaction: transaction);
        }
        public virtual async Task<Result> UpdateAsync(Action<Dictionary<string, object>> items, int id, DbTransaction transaction = null)
        {
            if (items == null) throw new ArgumentNullException();
            var d = new Dictionary<string, object>();
            items(d);
            string[] columns = new string[d.Count];
            object[] values = new object[d.Count];
            d.Keys.CopyTo(columns, 0); d.Values.CopyTo(values, 0);
            return await UpdateAsync(columns, values, id,transaction:transaction);
        }
        public virtual async Task<Result> UpdateAsync(string[] columns, object[] values, int id, DbTransaction transaction = null)
        {
            return await UpdateAsync<T>(columns, values, id,transaction:transaction);
        }
        public virtual async Task<Result> UpdateAsync<M>(string[] columns, object[] values, int id, DbTransaction transaction = null) where M : class, new()
        {
            string cmtext = query.Update<M>(id.ToString(), columns);
            var p = new List<DbParameter>();
            await using (var commander = DBContext.CreateCommanderAsync())
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    p.Add(commander.SetParametr(columns[i], values[i]));
                }
                return await commander.NonQueryAsync(cmtext, p,transaction:transaction);
            }
        }
    }
}
