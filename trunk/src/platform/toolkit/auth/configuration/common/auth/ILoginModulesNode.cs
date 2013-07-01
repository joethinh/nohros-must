using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="ILoginModulesNode"/> is a collection of
  /// <see cref="ILoginModuleNode"/>.
  /// </summary>
  public interface ILoginModulesNode
  {
    /// <summary>
    /// Gets a <see cref="ILoginModuleNode"/> node whose name is
    /// <paramref name="login_module_name"/>.
    /// </summary>
    /// <param name="login_module_name">
    /// The name of the login module.
    /// </param>
    /// <returns>
    /// A <see cref="ILoginModuleNode"/> object whose name is
    /// <paramref name="login_module_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A login module whose name is <param name="login_module_name"> was
    /// not found.
    /// </param>
    /// </exception>
    ILoginModuleNode GetLoginModuleNode(string login_module_name);

    /// <summary>
    /// Gets a <see cref="LoginModuleNode"/> whose name is
    /// <paramref name="login_module_name"/>.
    /// </summary>
    /// <param name="login_module_name">
    /// The name of the login module.
    /// </param>
    /// <param name="login_module_node">
    /// When this method returns contains a <see cref="LoginModuleNode"/>
    /// object whose name is <paramref name="login_module_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a login module whose name is
    /// <paramref name="login_module_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    bool GetLoginModuleNode(string login_module_name,
      out ILoginModuleNode login_module_node);

    /// <summary>
    /// Gets an array containing all the configured login modules.
    /// </summary>
    /// <remarks>
    /// If no login modules was configured for the application, this method
    /// returns an empty array.
    /// </remarks>
    ILoginModuleNode[] LoginModuleNodes { get; }
  }
}
