using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Nohros.Collections;
using Nohros.Configuration;
using Nohros.Logging;
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
      settings.query_settings_ = CreateQuerySettings(settings);
      settings.token_principal_mapper_settings_ =
        CreateTokenPrincipalMapperSettings(settings);
      return settings;
    }

    /// <summary>
    /// Creates an instance of the <see cref="IQuerySettings"/> object.
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IQuerySettings CreateQuerySettings(Settings settings) {
      XmlElement element = GetConfigurationElement(settings.element,
        Strings.kQueryNode);
      IProviderNode[] processors =
        settings.Providers.GetProvidersNode(Strings.kQueryProcessorsGroup);
      QuerySettings query_settings = new QuerySettings(processors);
      query_settings.Load(element);
      return query_settings;
    }

    /// <summary>
    /// Creates an instance of the <see cref="ITokenPrincipalMapperSettings"/>
    /// object.
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static ITokenPrincipalMapperSettings
      CreateTokenPrincipalMapperSettings(Settings settings) {
      XmlElement element = GetConfigurationElement(settings.element,
        Strings.kTokenPrincipalMapperNode);
      TokenPrincipalMapperSettings token_principal_mapper_settings =
        new TokenPrincipalMapperSettings();
      token_principal_mapper_settings.Load(element);
      return token_principal_mapper_settings;
    }

    static XmlElement GetConfigurationElement(XmlElement element,
      string element_name) {
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
