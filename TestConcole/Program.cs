using MicroORM;
using MicroORM.Attributes;
using MicroORM.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TestConcole
{
    class Program
    {

        public static string GetByColumName(string o, string[] columNames, params string[] selectColumn) 
        {
            var s = columNames.Select(c => $"{c} = @{c}")?.Aggregate((a, b) => $"{a} {o} {b}");

            return s;
        }

        static void Main(string[] args)
        {

            var s = new string[3] { "a", "b", "cd" };

            

            Console.WriteLine(GetByColumName("and", s));

           
           

            AllConfig.SetConfig(mm =>
            {
                mm.ConnectionString = "Server=.\\SQLExpress;Database=Bonus;Integrated Security=true;";
                mm.DbType = MicroORM.DbType.MSSQL;
                mm.IsDbLogger = true;
                mm.LogDbName = "AppLog";
                mm.IsFileLog= true;
            });

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //var t = DbLogger.WriteDb("few");
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);

            //sw.Start();
            //var t2 = DbLogger.WriteDbAsync("few").Result;
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);

            //sw.Start();
            //var t3 = DbLogger.WriteDbAsync("few").Result;
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);

            //sw.Start();


            //for (int i = 0; i < 1000; i++)
            //{
            //     DbLogger.WriteDb("asd");
            //}
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);

            var rep = new UserRepostory1();
            var t=rep.GetByColumsFistAsync(
                new Dictionary<string, object> { { "Id", 2 }, { "Name", "2mirze2" } }
            , LogicalOperator.or).Result;
            var lf = new LogWriteFile();
            
            lf.WriteFileAsync("sd", LogLevel.Warning);
            lf.WriteFileAsync("sd", LogLevel.Warning);
            lf.WriteFileAsync("sd", LogLevel.Warning);

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

        public class UserRepostory1 : CRUDSyncAndAsync<AppUser>
        {
            //void getId()
            //{
            //    AsyncMetod.
            //}
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
