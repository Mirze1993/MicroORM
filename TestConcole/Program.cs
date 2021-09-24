using MicroORM;
using System;
using System.Collections.Generic;

namespace TestConcole
{
    class Program
    {
        static void Main(string[] args)
        {
            MicroORM.ORMConfig.ConnectionString = "Server=.\\SQLExpress;Database=Dama;Integrated Security=true;";
            MicroORM.ORMConfig.DbType = MicroORM.DbType.MSSQL;

            var rep = new UserRepostory();
            //rep.Insert<UserClaims>(new UserClaims { AppUserId = 1,Issuer="Sef",Type="efd",Value="fsef",ValueType="sef" });
            //rep r=ue.GetByColumNameFistLeftJoin<UserClaims>("Id",1);
            var r = rep.GetUserss();
            Console.WriteLine(r.Email);

        }

        public class UserRepostory : CRUD<AppUser>
        {
            public AppUser GetUserss()
            {
                using (var comander = DBContext.CreateCommander())
                {
                    
                     var (i,b)=comander.Reader<AppUser>(
                        mm => {
                            while (mm.Read())
                            {
                                var t = comander.GetValues<AppUser>(mm);
                                var c= comander.GetValues<UserClaims>(mm);
                                t.Add(c);
                                return t;
                            }
                            return null;
                        },                        
                        commandText: $"select * from AppUser m left join UserClaims u on u.AppUserId = m.Id");
                    return i;
                }              
            }


        }


        public class AppUser
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }

            public List<UserClaims> UserClaims { get; set; }

            public AppUser()
            {
                UserClaims = new List<UserClaims>();
            }

            public void Add(UserClaims o)
            {
                UserClaims.Add(o);
            }
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
