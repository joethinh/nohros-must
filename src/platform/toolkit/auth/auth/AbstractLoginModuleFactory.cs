using System;
using System.Collections.Generic;
using Nohros.Configuration;
using Nohros.Extensions;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A generic implementation of the <see cref="ILoginModuleFactory"/>
  /// interface that reduces the effort required to implement the
  /// <see cref="ILoginModuleFactory"/> interface.
  /// </summary>
  /// <remarks>
  /// <see cref="AbstractLoginModuleFactory"/> a "controlFlag" option to
  /// be present in the configured provider options.
  /// </remarks>
  public abstract class AbstractLoginModuleFactory : ILoginModuleFactory
  {
    public const string kControlFlagOption = "controlFlag";

    /// <inheritdoc/>
    public abstract ILoginModule CreateLoginModule(
      Subject subject,
      IAuthCallbackHandler callback,
      IDictionary<string, string> shared,
      IDictionary<string, string> options);

    protected LoginModuleControlFlag GetControlFlag(
      IDictionary<string, string> options) {
      string option = options.GetString(kControlFlagOption);
      switch (option.ToLower()) {
        case "optional":
          return LoginModuleControlFlag.Optional;

        case "required":
          return LoginModuleControlFlag.Required;

        case "requisite":
          return LoginModuleControlFlag.Requisite;

        case "sufficient":
          return LoginModuleControlFlag.Sufficient;

        default:
          return LoginModuleControlFlag.Required;
      }
    }
  }
}
