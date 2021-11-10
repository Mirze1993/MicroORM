using MicroORM.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;


namespace MicroORM
{
    public abstract class CRUD<T> : ICRUD<T> where T : class, new()
    {
        IQuery query;
        public DBContext DBContext { get; set; }

        public CRUD()
        {
            this.DBContext = new DBContext();
            query = DBContext.CreateQuary();
        }


        public virtual Result<int> Insert(T t, DbTransaction transaction = null)
        {           
            return Insert<T>(t, transaction);
        }
        public virtual Result<int> Insert(Action<T> item, DbTransaction transaction = null)
        {
            if (item == null) throw new ArgumentNullException();
            T t = new T();
            item(t);
            return Insert<T>(t, transaction);
        }
        public virtual Result<int> Insert<M>(M t, DbTransaction transaction = null) where M : class, new()
        {
            if (t == null) throw new ArgumentNullException();
            string cmtext = query.Insert<M>();
            using (CommanderBase commander = DBContext.CreateCommander())
            {

                var p = commander.SetParametrs(t);
                var result = commander.Scaller(cmtext, parameters: p, transaction: transaction);

                if (result.Success && result.Value != null) 
                    return new Result<int>(). SuccessResult(Convert.ToInt32(result.Value));
                else return new Result<int>() {Message=result.Message,Success=false };
            }
        }


        public virtual Result Delet(int id)
        {
            return Delet<T>(id);
        }
        public virtual Result Delet<M>(int id) where M : class, new()
        {
            string cmtext = query.Delete<M>(id.ToString());
            using CommanderBase commander = DBContext.CreateCommander();
            return commander.NonQuery(cmtext);
        }


        public virtual Result<List<T>> GetByColumName(string columName, object value, params string[] selectColumn)
        {
            return GetByColumName<T>(columName, value, selectColumn);
        }
        public virtual Result<List<T>> GetByColumNameLeftJoin<Join>(string columName, object value) where Join : class, new()
        {
            return GetByColumNameLeftJoin<T, Join>(columName,value);
        }
        public virtual Result<List<M>> GetByColumName<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.GetByColumName<M>(columName, selectColumn);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.Reader<M>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }

        }
        public virtual Result<List<M>> GetByColumNameLeftJoin<M, Join>(string columName, object value) where M : class, new() where Join : class, new()
        {
            string cmtext = query.GetByColumNameLeftJoin<M, Join>(columName);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.ReaderLeftJoin<M, Join>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }

        }
        

        public virtual Result<T> GetByColumNameFist(string columName, object value, params string[] selectColumn)
        {
            return GetByColumNameFist<T>(columName, value, selectColumn);
        }
        public virtual Result<T> GetByColumNameFistLeftJoin<Join>(string columName, object value) where Join : class, new()
        {
            return GetByColumNameFistLeftJoin<T, Join>(columName, value);
        }
        public virtual Result<M> GetByColumNameFist<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.GetByColumName<M>(columName, selectColumn);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.ReaderFist<M>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }
        }
        public virtual Result<M> GetByColumNameFistLeftJoin<M, Join>(string columName, object value) where M : class, new() where Join : class, new()
        {
            string cmtext = query.GetByColumNameLeftJoin<M, Join>(columName);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.ReaderLeftJoinFist<M, Join>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }

        }
        

        public virtual Result<List<T>> GetWithCondition(string condition, params string[] selectColumn)
        {
            return GetWithCondition<T>(condition, selectColumn);
        }
        public virtual Result<List<T>> GetWithConditionLeftJoin<Join>(string condition) where Join : class, new()
        {
            return GetWithConditionLeftJoin<T,Join>(condition);
        }
        public virtual Result<List<M>> GetWithCondition<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.Condition<M>(condition, selectColumn);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.Reader<M>(cmtext);
            }

        }
        public virtual Result<List<M>> GetWithConditionLeftJoin<M,Join>(string condition) where M : class, new() where Join : class, new()
        {
            string cmtext = query.ConditionLeftJoin<M,Join>(condition);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.ReaderLeftJoin<M,Join>(cmtext);
            }

        }

        public virtual Result<T> GetWithConditionFist(string condition, params string[] selectColumn)
        {
            return GetWithConditionFist<T>(condition, selectColumn);
        }
        public virtual Result<T> GetWithConditionFistLeftJoin<Join>(string condition) where Join : class, new()
        {
            return GetWithConditionFistLeftJoin<T, Join>(condition);
        }
        public virtual Result<M> GetWithConditionFist<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.Condition<M>(condition, selectColumn);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.ReaderFist<M>(cmtext);
            }

        }
        public virtual Result<M> GetWithConditionFistLeftJoin<M,Join>(string condition) where M : class, new() where Join : class, new()
        {
            string cmtext = query.ConditionLeftJoin<M,Join>(condition);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.ReaderLeftJoinFist<M,Join>(cmtext);
            }

        }


        public virtual Result<List<T>> GetAll(params string[] column)
        {
            return GetAll<T>(column);
        }
        public virtual Result<List<T>> GetAllLeftJoin<Join>(params string[] column) where Join : class, new()
        {
            return GetAllLeftJoin<T,Join>();
        }
        public virtual Result<List<M>> GetAll<M>(params string[] column) where M : class, new()
        {
            string cmtext = query.GetAll<M>(column);
            using (CommanderBase commander = DBContext.CreateCommander())
                return commander.Reader<M>(cmtext);
        }
        public virtual Result<List<M>> GetAllLeftJoin<M,Join>() where M : class, new() where Join : class, new()
        {
            string cmtext = query.GetAllLeftJoin<M,Join>();
            using (CommanderBase commander = DBContext.CreateCommander())
                return commander.ReaderLeftJoin<M,Join>(cmtext);
        }


        public virtual Result Update(T t, int id)
        {
            return Update<T>(t, id);
        }
        public virtual Result Update<M>(M t, int id) where M : class, new()
        {
            string cmtext = query.Update<M>(id.ToString());
            using (CommanderBase commander = DBContext.CreateCommander())
                return commander.NonQuery(cmtext, commander.SetParametrs(t));
        }
        public virtual Result Update(Action<Dictionary<string,object>>items,int id)
        {
            if(items == null) throw new ArgumentNullException();
            var d=new Dictionary<string, object>();
            items(d);
            string[] columns = new string[d.Count];
            object[] values = new object[d.Count];
            d.Keys.CopyTo(columns, 0);d.Values.CopyTo(values, 0);
            return Update(columns, values, id);
        }
        public virtual Result Update(string[] columns, object[] values, int id)
        {
            return Update<T>(columns, values, id);
        }
        public virtual Result Update<M>(string[] columns, object[] values, int id) where M : class, new()
        {
            string cmtext = query.Update<M>(id.ToString(), columns);
            var p = new List<DbParameter>();
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    p.Add(commander.SetParametr(columns[i], values[i]));
                }
                return commander.NonQuery(cmtext, p);
            }
        }

        #region elave
        //public virtual (int,bool) RowCount()
        //{
        //    string cmtext = query.RowCount();
        //    using (CommanderBase commander = DBContext.CreateCommander())
        //    {
        //        var (o,b) = commander.Scaller(cmtext);
        //        if (b && o != null) return (Convert.ToInt32(o), b);
        //        else return (0, b);
        //    }
        //}

        //public virtual (int, bool) RowCountWithSrc(string srcClm, string srcValue)
        //{
        //    string cmtext = query.RowCountWithSrc(srcClm);
        //    using (CommanderBase commander = DBContext.CreateCommander())
        //    {
        //        var (o,b) = commander.Scaller(cmtext, new List<DbParameter>() { commander.SetParametr(srcClm, srcValue) });

        //        if (b && o != null) return (Convert.ToInt32(o), b);
        //        else return (0, b);
        //    }
        //}

        //public virtual (List<T>,bool) GetFromTo(int from, int to)
        //{
        //    string cmtext = query.getFromTo(from, to);
        //    using (CommanderBase commander = DBContext.CreateCommander())
        //        return commander.Reader<T>(cmtext);
        //}

        //public virtual (List<T>, bool) GetFromToWithSrc(int from, int to, string srcClm, string srcValue)
        //{
        //    string cmtext = query.getFromToWithSrc(from, to, srcClm);
        //    using (CommanderBase commander = DBContext.CreateCommander())
        //        return commander.Reader<T>(cmtext, new List<DbParameter> { commander.SetParametr(srcClm, srcValue) });
        //}

        #endregion

    }
}
