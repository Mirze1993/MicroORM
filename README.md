<pre class="language-csharp"><code>CREATE TABLE [dbo].[AppUser](
	[Name] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


public class AppUser
    {	
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }</code></pre>


<pre class="language-csharp"><code>public class UserRepository : CRUD&lt;AppUser&gt;
    {
       
    }</code></pre>
    
    //crud metod add to repository.     
    //also add store proc
    
    
    using (var commander = DBContext.CreateCommander())
            {
                var outParam = commander.SetOutputParametr();
                var sqlparams = new List&lt;System.Data.Common.DbParameter&gt; {
                    commander.SetParametr("Id",id),
                    outParam
                };

                var b = commander.NonQuery("AcceptGame", commandType: System.Data.CommandType.StoredProcedure,
                    parameters: sqlparams);
                if (outParam.Value == null) return 0;
                return (int)outParam.Value;
            }
	   
	    
	    
    
