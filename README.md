<p>DB -de cedvel yaradilir&nbsp;</p>
<pre class="language-csharp"><code>CREATE TABLE [dbo].[Questions ](
	[Name] [nvarchar](50) NULL,	
	[Id] [int] IDENTITY(1,1) NOT NULL)
</code></pre>
<p>&nbsp;</p>
<p>ve bu cedvele uygun EntityClass yaradilir</p>
<pre class="language-csharp"><code>
public class Questions 
    {	
        public int Id { get; set; }
        public string Name { get; set; }
    }</code></pre>
<p>&nbsp;</p>
<p>Interface yaradilir</p>
<pre class="language-csharp"><code> public interface IQuestionsRepository:ICRUD&lt;Questions&gt;
    {
    }</code></pre>
<p>&nbsp;</p>
<p>Bu interface uygun Repository yaradilir</p>
<pre class="language-csharp"><code> public class QuestionsRepository:CRUD&lt;Questions&gt;, IQuestionsRepository
    {
    }</code></pre>
<p>&nbsp;</p>
<p>bu zaman QuestionsRepository sinifinde olan metodlar</p>
<p>1.Insert&nbsp;&nbsp;</p>
<ul>
<li>Result&lt;int&gt; Insert(Questions t, DbTransaction transaction = null)</li>
<li>Result&lt;int&gt; Insert(Action&lt;Questions &gt; item, DbTransaction transaction = null)</li>
<li>Result&lt;int&gt; public virtual Result&lt;int&gt; Insert&lt;M&gt;(M t, DbTransaction transaction = null)</li>
</ul>
<p>&nbsp;</p>
<p>2.Delete</p>
<ul>
<li>Result Delet(int id, DbTransaction transaction = null)</li>
<li>Result Delet&lt;M&gt;(int id,DbTransaction transaction=null)</li>
</ul>	
<p>3. get</p>
<ul>
<li>&nbsp;Result&lt;List&lt;Questions&gt;&gt; GetByColumName(string columName, object value, params string[] selectColumn) &nbsp; &nbsp;columName=value serti daxilinde secimis selectColumn stunlar gelecek. eger selectColumn secilmezse butun stunlar gelecek</li>
<li>Result&lt;List&lt;M&gt;&gt; GetByColumName&lt;M&gt;(string columName, object value, params string[] selectColumn)</li>
<li>Result&lt;List&lt;Questions&gt;&gt; GetByColumNameLeftJoin&lt;Join&gt;(string columName, object value)&nbsp; &nbsp;bu metodan istifade etmek ucun Questions sinifinde&nbsp;</li>
</ul>
<pre class="language-csharp"><code>[DbMaping(DbMap.noMaping)]
public List&lt;Join&gt; Join { get; set; } = new();</code></pre>
<p>bele property ve</p>
<pre class="language-csharp"><code> public void Join(Join j)
 {
       Join.Add(j);
  }</code></pre>
<p>Join adinda metod olmalidir</p>
<ul>
<li>Result&lt;List&lt;M&gt;&gt; GetByColumNameLeftJoin&lt;M, Join&gt;(string columName, object value) -umumi hal</li>
</ul>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>yuxarki metodlara uygun olaraq ilk neticeni donderen asagki metodlar da vardir</p>
<ul>
<li>Result&lt;Questions&gt; GetByColumNameFist(string columName, object value, params string[] selectColumn)</li>
<li>Result&lt;Questions&gt; GetByColumNameFistLeftJoin&lt;Join&gt;(string columName, object value)</li>
<li>Result&lt;M&gt; GetByColumNameFist&lt;M&gt;(string columName, object value, params string[] selectColumn)</li>
<li>Result&lt;M&gt; GetByColumNameFistLeftJoin&lt;M, Join&gt;(string columName, object value)</li>
</ul>
<p>eyni zamanda Queryde where keywordunden sonra hisseni yazib netice elde etmek ucun olan metodlar</p>
<ul>
<li>&nbsp;Result&lt;List&lt;Questions&gt;&gt; GetWithCondition(string condition, params string[] selectColumn)</li>
<li>Result&lt;List&lt;Questions&gt;&gt; GetWithConditionLeftJoin&lt;Join&gt;</li>
<li>Result&lt;List&lt;M&gt;&gt; GetWithCondition&lt;M&gt;(string condition, params string[] selectColumn)</li>
<li>Result&lt;List&lt;M&gt;&gt; GetWithConditionLeftJoin&lt;M,Join&gt;</li>
</ul>
<p>&nbsp;</p>
<ul>
<li>Result&lt;List&lt;Questions&gt;&gt; GetWithConditionFist(string condition, params string[] selectColumn)</li>
<li>Result&lt;List&lt;Questions&gt;&gt; GetWithConditionLeftJoinFist&lt;Join&gt;</li>
<li>Result&lt;List&lt;M&gt;&gt; GetWithConditionFist&lt;M&gt;(string condition, params string[] selectColumn)</li>
<li>Result&lt;List&lt;M&gt;&gt; GetWithConditionLeftJoinFist&lt;M,Join&gt;</li>
</ul
<p>Eyni Qaydada GetAll Metodlari</p>
<ul>
<li>Result&lt;List&lt;Questions &gt;&gt; GetAll(params string[] column)</li>
<li>Result&lt;List&lt;Questions &gt;&gt; GetAllLeftJoin&lt;Join&gt;(params string[] column)</li>
<li>Result&lt;List&lt;M&gt;&gt; GetAll&lt;M&gt;(params string[] column)</li>
<li>Result&lt;List&lt;M&gt;&gt; GetAllLeftJoin&lt;M,Join&gt;()</li>
</ul>
<p>4. Update&nbsp;</p>
<ul>
<li>Result Update(Questions &nbsp;t, int id,DbTransaction transaction = null)&nbsp; &nbsp; &nbsp;burada t butun parametrlleri update olunur</li>
<li>&nbsp;Result Update&lt;M&gt;(M t, int id,,DbTransaction transaction = null)</li>
<li>Result Update(Action&lt;Dictionary&lt;string,object&gt;&gt;items,int id,,DbTransaction transaction = null) &nbsp; Dictionary--de key parametrName&nbsp; &nbsp;value ise parametrin deyeridir. Yalniz qeyd olunmus parametrer deyisecek</li>
<li>Result Update(string[] columns, object[] values, int id,DbTransaction transaction = null) &nbsp;columns.Length==values.Length&nbsp; &nbsp;olmalidir</li>
<li>Result Update&lt;M&gt;(string[] columns, object[] values, int id,DbTransaction transaction = null)</li>
</ul>
<p>&nbsp;</p>
<p>Bundan elave metodlar yazib storeProsedurlar cagrila, Query-ler gonderile biler.parametrler set olunur.</p>
<p>gelen melumatlar qeyd olunmus sinife map-lene biler ve ya map-lemek ucun mentod parametr kimi teyin edile biler.</p>
<p>Numuneler</p>
<pre class="language-csharp"><code> public Result&lt;List&lt;CategoryResponse&gt;&gt; GetCategoryOnlyUpByType(CategoryType type)
        {
            using (var commander = DBContext.CreateCommander())
            {
                var result = commander.Reader&lt;CategoryResponse&gt;(
                    commandText: "GetCategoryOnlyUpByType"
                    , commandType: System.Data.CommandType.StoredProcedure
                    , parameters: new List&lt;System.Data.Common.DbParameter&gt; { 
                        commander.SetParametr("type", type) }
                    );
                return result;
            }
        }</code></pre>
<p>&nbsp;</p>
<pre class="language-csharp"><code>public Result&lt;int&gt; SingInGoogle(GoogleLoginReqType googleLoginReq)
        {
            using (var commander = DBContext.CreateCommander())
            {
                var p = commander.SetParametrs(googleLoginReq);
                var outPr = commander.SetReturnParametr();
                var oUserId = commander.SetOutParametr("oUserId", System.Data.DbType.Int32);               
                oUserId.DbType = System.Data.DbType.Int32;
                p.Add(outPr);
                p.Add(oUserId);
                var b = commander.NonQuery("SingInGoogle"
                    , commandType: System.Data.CommandType.StoredProcedure
                    , parameters: p);
                var i = outPr.Value != null ? (int)outPr.Value : -1
                int id = 0;
                int.TryParse(oUserId.Value?.ToString(),out id);
                return new Result&lt;int&gt;
                {
                    Success = b.Success,
                    Value = id,
                    Message = !string.IsNullOrWhiteSpace(b.Message) ? b.Message : i &lt; 0 ? "OldUser" : "NewUser"
                };
            }
        }</code></pre>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>yuxarki numunedeki comander obyektinde olan metodlar</p>
<ul>
<li>DbParameter SetParametr(string paramName, object value)</li>
<li>DbParameter SetParametr()&nbsp;</li>
<li>DbParameter SetReturnParametr(string paramName)</li>
<li>DbParameter SetReturnParametr()</li>
<li>DbParameter SetOutParametr(string paramName, System.Data.DbType dbType)</li>
<li>DbParameter SetInputOutputParametr(string paramName, object value)</li>
</ul>
<p>&nbsp;</p>
<ul>
<li>Result NonQuery(string commandText, List&lt;DbParameter&gt; parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)</li>
<li>Result&lt;object&gt; Scaller(string commandText, List&lt;DbParameter&gt; parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)</li>
<li>Reader&lt;T&gt;(Func&lt;DbDataReader, T&gt; readMetod, string commandText, List&lt;DbParameter&gt; parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)</li>
<li>Result&lt;List&lt;T&gt;&gt; Reader&lt;T&gt;(string commandText, List&lt;DbParameter&gt; parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)</li>
<li>&nbsp;ReaderFist&lt;T&gt;(string commandText, List&lt;DbParameter&gt; parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)</li>
<li>Result&lt;List&lt;T&gt;&gt; ReaderLeftJoin&lt;T,M&gt;(string commandText, List&lt;DbParameter&gt; parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)</li>
<li>Result&lt;T&gt; ReaderLeftJoinFist&lt;T, M&gt;(string commandText, List&lt;DbParameter&gt; parameters = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null)</li>
</ul>
<p>Hemcinin transaction yaratmaq ucun&nbsp; -DbTransaction TransactionStart() metodu da vardir.</p>
<p>Qeyd.Yaradilis transaction mutleq commit ve ya roll back edilmelidir.Sonda Dispose edilmeidir.</p>
<p>Elave olaraq&nbsp;</p>
<pre class="language-csharp"><code>public class HashCreate</code></pre>
<p>sinifi.</p>
<p>Hemcinin Mediator patern ucun Mediator sinifinde</p>
<p>&nbsp;public Resp ToDo&lt;Req, Resp&gt;(Req req) where Req : IRequest where Resp : IResponse</p>
<p>medodu da vardir.</p>
<p>&nbsp;</p>
<p>Loglama text faylina ve ya bazaya da yazilia biler. Default table name --AppLog</p>
<p>&nbsp;</p>
<p>AllConfig clasinda SetConfig metodu&nbsp;vastesi ile Orm config edilir.</p>
<p>numune</p>
<pre class="language-csharp"><code> MicroORM.AllConfig.SetConfig(c =&gt;
                {
                    c.ConnectionString = Configuration.GetConnectionString("BloggingDatabase");
                    c.DbType = MicroORM.DbType.MSSQL;
                    c.IsFileLog = true;
                    c.FolderPath = System.IO.Path.Combine(Environment.WebRootPath, "AllLogs");
                    c.IsDbLogger = true;
                    c.LogDbName = "AppLog";
                });</code></pre>
<p>&nbsp;</p>
<p>Yuxarda qeyd olunan metodlarin siniflerin async variantlari da vardir</p>
<p>--------</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>Join olunacaq Class deyisenlerinin Evveline J herfi evez olunur</p>
<p>&nbsp;</p>
<pre class="language-csharp"><code> public class AppUser
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            [DbMaping(DbMap.noMaping)]
            public List&lt;UserClaims&gt; UserClaims { get; set; } 
            public AppUser()
            {
                UserClaims = new List&lt;UserClaims&gt;();
            }
            public void Join(UserClaims j)
            {
                UserClaims.Add(j);
            }
        }</code></pre>
<p>---</p>
<pre class="language-csharp"><code> public class UserClaims
        {
            public int JId { get; set; }
            public int JAppUserId { get; set; }
            public string JType { get; set; }
            public string JValue { get; set; }
            public string JValueType { get; set; }
            public string JIssuer { get; set; }
        }</code></pre>
<p>--</p>
<pre class="language-csharp"><code> var t= rep.GetByColumNameFistLeftJoin&lt;AppUser,UserClaims&gt;("Id",2);</code></pre>
