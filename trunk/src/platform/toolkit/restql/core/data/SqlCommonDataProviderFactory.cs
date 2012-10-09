using System;
using System.Collections.Generic;

using Nohros.Configuration;
using Nohros.Data.Providers;
using Nohros.Providers;

namespace Nohros.Toolkit.RestQL
{
  public partial class SqlCommonDataProvider : ICommonDataProviderFactory
  {
    #region .ctor
    /// <summary>
    /// Constructor required by the <see cref="ICommonDataProviderFactory"/>
    /// interface.
    /// </summary>
    SqlCommonDataProvider() {
    }
    #endregion

    #region ICommonDataProviderFactory Members
    public ICommonDataProvider CreateCommonDataProvider(
      IDictionary<string, string> options, ISettings settings) {
      string provider_name = options[Strings.kConnectionProviderOption];
      IProviderNode provider = settings.Providers[provider_name];
      IConnectionProvider connection_provider =
        ProviderFactory<IConnectionProviderFactory>
          .CreateProviderFactory(provider)
          .CreateProvider(provider.Options);
      return new SqlCommonDataProvider(connection_provider);
    }
    #endregion
  }
}
