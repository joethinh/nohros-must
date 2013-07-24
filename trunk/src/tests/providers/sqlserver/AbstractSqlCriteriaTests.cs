using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using NUnit.Framework;
using Nohros.Data;
using Nohros.Data.SqlServer;

namespace sqlserver
{
  public class AbstractSqlCriteriaTests
  {
    public class TestCriteria : AbstractCriteria<TestPoco>
    {
      #region .ctor
      public TestCriteria() {
        Map(x => x.Name, "t.name_on_database");
        Map(x => x.ID, "t.id_on_database");
      }
      #endregion
    }

    public class TestPoco
    {
      public string Name { get; set; }
      public int ID { get; set; }
    }

    [Test]
    public void ShoulBuildValidQuery() {
      var criteria = new TestCriteria()
        .Select(x => x.Name)
        .Select(x => x.ID);

      var cmd = new CommandBuilder(new SqlCommand());
      SqlQueryResolver.Resolve(cmd, "from table", criteria);
      IDbCommand command = cmd.Build();
      Assert.That(command.CommandText,
        Is.EqualTo("select t.name_on_database,t.id_on_database from table"));
    }
  }
}
