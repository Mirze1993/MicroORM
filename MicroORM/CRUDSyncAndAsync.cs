using MicroORM.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace MicroORM
{
    public abstract class CRUDSyncAndAsync<T>: ICRUDSyncAndAsync<T> where T : class, new()
    {
        private CRUD<T> syncMetod;

        private CRUD<T> SyncMetod()
        {
            return syncMetod??new Csync<T>();         
        }

        private CRUDAsync<T> asyncMetod;

        private CRUDAsync<T> AsyncMetod()
        {
             return asyncMetod ?? new Casync<T>();           
        }

        public Result<int> Insert(T t, DbTransaction transaction = null)
        {
           return SyncMetod().Insert(t,transaction);
        }

        public Result<int> Insert(Action<T> item, DbTransaction transaction = null)
        {
            return SyncMetod().Insert(item,transaction);
        }

        public Result<int> Insert<M>(M t, DbTransaction transaction = null) where M : class, new()
        {
            return SyncMetod().Insert(t,transaction);
        }

        public Result Delet(int id, DbTransaction transaction = null)
        {
            return SyncMetod().Delet(id,transaction);
        }

        public Result Delet<M>(int id, DbTransaction transaction = null) where M : class, new()
        {
            return SyncMetod().Delet<M>(id,transaction);
        }

        public Result<List<T>> GetByColumName(string columName, object value, params string[] selectColumn)
        {
            return SyncMetod().GetByColumName(columName,value,selectColumn);
        }

        public Result<List<T>> GetByColumNameLeftJoin<Join>(string columName, object value) where Join : class, new()
        {
            return SyncMetod().GetByColumNameLeftJoin<Join>(columName,value);
        }

        public Result<List<M>> GetByColumName<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            return SyncMetod().GetByColumName<M>(columName,value,selectColumn);
        }

        public Result<List<M>> GetByColumNameLeftJoin<M, Join>(string columName, object value)
            where M : class, new()
            where Join : class, new()
        {
            return SyncMetod().GetByColumNameLeftJoin<M,Join>(columName,value);
        }

        public Result<T> GetByColumNameFist(string columName, object value, params string[] selectColumn)
        {
            return SyncMetod().GetByColumNameFist(columName,value,selectColumn);
        }

        public Result<T> GetByColumNameFistLeftJoin<Join>(string columName, object value) where Join : class, new()
        {
            return SyncMetod().GetByColumNameFistLeftJoin<Join>(columName,value);
        }

        public Result<M> GetByColumNameFist<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            return SyncMetod().GetByColumNameFist<M>(columName,value,selectColumn);
        }

        public Result<M> GetByColumNameFistLeftJoin<M, Join>(string columName, object value)
            where M : class, new()
            where Join : class, new()
        {
            return SyncMetod().GetByColumNameFistLeftJoin<M, Join>(columName, value);
        }

        public Result<List<T>> GetWithCondition(string condition, params string[] selectColumn)
        {
            return SyncMetod().GetWithCondition(condition,selectColumn);   
        }

        public Result<List<T>> GetWithConditionLeftJoin<Join>(string condition) where Join : class, new()
        {
            return SyncMetod().GetWithConditionLeftJoin<Join>(condition);
        }

        public Result<List<M>> GetWithCondition<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            return SyncMetod().GetWithCondition<M>(condition,selectColumn);
        }

        public Result<List<M>> GetWithConditionLeftJoin<M, Join>(string condition)
            where M : class, new()
            where Join : class, new()
        {
            return SyncMetod().GetWithConditionLeftJoin<M, Join>(condition);
        }

        public Result<T> GetWithConditionFist(string condition, params string[] selectColumn)
        {
            return SyncMetod().GetWithConditionFist(condition,selectColumn);
        }

        public Result<T> GetWithConditionFistLeftJoin<Join>(string condition) where Join : class, new()
        {
            return SyncMetod().GetWithConditionFistLeftJoin<Join>(condition);
        }

        public Result<M> GetWithConditionFist<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            return SyncMetod().GetWithConditionFist<M>(condition,selectColumn);
        }

        public Result<M> GetWithConditionFistLeftJoin<M, Join>(string condition)
            where M : class, new()
            where Join : class, new()
        {
            return SyncMetod().GetWithConditionFistLeftJoin<M, Join>(condition);
        }

        public Result<List<T>> GetAll(params string[] column)
        {
            return SyncMetod().GetAll(column);
        }

        public Result<List<T>> GetAllLeftJoin<Join>(params string[] column) where Join : class, new()
        {
            return SyncMetod().GetAllLeftJoin<Join>(column);
        }

        public Result<List<M>> GetAll<M>(params string[] column) where M : class, new()
        {
            return SyncMetod().GetAll<M>(column);
        }

        public Result<List<M>> GetAllLeftJoin<M, Join>()
            where M : class, new()
            where Join : class, new()
        {
            return SyncMetod().GetAllLeftJoin<M, Join>();
        }

        public Result Update(T t, int id, DbTransaction transaction = null)
        {
            return SyncMetod().Update(t, id, transaction);
        }

        public Result Update<M>(M t, int id, DbTransaction transaction = null) where M : class, new()
        {
            return SyncMetod().Update<M>(t, id, transaction);   
        }

        public Result Update(Action<Dictionary<string, object>> items, int id, DbTransaction transaction = null)
        {
            return SyncMetod().Update(items, id, transaction);
        }

        public Result Update(string[] columns, object[] values, int id, DbTransaction transaction = null)
        {
            return SyncMetod().Update(columns, values, id, transaction);
        }

        public Result Update<M>(string[] columns, object[] values, int id, DbTransaction transaction = null) where M : class, new()
        {
            return SyncMetod().Update<M>(columns, values, id, transaction);
        }

        public Task<Result<int>> InsertAsync(T t, DbTransaction transaction = null)
        {
            return AsyncMetod().InsertAsync(t, transaction);
        }

        public Task<Result<int>> InsertAsync(Action<T> item, DbTransaction transaction = null)
        {
            return AsyncMetod().InsertAsync(item, transaction);
        }

        public Task<Result<int>> InsertAsync<M>(M t, DbTransaction transaction = null) where M : class, new()
        {
            return AsyncMetod().InsertAsync<M>(t, transaction);
        }

        public Task<Result> DeletAsync(int id, DbTransaction transaction = null)
        {
            return AsyncMetod().DeletAsync(id, transaction);
        }

        public Task<Result> DeletAsync<M>(int id, DbTransaction transaction = null) where M : class, new()
        {
            return AsyncMetod().DeletAsync<M>(id, transaction);
        }

        public Task<Result<List<T>>> GetByColumNameAsync(string columName, object value, params string[] selectColumn)
        {
            return AsyncMetod().GetByColumNameAsync(columName, value, selectColumn);
        }

        public Task<Result<List<T>>> GetByColumNameLeftJoinAsync<Join>(string columName, object value) where Join : class, new()
        {
            return AsyncMetod().GetByColumNameLeftJoinAsync<Join>(columName, value);    
        }

        public Task<Result<List<M>>> GetByColumNameAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            return AsyncMetod().GetByColumNameAsync<M>(columName, value, selectColumn);  
        }

        public Task<Result<List<M>>> GetByColumNameLeftJoinAsync<M, Join>(string columName, object value)
            where M : class, new()
            where Join : class, new()
        {
            return AsyncMetod().GetByColumNameLeftJoinAsync<M, Join>(columName, value);
        }

        public Task<Result<T>> GetByColumNameFistAsync(string columName, object value, params string[] selectColumn)
        {
            throw new NotImplementedException();
        }

        public Task<Result<T>> GetByColumNameFistLeftJoinAsync<Join>(string columName, object value) where Join : class, new()
        {
            return AsyncMetod().GetByColumNameFistLeftJoinAsync<Join>(columName, value);
        }

        public Task<Result<M>> GetByColumNameFistAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            return AsyncMetod().GetByColumNameFistAsync<M>(columName, value, selectColumn);
        }

        public Task<Result<M>> GetByColumNameFistLeftJoinAsync<M, Join>(string columName, object value)
            where M : class, new()
            where Join : class, new()
        {
            return AsyncMetod().GetByColumNameFistLeftJoinAsync<M, Join>(columName, value);
        }

        public Task<Result<List<T>>> GetWithConditionAsync(string condition, params string[] selectColumn)
        {
            return AsyncMetod().GetWithConditionAsync(condition, selectColumn);
        }

        public Task<Result<List<T>>> GetWithConditionLeftJoinAsync<Join>(string condition) where Join : class, new()
        {
            return AsyncMetod().GetWithConditionLeftJoinAsync<Join>(condition);
        }

        public Task<Result<List<M>>> GetWithConditionAsync<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            return AsyncMetod().GetWithConditionAsync<M>(condition, selectColumn);
        }

        public Task<Result<List<M>>> GetWithConditionLeftJoinAsync<M, Join>(string condition)
            where M : class, new()
            where Join : class, new()
        {
            return AsyncMetod().GetWithConditionLeftJoinAsync<M, Join>(condition);
        }

        public Task<Result<T>> GetWithConditionFistAsync(string condition, params string[] selectColumn)
        {
            return AsyncMetod().GetWithConditionFistAsync(condition, selectColumn);
        }

        public Task<Result<T>> GetWithConditionFistLeftJoinAsync<Join>(string condition) where Join : class, new()
        {
            return AsyncMetod().GetWithConditionFistLeftJoinAsync<Join>(condition);
        }

        public Task<Result<M>> GetWithConditionFistAsync<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            return AsyncMetod().GetWithConditionFistAsync<M>(condition, selectColumn);
        }

        public Task<Result<M>> GetWithConditionFistLeftJoinAsync<M, Join>(string condition)
            where M : class, new()
            where Join : class, new()
        {
            return AsyncMetod().GetWithConditionFistLeftJoinAsync<M, Join>(condition);
        }

        public Task<Result<List<T>>> GetAllAsync(params string[] column)
        {
            return AsyncMetod().GetAllAsync(column);
        }

        public Task<Result<List<T>>> GetAllLeftJoinAsync<Join>(params string[] column) where Join : class, new()
        {
            return AsyncMetod().GetAllLeftJoinAsync<Join>(column);
        }

        public Task<Result<List<M>>> GetAllAsync<M>(params string[] column) where M : class, new()
        {
            return AsyncMetod().GetAllAsync<M>(column);
        }

        public Task<Result<List<M>>> GetAllLeftJoinAsync<M, Join>()
            where M : class, new()
            where Join : class, new()
        {
            return AsyncMetod().GetAllLeftJoinAsync<M, Join>();
        }

        public Task<Result> UpdateAsync(T t, int id, DbTransaction transaction = null)
        {
            return AsyncMetod().UpdateAsync(t, id, transaction);
        }

        public Task<Result> UpdateAsync<M>(M t, int id, DbTransaction transaction = null) where M : class, new()
        {
            return AsyncMetod().UpdateAsync<M>(t, id, transaction);
        }

        public Task<Result> UpdateAsync(Action<Dictionary<string, object>> items, int id, DbTransaction transaction = null)
        {
            return AsyncMetod().UpdateAsync(items, id, transaction);
        }

        public Task<Result> UpdateAsync(string[] columns, object[] values, int id, DbTransaction transaction = null)
        {
            return AsyncMetod().UpdateAsync(columns, values, id, transaction);
        }

        public Task<Result> UpdateAsync<M>(string[] columns, object[] values, int id, DbTransaction transaction = null) where M : class, new()
        {
            return AsyncMetod().UpdateAsync<M>(columns, values, id, transaction);
        }
    }
}
