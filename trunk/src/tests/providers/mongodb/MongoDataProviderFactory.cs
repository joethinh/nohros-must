using System;
using System.Collections.Generic;
using NUnit.Framework;
using Nohros.Data.MongoDB;

namespace Nohros.MongoDB.Tests
{
  public class MongoDataProviderFactoryTests
  {
    [Test]
    public void ShouldCreateProviderUsingConnectionString() {
      var factory = new MongoDatabaseProviderFactory();
      var options = new Dictionary<string, string> {
        {
          MongoDatabaseProviderFactory.kConnectionStringOption,
          "mongodb://127.0.0.1"
        },
        {MongoDatabaseProviderFactory.kDatabaseOption, "mydatabase"}
      };
      Assert.DoesNotThrow(() => factory.CreateProvider(options));
    }

    [Test]
    public void ShouldNotCreateProviderWhenDatabaseOptionIsMissing() {
      var factory = new MongoDatabaseProviderFactory();
      var options = new Dictionary<string, string> {
        {
          MongoDatabaseProviderFactory.kConnectionStringOption,
          "mongodb://127.0.0.1/database"
        },
      };
      try {
        factory.CreateProvider(options);
        Assert.Fail("Exception was not throwed");
      } catch {
        Assert.Pass();
      }
    }

    [Test]
    public void ShouldCreateProviderUsingOptions() {
      var factory = new MongoDatabaseProviderFactory();
      var options = new Dictionary<string, string> {
        {
          MongoDatabaseProviderFactory.kDatabaseOption,
          "mydatabase"
        },
        {MongoDatabaseProviderFactory.kHostOption, "localhost"}
      };
    }
  }
}
