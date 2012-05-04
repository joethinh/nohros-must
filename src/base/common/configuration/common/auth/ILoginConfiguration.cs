using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// A configuration object is responsible for specifiying which login modules
  /// should be used for a particular application, and what order the login
  /// modules should be invoked.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Each module is indexed by its name. Each type property of each login
  /// module entry must contain the fully qualified class name. Authentication
  /// proceeds down the module list in the exact order specified.
  /// </para>
  /// <para>
  /// The flag value controls the overall behavior as authentication
  /// proceeds down the stack.
  /// </para>
  /// <para>
  /// The overall authentication succeeds only if all "required" and
  /// "requisite" login modules succeed. If a sufficient login module is
  /// configured and succeeds, then only the "required" and "requisite" login
  /// modules prior to that "sufficient" login module need to have succeeded
  /// for the overall authentication to succeed. If no "required" or
  /// "requisite" login module are configured for an application, then at least
  /// one "sufficient" or "optional" login module must secceed.
  /// </para>
  /// <para>
  /// Module options is a collection of Xml nodes containing the login module
  /// specific values which are passed directly to the underlying login
  /// modules. Options are defined by the login module itself, and control the
  /// behavior within it. For example, a login module may define options to
  /// support debugging/testing capabilities.
  /// </para>
  /// <para>
  /// The default way to specify options in the configuration is by using the
  /// following syntax: <options option-name="option-value"/>
  /// If a string in the form ${system.enviroment.property}, occurs in the
  /// value, it will be expanded to the value of the system property. Note
  /// that there is no limit to the number of options a login module may
  /// define.
  /// </para>
  /// <para>
  /// <example>
  /// The following represents an example of configuration entry on the syntax
  /// above:
  /// <code>
  ///   <login-modules>
  ///     <module
  ///       name="MyLoginModule"
  ///       type="MyNamespace.MyLoginModule, Nohros"
  ///       control-flag="required">
  ///       <options
  ///         useTicketCache="true"
  ///         ticketCache="${System.Environment.CurrentDirectory"
  ///       />
  ///     </module>
  ///   </login-modules>
  /// </code>
  /// </example>
  /// This configuration specifies that the application requires users to
  /// authenticate to the MyNamespace.MyLoginModule, which is required to
  /// succeed. Also note that the login module specific options,
  /// useTicketCache="true" and ticketCache="${System.Enviroment.UserName}",
  /// are passed to the "MyLoginModule". These options instruct the
  /// "MyLoginModule" to use the ticket cache at the specified location. The
  /// system property "System.Enviroment.CurrentDirectory" is expanded to it
  /// respective value.
  /// </para>
  /// </remarks>
  public interface ILoginConfiguration : IMustConfiguration
  {
    /// <summary>
    /// Gets the login modules that was configured for this application.
    /// </summary>
    /// <remarks>
    /// If this application has no login modules configured, this property will
    /// returns a empty <see cref="LoginModulesNode"/>, that is a
    /// <see cref="LoginModulesNode"/> object that contains no login modules.
    /// </remarks>
    ILoginModulesNode LoginModules { get; }
  }
}
