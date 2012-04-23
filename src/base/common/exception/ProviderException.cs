using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Nohros
{
  /// <summary>
  /// This exception is throw when a provider instance could not be created or
  /// its type could not be resolved.
  /// </summary>
  public class ProviderException: Exception
  {
    /// <summary>
    /// Creates a new instance_ of the <see cref="ProviderException"/> class.
    /// </summary>
    public ProviderException() {
    }

    /// <summary>
    /// Creates a new instance_ of the <see cref="ProviderException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="ProviderException"/>was throw</param>
    public ProviderException(string message)
      : base(message) {
    }

    /// <summary>
    /// Creates a new instance_ if the <see cref="ProviderException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the information to
    /// deserialize.</param>
    /// <param name="context">Contextual information about the source or
    /// destination.</param>
    protected ProviderException(SerializationInfo info,
      StreamingContext context) : base(info, context) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProviderException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="ProviderException"/>was throw.</param>
    /// <param name="innerException">The exception that caused this
    /// <see cref="ProviderException"/>to be throw.</param>
    public ProviderException(string message, System.Exception innerException)
      : base(message, innerException) {
    }
  }
}
