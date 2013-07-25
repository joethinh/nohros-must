using System;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Nohros.Data
{
  public class AbstractCriteriaTests
  {
    public class TestCriteria : AbstractCriteria<TestPoco>
    {
      #region .ctor
      public TestCriteria() {
        Map(x => x.Name, "t.name_on_database");
        Map(x => x.ID, "t.id_on_database");
      }
      #endregion

      public TestCriteria Select<TProperty>(
        Expression<Func<TestPoco, TProperty>> expression) {
        BaseSelect(expression);
        return this;
      }

      public TestCriteria Where<TProperty>(
        Expression<Func<TestPoco, TProperty>> expression, object value) {
        BaseWhere(expression, value);
        return this;
      }
    }

    public class TestPoco
    {
      public string Name { get; set; }
      public int ID { get; set; }
    }

    [Test]
    public void ShouldMapPropertyToCustomField() {
      TestCriteria criteria = new TestCriteria()
        .Select(x => x.Name)
        .Where(x => x.Name, "maria");

      Assert.That(criteria.Fields.Contains("t.name_on_database"), Is.True);
      Assert.That(criteria.Fields.Count, Is.EqualTo(1));
      Assert.That(criteria.Filters.ContainsKey("t.name_on_database"), Is.True);
      Assert.That(criteria.Filters.Count, Is.EqualTo(1));
    }
  }
}
