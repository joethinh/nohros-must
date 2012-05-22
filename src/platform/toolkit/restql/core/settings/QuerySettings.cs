using System;
using System.Xml;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  public class QuerySettings : Settings, IQuerySettings
  {
    readonly IProviderNode[] executors_;
    long query_cache_duration_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QuerySettings"/>
    /// class.
    /// </summary>
    public QuerySettings(IProviderNode[] executors) {
      executors_ = executors;
    }
    #endregion

    #region IQuerySettings Members
    /// <inheritdoc/>
    public IProviderNode[] Executors {
      get { return executors_; }
    }

    /// <inheritdoc/>
    public long QueryCacheDuration {
      get { return query_cache_duration_; }
      protected set { query_cache_duration_ = value; }
    }
    #endregion
  }
}
