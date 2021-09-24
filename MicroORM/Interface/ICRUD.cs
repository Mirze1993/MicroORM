using System;
using System.Collections.Generic;
using System.Data.Common;

namespace MicroORM.Interface
{
    public interface ICRUD<T> where T : class, new()
    {
        Result<int> Insert(T t, DbTransaction transaction = null);
        Result<int> Insert(Action<T> item, DbTransaction transaction = null);
        Result<int> Insert<M>(M t, DbTransaction transaction = null) where M : class, new();


        bool Delet(int id);
        bool Delet<M>(int id) where M : class, new();

        Result<List<T>> GetByColumName(string columName, object value, params string[] selectColumn);
        Result<List<T>> GetByColumNameLeftJoin<Join>(string columName, object value) where Join : class, new();
        Result<List<M>> GetByColumName<M>(string columName, object value, params string[] selectColumn) where M : class, new();
        Result<List<M>> GetByColumNameLeftJoin<M, Join>(string columName, object value) where M : class, new() where Join : class, new();

        Result<T> GetByColumNameFist(string columName, object value, params string[] selectColumn);
        Result<T> GetByColumNameFistLeftJoin<Join>(string columName, object value) where Join : class, new();
        Result<M> GetByColumNameFist<M>(string columName, object value, params string[] selectColumn) where M : class, new();
        Result<M> GetByColumNameFistLeftJoin<M, Join>(string columName, object value) where M : class, new() where Join : class, new();

        Result<List<T>> GetWithCondition(string condition, params string[] selectColumn);
        Result<List<T>> GetWithConditionLeftJoin<Join>(string condition) where Join : class, new();
        Result<List<M>> GetWithCondition<M>(string condition, params string[] selectColumn) where M : class, new();
        Result<List<M>> GetWithConditionLeftJoin<M, Join>(string condition) where M : class, new() where Join : class, new();

        Result<T> GetWithConditionFist(string condition, params string[] selectColumn);
        Result<T> GetWithConditionFistLeftJoin<Join>(string condition) where Join : class, new();
        Result<M> GetWithConditionFist<M>(string condition, params string[] selectColumn) where M : class, new();
        Result<M> GetWithConditionFistLeftJoin<M, Join>(string condition) where M : class, new() where Join : class, new();

        Result<List<T>> GetAll(params string[] column);
        Result<List<T>> GetAllLeftJoin<Join>(params string[] column) where Join : class, new();
        Result<List<M>> GetAll<M>(params string[] column) where M : class, new();
        Result<List<M>> GetAllLeftJoin<M, Join>() where M : class, new() where Join : class, new();

        bool Update(T t, int id);
        bool Update<M>(M t, int id) where M : class, new();
        bool Update(Action<Dictionary<string, object>> items, int id);
        bool Update(string[] columns, object[] values, int id);
        bool Update<M>(string[] columns, object[] values, int id) where M : class, new();



        //(int, bool) RowCount();
        //(int, bool) RowCountWithSrc(string srcClm, string srcValue);
        //(List<T>, bool) GetFromTo(int from, int to);
        //(List<T>, bool) GetFromToWithSrc(int from, int to, string srcClm, string srcValue);

    }
}
