using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// An implementation of the <see cref="AbstractConfigurationBuilder{T}"/>
  /// class that builds instance of the <see cref="Configuration"/> class.
  /// </summary>
  public class ConfigurationBuilder :
    AbstractConfigurationBuilder<Configuration>
  {
    public override Configuration Build() {
      return new Configuration(this);
    }
  }
}
