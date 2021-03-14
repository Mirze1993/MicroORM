using MicroORM.Interface;
using MicroORM.SqlQueries;

namespace MicroORM
{
    public class DBContext
    {

        public CommanderBase CreateCommander()
        {
            CommanderBase commander = null; ;
            switch (ORMConfig.DbType)
            {
                case DbType.MSSQL:
                    commander = new SqlCommander();
                    break;
                case DbType.Oracle:
                    commander = new OracleCommander();
                    break;
                default:
                    break;
            }
            return commander;
        }

        public  IQuery CreateQuary()
        {
            IQuery query = null;
            switch (ORMConfig.DbType)
            {
                case DbType.MSSQL:
                    query = new SqlQuery();
                    break;
                case DbType.Oracle:
                    query = new SqlQuery();
                    break;
                default:
                    break;
            }
            return query;
        }

    }
}
