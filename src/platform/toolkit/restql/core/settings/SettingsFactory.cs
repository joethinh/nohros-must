using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Nohros.Collections;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  public partial class Settings
  {
    const string kRestQLSettingsFileName = "restql.config";
    const string kRestQLRootNodeName = "restql";
    const string kQueryProcessorProvidersNode = "query-processor";

    /// <summary>
    /// A factory method used to created instances of <see cref="Settings"/>.
    /// class.
    /// </summary>
    /// <returns>
    /// A instance of the <see cref="Settings"/> class.
    /// </returns>
    public static Settings Create() {
      string settings_file_path = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
          kRestQLSettingsFileName);

      FileInfo settings_file_info = new FileInfo(settings_file_path);

      Settings settings = new Settings();
      settings.LoadAndWatch(settings_file_info, kRestQLRootNodeName);
      settings.query_settings_ = CreateQuerySettings(settings);

      return settings;
    }

    static IQuerySettings CreateQuerySettings(Settings settings) {
      DictionaryValue<SimpleProviderNode> providers =
        settings.SimpleProviderNodes[kQueryProcessorProvidersNode];

      QuerySettings query_settings = new QuerySettings(providers.ToArray());

      XmlElement query_element = SelectElement(settings.element, "query");
      if (query_element == null) {
        // A query node was not found, log it and build a default QuerySettings
        // object.
        if (MustLogger.ForCurrentProcess.IsWarnEnabled) {
          MustLogger.ForCurrentProcess.Warn("The query node is not defined.");
        }
      } else {
        query_settings.Load(query_element);
      }

      return query_settings;
    }
  }
}
