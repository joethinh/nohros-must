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
  }
}
