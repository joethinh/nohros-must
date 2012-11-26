using System;
using System.Data;
using NUnit.Framework;
using Nohros.Data.Json;
using Telerik.JustMock;

namespace Nohros
{
  public class JsonCollectionFactoryTest
  {
    [Test]
    public void ShouldCreateTableCollection() {
      var reader = Mock.Create<IDataReader>();
      var factory = new JsonCollectionFactory();

      IJsonCollection table =
        factory.CreateJsonCollection(JsonCollectionFactory.kJsonTableCollection);
      Assert.That(table, Is.InstanceOf<JsonTable>());

      table = factory
        .CreateJsonCollection(JsonCollectionFactory.kJsonTableCollection, reader);
      Assert.That(table, Is.InstanceOf<JsonTable>());
    }

    [Test]
    public void ShouldCreteArrayCollection() {
      var reader = Mock.Create<IDataReader>();
      var factory = new JsonCollectionFactory();

      IJsonCollection table =
        factory.CreateJsonCollection(JsonCollectionFactory.kJsonArrayCollection);
      Assert.That(table, Is.InstanceOf<JsonArray>());

      table = factory
        .CreateJsonCollection(JsonCollectionFactory.kJsonArrayCollection,
          reader);
      Assert.That(table, Is.InstanceOf<JsonArray>());
    }

    [Test]
    public void ShouldCreateJsonObjectCollection() {
      var reader = Mock.Create<IDataReader>();
      var factory = new JsonCollectionFactory();

      IJsonCollection table =
        factory.CreateJsonCollection(JsonCollectionFactory.kJsonObjectCollection);
      Assert.That(table, Is.InstanceOf<JsonObject>());

      table = factory
        .CreateJsonCollection(JsonCollectionFactory.kJsonObjectCollection,
          reader);
      Assert.That(table, Is.InstanceOf<JsonObject>());
    }
  }
}
