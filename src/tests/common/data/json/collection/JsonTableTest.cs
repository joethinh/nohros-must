using System;
using NUnit.Framework;

namespace Nohros.Data.Json
{
  public class JsonTableTest
  {
    [Test]
    public void ShouldCountTheNumberOfRows() {
      var table = new JsonTable();
      Assert.That(table.Count, Is.EqualTo(0));

      table = new JsonTable(new[] {"column1", "column2"});
      Assert.That(table.Count, Is.EqualTo(0));

      table = new JsonTable(new[] {"column1", "column2"});
      table.Add(new JsonArray(new [] {
        new JsonString("value"), new JsonString("value"),
      }));

      Assert.That(table.Count, Is.EqualTo(1));
    }

    [Test]
    public void ShouldNotAcceptNullColumnNames() {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
        new JsonTable(new string[] {null}));
    }

    [Test]
    public void ShouldAccpetOnlyRowsWithTheSameNumberOfTableColumns() {
      var table = new JsonTable(new[] {"x", "y"});
      Assert.That(() => table.Add(new JsonArray()), Throws.ArgumentException);
    }

    [Test]
    public void ShouldSerializeAsJsonObject() {
      var table = new JsonTable(new[] {"x", "y"});
      Assert.That(table.AsJson(), Is.EqualTo("{\"columns\":[\"x\",\"y\"],\"rows\":[[]]}"));

      table.Add(new JsonArray(new [] {
        new JsonInteger(0), new JsonInteger(1)
      }));
      Assert.That(table.AsJson(),
        Is.EqualTo("{\"columns\":[\"x\",\"y\"],\"rows\":[[0,1]]}"));
    }
  }
}
