using System;
using System.Xml;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  public class QuerySettings : MustConfiguration, IQuerySettings
  {
    IProviderNode[] processors_;
    long query_cache_duration_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MustConfiguration"/>
    /// class.
    /// </summary>
    public QuerySettings(IProviderNode[] processors) {
      processors_ = processors;
    }
    #endregion

    #region IQuerySettings Members
    /// <inheritdoc/>
    public IProviderNode[] Processors {
      get { return processors_; }
    }

    /// <inheritdoc/>
    public long QueryCacheDuration {
      get { return query_cache_duration_; }

      // set by base class.
      private set { query_cache_duration_ = value; }
    }
    #endregion
  }
}
