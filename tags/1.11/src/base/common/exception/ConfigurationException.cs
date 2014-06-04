using System;
using System.Runtime.Serialization;

namespace Nohros.Configuration
{
  /// <summary>
  /// The exception that is thrown when a configuration error has occured.
  /// </summary>
  [Serializable]
  public class ConfigurationException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationException"/>
    /// class.
    /// </summary>
    public ConfigurationException() {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationException"/>
    /// class with a specified error message.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public ConfigurationException(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationException"/>
    /// class with a specified error message and a reference to the inner
    /// exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// </param>
    /// <param name="inner_exception">
    /// The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.
    /// </param>
    public ConfigurationException(string message, Exception inner_exception)
      : base(message, inner_exception) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationException"/>
    /// class with a specified error message and a reference to the inner
    /// exception that is the cause of this exception.
    /// </summary>
    /// <param name="inner_exception">
    /// The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.
    /// </param>
    public ConfigurationException(Exception inner_exception)
      : base(
        "There is an error in the application configuration", inner_exception) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationException"/>
    /// class with serialized data.
    /// </summary>
    /// <param name="context">
    /// The <see cref="StreamingContext"/> that contains contextual information
    /// about the source or destination.
    /// </param>
    /// <param name="info">The <see cref="SerializationInfo"/> that holds the
    /// serialized object data about the exception being thrown.
    /// </param>
    /// <exception cref="SerializationException">The class name is null or
    /// <see cref="Exception.HResult"/> is zero (0).
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The info parameter is null.
    /// </exception>
    protected ConfigurationException(SerializationInfo info,
      StreamingContext context) : base(info, context) {
    }
  }
}
