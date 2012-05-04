using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// This class represents a single login module configured for the
  /// application specified in the configuration file. Each respective
  /// <see cref="ILoginModuleNode"/> contains a login module's name, a control
  /// flag(specifying whether this login module is Required, Requisite,
  /// Sufficient or Optional) and a login module's specific options.
  /// </summary>
  /// <seealso cref="ILoginConfiguration"/>
  /// <seealso cref="LoginModuleControlFlag"/>
  public interface ILoginModuleNode : IProviderNode
  {
    /// <summary>
    /// Gets the control flag for this login module.
    /// </summary>
    LoginModuleControlFlag ControlFlag { get; }
  }
}
