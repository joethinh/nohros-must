using System;

using Nohros.Configuration;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Defines a relashionship between a <see cref="ILoginModule"/> and a
  /// <see cref="ILoginModuleNode"/>.
  /// </summary>
  public interface ILoginModuleNodePair
  {
    /// <summary>
    /// Gets the <see cref="ILoginModule"/> in the
    /// <see cref="ILoginModuleNodePair"/>.
    /// </summary>
    ILoginModule LoginModule { get; }

    /// <summary>
    /// Gets the <see cref="ILoginModuleNode"/> in the
    /// <see cref="ILoginModuleNodePair"/>.
    /// </summary>
    ILoginModuleNode LoginModuleNode { get; }
  }
}
