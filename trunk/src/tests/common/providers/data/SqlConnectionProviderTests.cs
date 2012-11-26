using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NUnit.Framework;
using Nohros.Data.Providers;

namespace Nohros.Common.Providers
{
  public class SqlConnectionProviderTests
  {
    [Test]
    public void ShouldCreateSqlConnectionProvider() {
      var options = new Dictionary<string, string>();
      options[SqlConnectionProviderFactory.kConnectionStringOption] = "";

      var factory = new SqlConnectionProviderFactory();
      IConnectionProvider provider = factory.CreateProvider(options);
      Assert.That(provider, Is.TypeOf(typeof (SqlConnectionProvider)));

      options.Clear();
      options[SqlConnectionProviderFactory.kLoginOption] = "login";
      options[SqlConnectionProviderFactory.kPasswordOption] = "password";
      options[SqlConnectionProviderFactory.kServerOption] = "server";
      options[SqlConnectionProviderFactory.kInitialCatalogOption] = "database";

      provider = factory.CreateProvider(options);
      Assert.That(provider, Is.TypeOf(typeof(SqlConnectionProvider)));
    }

    [Test]
    public void ShouldThrowKeyNotFoundExeptionWhenRequiredOptionIsMissing() {
      var options = new Dictionary<string, string>();
      options[SqlConnectionProviderFactory.kConnectionStringOption] = "";

      var factory = new SqlConnectionProviderFactory();
      options.Clear();

      options[SqlConnectionProviderFactory.kLoginOption] = "login";
      Assert.Throws<KeyNotFoundException>(() => factory.CreateProvider(options));
      options.Clear();

      options[SqlConnectionProviderFactory.kPasswordOption] = "password";
      Assert.Throws<KeyNotFoundException>(() => factory.CreateProvider(options));
      options.Clear();

      options[SqlConnectionProviderFactory.kServerOption] = "server";
      Assert.Throws<KeyNotFoundException>(() => factory.CreateProvider(options));
      options.Clear();
    }
  }
}
