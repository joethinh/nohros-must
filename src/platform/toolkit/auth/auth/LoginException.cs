using System;
using System.Runtime.Serialization;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Defines a base class for predefined exceptions in the
  /// <see cref="Nohros.Security.Auth"/> namespace.
  /// </summary>
  [Serializable]
  public class LoginException : Exception
  {
    public LoginException() : base() {
    }

    public LoginException(string message) : base(message) {
    }

    public LoginException(SerializationInfo info, StreamingContext context)
      : base(info, context) {
    }

    public LoginException(string message, Exception innerException)
      : base(message, innerException) {
    }
  }
}
