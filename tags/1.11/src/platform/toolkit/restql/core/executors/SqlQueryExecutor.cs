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

    public override bool GetConnectionProvider(IQuery query,
      out IConnectionProvider provider) {
      try {
        if (!cache_.GetIfPresent(query.Name, out provider)) {
          provider = CreateConnectionProvider(query.Options);
          cache_.Put(query.Name, provider);
        }
        return true;
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
      return new SqlConnectionProviderFactory().CreateProvider(options);
    }
  }
}
