using System;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The application settings.
  /// </summary>
  public partial class QuerySettings : Configuration.Configuration, IConfiguration,
                                  IQuerySettings
  {
    readonly long query_cache_duration_;

    #region .ctor
    public QuerySettings(Builder builder)
      : base(builder) {
      query_cache_duration_ = builder.QueryCacheDuration;
    }
    #endregion

    /// <inheritdoc/>
    public long QueryCacheDuration {
      get { return query_cache_duration_; }
    }
  }
}
