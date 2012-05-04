using System;

namespace Nohros.Configuration
{
  public interface IMustConfiguration : IConfiguration
  {
    /// <summary>
    /// Gets the repositories that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If this application has no repositories configured, this property will
    /// returns a empty <see cref="RepositoriesNode"/>, that is a
    /// <see cref="RepositoriesNode"/> object that has no repository.
    /// </remarks>
    IRepositoriesNode Repositories { get; }

    /// <summary>
    /// Gets the providers that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If this application has no providers configured, this property will
    /// returns a empty <see cref="ProvidersNode"/>, that is a
    /// <see cref="ProvidersNode"/> object that has no repository.
    /// </remarks>
    IProvidersNode Providers { get; }

    /// <summary>
    /// Gets the logging level that was configured for this application.
    /// </summary>
    Logging.LogLevel LogLevel { get; }
  }
}
