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
<p>2.</p>
<ul>
<li>Result Delet(int id, DbTransaction transaction = null)</li>
<li>Result Delet<M>(int id,DbTransaction transaction=null)</li>	
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
