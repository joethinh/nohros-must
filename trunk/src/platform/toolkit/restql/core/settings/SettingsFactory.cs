using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Nohros.Caching.Providers;
using Nohros.Configuration;
using Nohros.Providers;
using Nohros.Resources;

namespace Nohros.Toolkit.RestQL
{
  public partial class Settings
  {
    const string kRestQLSettingsFileName = "restql.config";
    const string kRestQLRootNodeName = "restql";

    /// <summary>
    /// A factory method used to created instances of <see cref="Settings"/>.
    /// class.
    /// </summary>
    /// <returns>
    /// A instance of the <see cref="Settings"/> class.
    /// </returns>
    public static Settings CreateSettings() {
      string settings_file_path = Path.Combine(
        Path.GetDirectoryName(
          Assembly.GetExecutingAssembly().Location), kRestQLSettingsFileName);

      FileInfo settings_file_info = new FileInfo(settings_file_path);

      Settings settings = new Settings();
      settings.LoadAndWatch(settings_file_info, kRestQLRootNodeName);

      settings.cache_provider_ = CreateCacheProvider(settings);
      settings.common_data_provider_ = CreateCommonDataProvider(settings);
      return settings;
    }

    static ICacheProvider CreateCacheProvider(Settings settings) {
      IProviderNode provider = settings.Providers[Strings.kCacheProviderName];
      return
        ProviderFactory<ICacheProviderFactory>
          .CreateProviderFactory(provider)
          .CreateCacheProvider(provider.Options);
    }

    static ICommonDataProvider CreateCommonDataProvider(Settings settings) {
      IProviderNode provider =
        settings.Providers[Strings.kCommonDataProviderName];
      return
        ProviderFactory<ICommonDataProviderFactory>
          .CreateProviderFactory(provider)
          .CreateCommonDataProvider(provider.Options, settings);
    }

    /// <summary>
    /// Creates an instance of the <see cref="IQuerySettings"/> object.
    /// </summary>
    /// <returns>
    /// The newly created <see cref="IQuerySettings"/> object.
    /// </returns>
    public IQuerySettings CreateQuerySettings() {
      XmlElement local_element = GetConfigurationElement(Strings.kQueryNode);
      IProviderNode[] processors =
        Providers.GetProvidersNode(Strings.kQueryProcessorsGroup);

      QuerySettings query_settings = new QuerySettings(processors);
      query_settings.CopyFrom(this);
      query_settings.Load(local_element);
      return query_settings;
    }

    /// <summary>
    /// Creates an instance of the <see cref="ITokenPrincipalMapperSettings"/>
    /// object.
    /// </summary>
    /// <returns></returns>
    public ITokenPrincipalMapperSettings CreateTokenPrincipalMapperSettings() {
      XmlElement local_element =
        GetConfigurationElement(Strings.kTokenPrincipalMapperNode);

      TokenPrincipalMapperSettings token_principal_mapper_settings =
        new TokenPrincipalMapperSettings();
      token_principal_mapper_settings.Load(local_element);
      return token_principal_mapper_settings;
    }

    XmlElement GetConfigurationElement(string element_name) {
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
