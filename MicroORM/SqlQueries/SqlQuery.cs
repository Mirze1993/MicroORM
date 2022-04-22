using MicroORM.Attributes;
using MicroORM.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MicroORM.SqlQueries
{
    public class SqlQuery : IQuery
    {
        public string Delete<M>(string id) where M : class, new()
        {
            return $"DELETE FROM {typeof(M).Name} WHERE Id ={id} ";
        }
        public string Update<M>(string id, string[] colms = null) where M : class, new()
        {
            string columns = "";
            if (colms?.Length > 0)
                foreach (var item in colms)
                {
                    if (item == "Id") continue;
                    columns += $"{item}=@{item} ,";
                }
            else
                foreach (var item in typeof(M).GetProperties())
                {
                    if (item.PropertyType.GetInterfaces().Contains(typeof(IEnumerable<>)))
                        continue;
                    var t = (DbMapingAttribute)Attribute.GetCustomAttribute(item, typeof(DbMapingAttribute));  
                    if (t != null) if (t.Map != DbMap.Maping) continue;
                    if (item.Name == "Id") continue;
                    columns += $"{item.Name}=@{item.Name} ,";
                }
            if (!String.IsNullOrEmpty(columns))
            {
                columns = columns.Remove(columns.Length - 1);
                return $"UPDATE  {typeof(M).Name} set {columns} Where Id={id}";
            }
            return "";
        }
        public string Insert<M>() where M : class, new()
        {
            string columns = "";
            string values = "";
            foreach (var item in typeof(M).GetProperties())
            {
                if (item.PropertyType.GetInterfaces().Contains(typeof(IEnumerable<>)))
                    continue;

                var t = (DbMapingAttribute)Attribute.GetCustomAttribute(item, typeof(DbMapingAttribute));
                if (t != null) if (t.Map != DbMap.Maping) continue;

                if (item.Name == "Id") continue;
                columns += $"{item.Name} ,";
                values += $"@{item.Name} ,";
            }
            //last "," remove
            if (!String.IsNullOrEmpty(columns) && !String.IsNullOrEmpty(values))
            {
                columns = columns.Remove(columns.Length - 1);
                values = values.Remove(values.Length - 1);
                if (columns.Split(",").Length != values.Split(",").Length) return "";
                return $"INSERT INTO {typeof(M).Name} ({columns}) VALUES({values}) " +
                    $" ;SELECT CAST(scope_identity() AS int)";
            }
            return "";
        }
        public string GetAll<M>(params string[] column) where M : class, new()
        {
            string query;
            if (column?.Length > 0)
            {
                string clm =string.Join(" , ", column);
              
                query = $"SELECT {clm}  FROM {typeof(M).Name}";
            }
            else query = $"SELECT *  FROM {typeof(M).Name}";
            return query;
        }    
        public string GetByColumName<M>(string columName,params string[] selectColumn) where M : class, new()
        {            
            var q=GetAll<M>(selectColumn);

            q += $" WHERE {columName} =@{columName}";
            return q;
        }

        public string GetByColumName<M>(string o,string[] columNames, params string[] selectColumn) where M : class, new()
        {
            var s=columNames.Select(c => $"{c} =@{c}")?.Aggregate((a,b)=>$"{a} {o} {b}");           

            var q = GetAll<M>(selectColumn);

            q += $" WHERE {s}";
            return q;
        }

        public string Condition<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            var query = GetAll<M>(selectColumn);
            query += $" WHERE {condition}";
            return query;
        }


        public string GetAllLeftJoin<M,J>() where M : class, new() where J : class, new()
        {
            var q=GetAll<M>();
            q += $" m left join {typeof(J).Name} j on j.{typeof(M).Name}Id=m.Id";
            return q;
        }

       

        public string GetByColumNameLeftJoin<M, J>(string columName) where M : class, new() where J : class, new()
        {
            var q = GetAllLeftJoin<M, J>();
            q += $" WHERE m.{columName} =@{columName}";
            return q;
        }


        public string ConditionLeftJoin<M, J>(string condition) where M : class, new() where J : class, new()
        {
            var q = GetAllLeftJoin<M, J>();
            q += $" WHERE {condition}";
            return q;
        }

        #region commet
        //public string getFromTo(int from, int to)
        //{
        //    return "select * from " +
        //           "  (select Row_Number() over" +
        //           $"  (order by Id) as RowIndex, * from {GetTypeT.Name}) as Sub" +
        //           $"  Where Sub.RowIndex >= {from} and Sub.RowIndex <= {to}";

        //}        
        //public string getFromToWithSrc(int from, int to, string srcClm)
        //{
        //    return "select * from " +
        //           "  (select Row_Number() over" +
        //           $" (order by Id) as RowIndex, * from {GetTypeT.Name} " +
        //           $" Where {srcClm} like '@{srcClm}%') as Sub" +
        //           $" Where Sub.RowIndex >= {from} and Sub.RowIndex <= {to}";

        //}
        //public string RowCount()
        //{
        //    return $"Select count (*) from {GetTypeT.Name}";
        //}
        //public string RowCountWithSrc(string srcClm)
        //{
        //    return $"Select count (*) from {GetTypeT.Name} u " +
        //        $"Where u.{srcClm} like '@{srcClm}'+'%'";

        //}
        #endregion
    }
}
