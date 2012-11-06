using System;
using System.Collections.Generic;
using Nohros.Caching;
using Nohros.Data.Json;
using Nohros.Data.Providers;
using Nohros.Resources;

namespace Nohros.RestQL
{
  public class SqlQueryExecutor : AbstractSqlQueryExecutor
  {
    const string kClassName = "Nohros.RestQL.SqlQueryExecutor";

    readonly ICache<IConnectionProvider> cache_;
    readonly RestQLLogger logger_;

    #region .ctor
    public SqlQueryExecutor(IJsonCollectionFactory json_collection_factory,
      ICache<IConnectionProvider> cache)
      : base(json_collection_factory) {
      cache_ = cache;
      logger_ = RestQLLogger.ForCurrentProcess;
    }
    #endregion

    public override bool GetConnectionProvider(
      IDictionary<string, string> options, out IConnectionProvider provider) {
      try {
        string name;
        if (options.TryGetValue(Strings.kConnectionProviderOption, out name)) {
          if (!cache_.GetIfPresent(name, out provider)) {
            provider = CreateConnectionProvider(options);
          }
          return true;
        }
      } catch (Exception e) {
        logger_.Error(
          string.Format(StringResources.Log_MethodThrowsException, kClassName,
            "GetConnectionProvider"),
          e);
      }
      provider = null;
      return false;
    }

    public IConnectionProvider CreateConnectionProvider(
      IDictionary<string, string> options) {
      string name;
      if (options.TryGetValue(Strings.kConnectionProviderOption, out name)) {
        return new SqlConnectionProviderFactory()
          .CreateProvider(options);
      }
      throw new KeyNotFoundException(Strings.kConnectionProviderOption);
    }
  }
}
