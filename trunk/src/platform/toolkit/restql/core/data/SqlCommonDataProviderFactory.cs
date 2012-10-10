using System;
using System.Collections.Generic;
using Nohros.Configuration;
using Nohros.Data.Providers;
using Nohros.Providers;

namespace Nohros.Toolkit.RestQL
{
  public partial class SqlQueryDataProvider : IQueryDataProviderFactory
  {
    readonly IQuerySettings settings_;

    #region .ctor
    public SqlQueryDataProvider(IQuerySettings settings) {
      settings_ = settings;
    }
    #endregion

    public IQueryDataProvider CreateCommonDataProvider(
      IDictionary<string, string> options) {
      var provider = new SqlConnectionProvider()
      IConnectionProvider connection_provider =
        ProviderFactory<IConnectionProviderFactory>
          .CreateProviderFactory(provider)
          .CreateProvider(provider.Options);
      return new SqlQueryDataProvider(connection_provider);
    }
  }
}
