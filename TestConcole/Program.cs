using MicroORM;
using MicroORM.Attributes;
using MicroORM.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestConcole
{
    class Program
    {
        static void Main(string[] args)
        {
            //MicroORM.ORMConfig.ConnectionString = "Server=.\\SQLExpress;Database=Bonus;Integrated Security=true;";
            //MicroORM.ORMConfig.DbType = MicroORM.DbType.MSSQL;
            //MicroORM.Logging.DBLoggerOptions.IsDbLogger = true;
            //MicroORM.Logging.DBLoggerOptions.LogDbName = "AppLog";

            AllConfig.SetConfig(mm => {
                mm.ConnectionString = "Server=.\\SQLExpress;Database=Bonus;Integrated Security=true;";
                mm.DbType= MicroORM.DbType.MSSQL;
                mm.IsDbLogger = true;
                mm.LogDbName = "AppLog";                
            });

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var t = DbLogger.WriteDb("few");
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            sw.Start();
            var t2 = DbLogger.WriteDbAsync("few").Result;
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            sw.Start();
            var t3 = DbLogger.WriteDbAsync("few").Result;
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            sw.Start();

            
            for (int i = 0; i < 1000; i++)
            {
                 DbLogger.WriteDb("asd");
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            //var rep = new UserRepostory();
            //rep.Insert<UserClaims>(new UserClaims { AppUserId = 1,Issuer="Sef",Type="efd",Value="fsef",ValueType="sef" });
            //rep r=ue.GetByColumNameFistLeftJoin<UserClaims>("Id",1);
            //var r = rep.GetUserss();
            Console.WriteLine(t.Message);

        }

        public class UserRepostory : CRUD<AppUser>
        {
            public AppUser GetUserss()
            {
                using (var comander = DBContext.CreateCommander())
                {
                    
                     var r=comander.Reader<AppUser>(
                        mm => {
                            while (mm.Read())
                            {
                                var t = comander.GetValues<AppUser>(mm);
                                var c= comander.GetValues<UserClaims>(mm);
                                //t.Add(c);
                                return t;
                            }
                            return null;
                        },                        
                        commandText: $"select * from AppUser m left join UserClaims u on u.AppUserId = m.Id");
                    return r.Value;
                }              
            }


        }


        public class AppUser
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }

            //[DbMaping(DbMap.noMaping)]
            //public List<Join> Join { get; set; } = new();




            //public AppUser()
            //{
            //    UserClaims = new List<UserClaims>();
            //}

            //public void Join(Join j)
            //{
            //    Join.Add(j);
            //}
        }

        public class UserClaims
        {
            public int Id { get; set; }
            public int AppUserId { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public string ValueType { get; set; }
            public string Issuer { get; set; }
        }

    }
}
