using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// An implementation of <see cref="ConfigurationLoader"/> that loads
  /// configuration data into a <see cref="Configuration"/> object.
  /// </summary>
  public class ConfigurationLoader : AbstractConfigurationLoader<Configuration>
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of hte <see cref="ConfigurationLoader"/>
    /// class.
    /// </summary>
    public ConfigurationLoader() : base(new ConfigurationBuilder()) {
    }
    #endregion

    public override Configuration CreateConfiguration(
      IConfigurationBuilder<Configuration> builder) {
      return new Configuration(builder as ConfigurationBuilder);
    }
  }
}
