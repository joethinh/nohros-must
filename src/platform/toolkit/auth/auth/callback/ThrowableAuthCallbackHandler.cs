using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A implementation of the <see cref="IAuthCallbackHandler"/> that always
  /// throw an <see cref="NotSupportedException"/>.
  /// </summary>
  public class ThrowableAuthCallbackHandler : IAuthCallbackHandler
  {
    /// <inheritdoc/>
    /// <remarks>
    /// Throws an <see cref="NotSupportedException"/>.
    /// </remarks>
    public void Handle(IAuthCallback[] callback) {
      throw new NotSupportedException();
    }
  }
}
