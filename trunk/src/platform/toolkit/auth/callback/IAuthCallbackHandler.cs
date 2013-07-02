using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// An application implements a <see cref="IAuthCallbackHandler"/> and passes
  /// it to the underlying security services so that they may interact with
  /// the application to retrieve specific authentication data, such as
  /// usernames and passwords, or to display certain information, such error
  /// and warning messages.
  /// <remarks>
  /// <para>
  /// <see cref="IAuthCallbackHandler"/> are implemented in an
  /// application-dependent fashion. For example, implementations for an
  /// application with a graphical user interface (GUI) may pop up windows to
  /// prompt for requested information or to display error messages. An
  /// implementation may also choose to obtain requested information from an
  /// alternate source without asking the end user.
  /// </para>
  /// <para>
  /// Underlying security services make requests for different types of
  /// information by passing individual <see cref="IAuthCallback"/> objects to
  /// the <see cref="IAuthCallbackHandler"/>. The
  /// <see cref="IAuthCallbackHandler"/> implementation decides how to
  /// retrieve and display information depending on the
  /// <see cref="IAuthCallback"/> passed to it.
  /// </para>
  /// </remarks>
  /// </summary>
  public interface IAuthCallbackHandler
  {
    /// <summary>
    /// Retrieve or display information requested in the provided callbacks.
    /// </summary>
    /// <remarks>
    /// The handle method implementation checks the instance(s) of the
    /// <see cref="IAuthCallback"/> objects(s) passed in to retrieve or
    /// display the requested information.
    /// </remarks>
    /// <param name="callback">
    /// An array of <see cref="IAuthCallback"/> objects provided underlying
    /// security service which contains the information requested to be
    /// retrieved or displayed.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// The implementation of this method does not support one or more of
    /// the <see cref="IAuthCallback"/>s specified.
    /// </exception>.
    void Handle(IAuthCallback[] callback);
  }
}