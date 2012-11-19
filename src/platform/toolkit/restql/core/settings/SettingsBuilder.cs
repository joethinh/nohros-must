using System;
using Nohros.Configuration.Builders;

namespace Nohros.RestQL
{
  public partial class QuerySettings
  {
    public class Builder : AbstractConfigurationBuilder<QuerySettings>
    {
      long query_cache_duration_;

      public Builder SetQueryCacheDuration(long query_cache_duration) {
        query_cache_duration_ = query_cache_duration;
        return this;
      }

      public override QuerySettings Build() {
        return new QuerySettings(this);
      }

      public long QueryCacheDuration {
        get { return query_cache_duration_; }
        internal set { SetQueryCacheDuration(value); }
      }
    }
  }
}
