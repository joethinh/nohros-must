using System;
using Nohros.Configuration.Builders;

namespace Nohros.Toolkit.RestQL
{
  public partial class Settings
  {
    public class Builder : AbstractConfigurationBuilder<Settings>
    {
      long query_cache_duration_;

      public Builder SetQueryCacheDuration(long query_cache_duration) {
        query_cache_duration_ = query_cache_duration;
        return this;
      }

      public override Settings Build() {
        return new Settings(this);
      }

      public long QueryCacheDuration {
        get { return query_cache_duration_; }
      }
    }
  }
}
