using System;
using System.Collections.Generic;
using System.Text;

namespace MicroORM.Interface
{
    public interface IQuery
    {
        string Delete<M>(string id) where M : class, new();
        string Update<M>(string id, string[] colms = null) where M : class, new();
        string Insert<M>() where M : class, new();
        string GetAll<M>(params string[] column) where M : class, new();     
        string GetByColumName<M>(string columName, params string[] selectColumn) where M : class, new();
        string Condition<M>(string condition, params string[] selectColumn) where M : class, new();
        string GetAllLeftJoin<M, J>() where M : class, new() where J : class, new();      
        string GetByColumNameLeftJoin<M, J>(string columName) where M : class, new() where J : class, new(); 
        string ConditionLeftJoin<M, J>(string condition) where M : class, new() where J : class, new();
        
        //string getFromTo(int from, int to); 
        //string getFromToWithSrc(int from, int to, string srcClm);
        //string RowCount();
        //string RowCountWithSrc(string srcClm);
    }
}
