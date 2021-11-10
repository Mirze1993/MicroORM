using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace MicroORM.Interface
{
    public interface ICRUDAsync<T> where T : class, new()
    {
        Task<Result<int>> InsertAsync(T t, DbTransaction transaction = null);
        Task<Result<int>> InsertAsync(Action<T> item, DbTransaction transaction = null);
        Task<Result<int>> InsertAsync<M>(M t, DbTransaction transaction = null) where M : class, new();


        Task<Result> DeletAsync(int id);
        Task<Result> DeletAsync<M>(int id) where M : class, new();

        Task<Result<List<T>>> GetByColumNameAsync(string columName, object value, params string[] selectColumn);
        Task<Result<List<T>>> GetByColumNameLeftJoinAsync<Join>(string columName, object value) where Join : class, new();
        Task<Result<List<M>>> GetByColumNameAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new();
        Task<Result<List<M>>> GetByColumNameLeftJoinAsync<M, Join>(string columName, object value) where M : class, new() where Join : class, new();

        Task<Result<T>> GetByColumNameFistAsync(string columName, object value, params string[] selectColumn);
        Task<Result<T>> GetByColumNameFistLeftJoinAsync<Join>(string columName, object value) where Join : class, new();
        Task<Result<M>> GetByColumNameFistAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new();
        Task<Result<M>> GetByColumNameFistLeftJoinAsync<M, Join>(string columName, object value) where M : class, new() where Join : class, new();

        Task<Result<List<T>>> GetWithConditionAsync(string condition, params string[] selectColumn);
        Task<Result<List<T>>> GetWithConditionLeftJoinAsync<Join>(string condition) where Join : class, new();
        Task<Result<List<M>>> GetWithConditionAsync<M>(string condition, params string[] selectColumn) where M : class, new();
        Task<Result<List<M>>> GetWithConditionLeftJoinAsync<M, Join>(string condition) where M : class, new() where Join : class, new();

        Task<Result<T>> GetWithConditionFistAsync(string condition, params string[] selectColumn);
        Task<Result<T>> GetWithConditionFistLeftJoinAsync<Join>(string condition) where Join : class, new();
        Task<Result<M>> GetWithConditionFistAsync<M>(string condition, params string[] selectColumn) where M : class, new();
        Task<Result<M>> GetWithConditionFistLeftJoinAsync<M, Join>(string condition) where M : class, new() where Join : class, new();

        Task<Result<List<T>>> GetAllAsync(params string[] column);
        Task<Result<List<T>>> GetAllLeftJoinAsync<Join>(params string[] column) where Join : class, new();
        Task<Result<List<M>>> GetAllAsync<M>(params string[] column) where M : class, new();
        Task<Result<List<M>>> GetAllLeftJoinAsync<M, Join>() where M : class, new() where Join : class, new();

        Task<Result> UpdateAsync(T t, int id);
        Task<Result> UpdateAsync<M>(M t, int id) where M : class, new();
        Task<Result> UpdateAsync(Action<Dictionary<string, object>> items, int id);
        Task<Result> UpdateAsync(string[] columns, object[] values, int id);
        Task<Result> UpdateAsync<M>(string[] columns, object[] values, int id) where M : class, new();
    }
}
