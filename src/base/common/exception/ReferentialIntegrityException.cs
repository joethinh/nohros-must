using System;
using System.Runtime.Serialization;

namespace Nohros
{
  /// <summary>
  /// Exception throws when referential integrity is violated.
  /// </summary>
  public class ReferentialIntegrityException : RepositoryException
  {
    #region .ctor
    /// <summary>
    /// Creates a new instance of the <see cref="ReferentialIntegrityException"/> class.
    /// </summary>
    public ReferentialIntegrityException() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ReferentialIntegrityException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="ReferentialIntegrityException"/>was throw</param>
    public ReferentialIntegrityException(string message) : base(message) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ReferentialIntegrityException"/> class.
    /// </summary>
    /// <param name="inner_exception">
    /// The exception that caused this <see cref="ReferentialIntegrityException"/> to be
    /// throw.
    /// </param>
    public ReferentialIntegrityException(Exception inner_exception)
      : base(inner_exception) {
    }

    /// <summary>
    /// Creates a new instance if the <see cref="ReferentialIntegrityException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the information to
    /// deserialize.</param>
    /// <param name="context">Contextual information about the source or
    /// destination.</param>
    protected ReferentialIntegrityException(SerializationInfo info,
      StreamingContext context) : base(info, context) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ReferentialIntegrityException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="ReferentialIntegrityException"/>was throw.</param>
    /// <param name="inner_exception">The exception that caused this
    /// <see cref="ReferentialIntegrityException"/>to be throw.</param>
    public ReferentialIntegrityException(string message,
      Exception inner_exception) : base(message, inner_exception) {
    }
    #endregion
  }
}
