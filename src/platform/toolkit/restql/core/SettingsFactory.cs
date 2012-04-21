using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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
    /// 
    /// </summary>
    /// <returns></returns>
    public ISettings CreateSettings() {
      string settings_file_path = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        kRestQLSettingsFileName);

      FileInfo settings_file_info = new FileInfo(settings_file_path);

      Settings settings = new Settings();
      settings.LoadAndWatch(settings_file_info, kRestQLRootNodeName);

      query_settings_ = CreateQuerySettings(settings);

      return settings;
    }

    IQuerySettings CreateQuerySettings(Settings settings) {
      DictionaryValue<SimpleProviderNode> providers =
        settings.SimpleProviderNodes[kQueryProcessorProvidersNode];

      QuerySettings query_settings = new QuerySettings(providers.ToArray());
    }
  }
}
