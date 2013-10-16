using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Implementations of this interface are passed to a
  /// <see cref="IAuthCallbackHandler"/>, allowing underlying security services
  /// the ability to interact with a calling application to retrieve specific
  /// authentication data such usernames and passwords, or to display certain
  /// information, such as error and warning messages.
  /// <para>
  /// <see cref="IAuthCallback"/> implementations do not retrieve or display
  /// the information requested by underlying security services, it simply
  /// provide means to pass such request to applications, and for applications,
  /// if appropriate, to return requested information back to the
  /// underlying security services.
  /// </para>
  /// </summary>
  public interface IAuthCallback
  {
  }
}
