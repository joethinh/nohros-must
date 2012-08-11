using System;
using Nohros.Logging;

namespace Nohros.Configuration
{
  public interface IMustConfiguration
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
    /// Gets the xml elements that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If this application has no xml elements configured, this property will
    /// returns an empty <see cref="IXmlElementsNode"/>, that is a
    /// <see cref="IXmlElementsNode"/> object that contains no xml elements.
    /// </remarks>
    IXmlElementsNode XmlElements { get; }

    /// <summary>
    /// Gets the logging level that was configured for this application.
    /// </summary>
    LogLevel LogLevel { get; }
  }
}
