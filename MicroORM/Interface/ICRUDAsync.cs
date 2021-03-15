using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace MicroORM.Interface
{
    public interface ICRUDAsync<T> where T : class, new()
    {
        Task<(int, bool)> InsertAsync(T t, DbTransaction transaction = null);
        Task<(int, bool)> InsertAsync(Action<T> item, DbTransaction transaction = null);
        Task<(int, bool)> InsertAsync<M>(M t, DbTransaction transaction = null) where M : class, new();

        Task<bool> DeletAsync(int id);
        Task<bool> DeletAsync<M>(int id) where M : class, new();

        Task<(List<T>, bool)> GetByColumNameAsync(string columName, object value, params string[] selectColumn);
        Task<(List<M>, bool)> GetByColumNameAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new();

        Task<(T, bool)> GetByColumNameFistAsync(string columName, object value, params string[] selectColumn);
        Task<(M, bool)> GetByColumNameFistAsync<M>(string columName, object value, params string[] selectColumn) where M : class, new();

        Task<(List<T>, bool)> GetAllAsync(params string[] column);
        Task<(List<M>, bool)> GetAllAsync<M>(params string[] column) where M : class, new();

        Task<bool> UpdateAsync(T t, int id);
        Task<bool> UpdateAsync<M>(M t, int id) where M : class, new();
        Task<bool> UpdateAsync(Action<Dictionary<string, object>> items, int id);
        Task<bool> UpdateAsync(string[] columns, object[] values, int id);
        Task<bool> UpdateAsync<M>(string[] columns, object[] values, int id) where M : class, new();

        Task<(List<T>, bool)> GetWithConditionAsync(string condition, params string[] selectColumn);
        Task<(List<M>, bool)> GetWithConditionAsync<M>(string condition, params string[] selectColumn) where M : class, new();

       Task<(T, bool)> GetWithConditionFistAsync(string condition, params string[] selectColumn);
        Task<(M, bool)> GetWithConditionFistAsync<M>(string condition, params string[] selectColumn) where M : class, new();
    }
}
