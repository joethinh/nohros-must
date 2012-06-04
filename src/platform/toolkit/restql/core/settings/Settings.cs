using System;
using System.Xml;

using Nohros.Caching.Providers;
using Nohros.Configuration;
using Nohros.Resources;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// The application settings.
  /// </summary>
  public partial class Settings : MustConfiguration, ISettings
  {
    ICacheProvider cache_provider_;
    ICommonDataProvider common_data_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    protected Settings() {
    }
    #endregion

    #region ISettings Members
    /// <inheritdoc/>
    public ICacheProvider CacheProvider {
      get { return cache_provider_; }
    }

    /// <inheritdoc/>
    public ICommonDataProvider CommonDataProvider {
      get { return common_data_provider_; }
    }
    #endregion

    protected override void OnLoadComplete() {
      base.OnLoadComplete();
      ParseQuerySettings();
      ParseTokenPrincipalMapperSettings();
    }

    /// <summary>
    /// Gets a <see cref="XmlElement"/> named <paramref name="element_name"/>
    /// from the loaded configuration file.
    /// </summary>
    /// <param name="element_name">
    /// The name of the xml element to get.
    /// </param>
    /// <returns>
    /// A <see cref="XmlElement"/> which name is
    /// <paramref name="element_name"/>.
    /// </returns>
    /// <exception cref="ConfigurationException">
    /// A <see cref="XmlElement"/> named <paramref name="element_name"/> does
    /// not exists in the configuration file.
    /// </exception>
    protected XmlElement GetConfigurationElement(string element_name) {
      XmlElement local_element = SelectElement(element, element_name);
      if (local_element == null) {
        throw new ConfigurationException(
          string.Format(
            StringResources.Configuration_MissingNode, element_name));
      }
      return local_element;
    }
  }
}
