using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Factory and utility methods for <see cref="IAuthCallbackHandler"/>
  /// </summary>
  public sealed class AuthCallbackHandlers
  {
    /// <summary>
    /// Creates a <see cref="IAuthCallbackHandler"/> that does nothing.
    /// </summary>
    /// <returns>
    /// A <see cref="IAuthCallbackHandler"/> that does nothing.
    /// </returns>
    public static IAuthCallbackHandler Nop() {
      return new NopAuthCallbackHandler();
    }

    /// <summary>
    /// Creates a <see cref="IAuthCallbackHandler"/> that throws a
    /// <see cref="NotSupportedException"/> exception when
    /// <see cref="IAuthCallbackHandler.Handle"/> method is executed.
    /// </summary>
    public static IAuthCallbackHandler Throwable() {
      return new ThrowableAuthCallbackHandler();
    }

    /// <summary>
    /// Creates a <see cref="IAuthCallbackHandler"/> that executes the given
    /// <see cref="Action{T}"/> when the
    /// <see cref="IAuthCallbackHandler.Handle"/> method is executed.
    /// </summary>
    /// <param name="actionable">
    /// The <see cref="Action{T}"/> that should be executed when the
    /// <see cref="IAuthCallbackHandler.Handle"/> is called.
    /// </param>
    public static IAuthCallbackHandler Actionable(
      Action<IAuthCallback[]> actionable) {
      return new ActionableAuthCallbackHandler(actionable);
    }
  }
}
