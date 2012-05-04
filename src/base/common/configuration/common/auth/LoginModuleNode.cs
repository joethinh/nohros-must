using System;
using System.Collections.Generic;
using System.Xml;
using System.Threading;
using Nohros.Security.Auth;

namespace Nohros.Configuration
{
  /// <summary>
  /// This class represents a single login module configured for the
  /// application specified in the configuration file.
  /// </summary>
  /// <remarks>
  /// Each respective <see cref="LoginModuleNode"/> contains a login module's
  /// name, a control flag (specifying whether this login module is Required,
  /// Requisite, Sufficient or Optional) and a login module's specific options.
  /// </remarks>
  /// <seealso cref="ILoginConfiguration"/>
  /// <seealso cref="LoginModuleControlFlag"/>
  public partial class LoginModuleNode : ProviderNode, ILoginModuleNode
  {
    readonly LoginModuleControlFlag control_flag_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModuleNode"/> class
    /// by using the specified module name, type and control flag.
    /// </summary>
    /// <param name="name">
    /// The name of the login module.
    /// </param>
    /// <param name="type">
    /// The assembly-qualified name of the login module type.
    /// </param>
    /// <param name="control_flag">
    /// The login module control flag.
    /// </param>
    protected LoginModuleNode(string name, string type,
      LoginModuleControlFlag control_flag)
      : this(name, type, control_flag, AppDomain.CurrentDomain.BaseDirectory) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderNode"/> class by
    /// using the specified provider name, type and assembly location.
    /// </summary>
    /// <param name="name">
    /// The name of the login module.
    /// </param>
    /// <param name="type">
    /// The assembly-qualified name of the login module type.
    /// </param>
    /// <param name="control_flag">
    /// The login module control flag.
    /// </param>
    /// <param name="location">
    /// The path to the directory where the login module assembly file is
    /// located.
    /// </param>
    protected LoginModuleNode(string name, string type,
      LoginModuleControlFlag control_flag, string location)
      : base(name, type, location) {
      control_flag_ = control_flag;
    }
    #endregion

    /// <summary>
    /// Gets the control flag for this login module.
    /// </summary>
    public LoginModuleControlFlag ControlFlag {
      get { return control_flag_; }
    }
  }
}
