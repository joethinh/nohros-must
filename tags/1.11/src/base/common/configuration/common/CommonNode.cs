using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// Represents the node that contains the common configuration data.
  /// </summary>
  public partial class CommonNode: AbstractConfigurationNode
  {
    ProvidersNode providers_;
    RepositoriesNode repositories_;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommonNode"/> class.
    /// </summary>
    public CommonNode() : base(Strings.kCommonNodeName) { }

    /// <summary>
    /// Gets the providers that was configured for this application.
    /// </summary>
    public ProvidersNode Providers {
      get { return providers_; }
    }

    /// <summary>
    /// Gets the list of repositories that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If no repositories was configured this class will returns a 
    /// </remarks>
    public RepositoriesNode Repositories {
      get { return repositories_; }
    }
  }
}
