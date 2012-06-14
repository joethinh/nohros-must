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
  /// <summary>
  /// The application settings.
  /// </summary>
  public partial class Settings : MustConfiguration, IConfiguration,
                                  IMustConfiguration, ISettings
  {
    const string kRestQLSettingsFileName = "restql.config";
    const string kRestQLRootNodeName = "restql";

    ICacheProvider cache_provider_;
    ICommonDataProvider common_data_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    public Settings() {
    }
    #endregion

    #region IConfiguration Members
    public override void Load() {
      string current_assembly_location =
        Assembly.GetExecutingAssembly().Location;
      string current_assembly_directory =
        Path.GetDirectoryName(current_assembly_location);
      string config_file_name =
        Path.Combine(current_assembly_directory, kRestQLSettingsFileName);
      LoadAndWatch(config_file_name, kRestQLRootNodeName);
    }

    void IConfiguration.Load(string root_node_name) {
      base.Load(root_node_name);
    }

    /// <inheritdoc/>
    void IConfiguration.Load(string config_file_name, string root_node_name) {
      base.Load(config_file_name, root_node_name);
    }

    /// <inheritdoc/>
    void IConfiguration.Load(FileInfo config_file_info,
      string root_node_name) {
      base.Load(config_file_info, root_node_name);
    }

    /// <inheritdoc/>
    void IConfiguration.Load(XmlElement element) {
      base.Load(element);
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

      cache_provider_ = GetCacheProvider();
      common_data_provider_ = GetCommonDataProvider();
    }

    ICacheProvider GetCacheProvider() {
      IProviderNode provider = Providers[Strings.kCacheProviderName];
      return
        ProviderFactory<ICacheProviderFactory>
          .CreateProviderFactory(provider)
          .CreateCacheProvider(provider.Options);
    }

    ICommonDataProvider GetCommonDataProvider() {
      IProviderNode provider = Providers[Strings.kCommonDataProviderName];
      return
        ProviderFactory<ICommonDataProviderFactory>
          .CreateProviderFactory(provider)
          .CreateCommonDataProvider(provider.Options, this);
    }
  }
}
