using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Nohros.Caching
{
  /// <summary>
  /// Throw to indicate that an invalid response was returned from a call to
  /// <see cref="CacheLoader{T}"/>
  /// </summary>
  public class InvalidCacheLoadException : Exception
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Exception"/>
    /// class.
    /// </summary>
    public InvalidCacheLoadException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Exception"/>
    /// class with a specified error message.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public InvalidCacheLoadException(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Exception"/>
    /// class with a specified error message and a reference to the inner
    /// exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// </param>
    /// <param name="inner_exception">The exception that is the cause of the
    /// current exception, or a null reference (Nothing in Visual Basic) if no
    /// inner exception is specified.
    /// </param>
    public InvalidCacheLoadException(string message, Exception inner_exception)
      : base(message, inner_exception) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Exception"/>
    /// class with serialized data.
    /// </summary>
    /// <param name="context">The
    /// <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
    /// contains contextual information about the source or destination.
    /// </param>
    /// <param name="info">The
    /// <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that
    /// holds the serialized object data about the exception being thrown.
    /// </param>
    /// <exception cref="T:System.Runtime.Serialization.SerializationException">
    /// The class name is null or <see cref="P:System.Exception.HResult"/> is
    /// zero (0).
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">The info parameter is
    /// null.
    /// </exception>
    protected InvalidCacheLoadException(SerializationInfo info,
      StreamingContext context) : base(info, context) {
    }
    #endregion
  }
}
