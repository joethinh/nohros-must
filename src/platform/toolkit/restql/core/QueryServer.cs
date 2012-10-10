using System;

namespace Nohros.Toolkit.RestQL
{
  public partial class QueryServer
  {
    readonly IQueryProcessor query_processor_;
    readonly IQuerySettings settings_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryServer"/> object using
    /// the specified <see cref="IQueryProcessor"/> objects.
    /// </summary>
    public QueryServer(Builder builder) {
      query_processor_ = builder.QueryProcessor;
      settings_ = builder.QuerySettings;
    }
    #endregion

    /// <summary>
    /// Gets the server's query principal mapper.
    /// </summary>
    public IQueryProcessor QueryProcessor {
      get { return query_processor_; }
    }

    /// <summary>
    /// Gets the server's settings.
    /// </summary>
    public IQuerySettings QuerySettings {
      get { return settings_; }
    }
  }
}
