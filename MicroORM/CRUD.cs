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

        


        public virtual (int, bool) Insert(T t, DbTransaction transaction = null)
        {
           return Insert<T>(t, transaction);
        }
        public virtual (int, bool) Insert<M>(M t, DbTransaction transaction = null) where M : class, new()
        {
            string cmtext = query.Insert<M>();

            using (CommanderBase commander = DBContext.CreateCommander())
            {

                var p = commander.SetParametrs(t);
                var (id, b) = commander.Scaller(cmtext, parameters: p, transaction: transaction);

                if (b && id != null) return (Convert.ToInt32(id), b);
                else return (0, b);
            }
        }


        public virtual bool Delet(int id)
        {
            return Delet<T>(id);
        }
        public virtual bool Delet<M>(int id) where M : class, new()
        {
            string cmtext = query.Delete<M>(id.ToString());
            using CommanderBase commander = DBContext.CreateCommander();
            return commander.NonQuery(cmtext);
        }

        public virtual (List<T>, bool) GetByColumName(string columName, object value, params string[] selectColumn)
        {
            return GetByColumName<T>(columName, value, selectColumn);
        }
        public virtual (List<M>, bool) GetByColumName<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.GetByColumName<M>(columName, selectColumn);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.Reader<M>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }

        }

        public virtual (T, bool) GetByColumNameFist(string columName, object value, params string[] selectColumn)
        {
            return GetByColumNameFist<T>(columName, value,selectColumn);
        }
        public virtual (M, bool) GetByColumNameFist<M>(string columName, object value, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.GetByColumName<M>(columName, selectColumn);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.ReaderFist<M>(cmtext, new List<DbParameter>() { commander.SetParametr(columName, value) });
            }
        }


        public virtual (List<T>, bool) GetWithCondition(string condition, params string[] selectColumn)
        {
            return GetWithCondition<T>(condition, selectColumn);
        }
        public virtual (List<M>, bool) GetWithCondition<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.Condition<M>(condition, selectColumn);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.Reader<M>(cmtext);
            }

        }

        public virtual (T, bool) GetWithConditionFist(string condition, params string[] selectColumn)
        {
            return GetWithConditionFist<T>(condition, selectColumn);
        }
        public virtual (M, bool) GetWithConditionFist<M>(string condition, params string[] selectColumn) where M : class, new()
        {
            string cmtext = query.Condition<M>(condition, selectColumn);
            using (CommanderBase commander = DBContext.CreateCommander())
            {
                return commander.ReaderFist<M>(cmtext);
            }

        }

        public virtual (List<T>, bool) GetAll(params string[] column)
        {
            return GetAll<T>(column);
        }
        public virtual (List<M>, bool) GetAll<M>(params string[] column) where M : class, new()
        {
            string cmtext = query.GetAll<M>(column);
            using (CommanderBase commander = DBContext.CreateCommander())
                return commander.Reader<M>(cmtext);
        }


        public virtual bool Update(T t, int id)
        {
            return Update<T>(t, id);
        }
        public virtual bool Update<M>(M t, int id) where M : class, new()
        {
            string cmtext = query.Update<M>(id.ToString());
            using (CommanderBase commander = DBContext.CreateCommander())
                return commander.NonQuery(cmtext, commander.SetParametrs(t));
        }


        public virtual bool Update(string[] columns, object[] values, int id)
        {
            return Update<T>(columns, values, id);
        }
        public virtual bool Update<M>(string[] columns, object[] values, int id) where M : class, new()
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
