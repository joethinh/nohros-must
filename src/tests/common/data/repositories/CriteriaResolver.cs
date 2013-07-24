using System;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Nohros.Data
{
  public class CriteriaResolver
  {
    public class TestCriteria : AbstractCriteria<TestPoco>
    {
      #region .ctor
      public TestCriteria() {
        MapField(x => x.Name, "t.name_on_database");
        MapField(x => x.ID, "t.id_on_database");
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
      criteria.Resolve("from table", cmd);
      IDbCommand command = cmd.Build();
      Assert.That(command.CommandText,
        Is.EqualTo("select t.name_on_database,t.id_on_database from table"));
    }
  }
}
