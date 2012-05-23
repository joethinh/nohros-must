using System;
using System.Xml;

using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  public partial class Settings : IQuerySettings
  {
    IProviderNode[] executors_;
    long query_cache_duration_;

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

    /// <summary>
    /// Parses the properties that is related with the
    /// <see cref="IQuerySettings"/> interface.
    /// </summary>
    void ParseQuerySettings() {
      XmlElement local_element = GetConfigurationElement(Strings.kQueryNode);
      ParseProperties(local_element);
      executors_ = Providers.GetProvidersNode(Strings.kQueryProcessorsGroup);
    }
  }
}
