using System;
using System.Runtime.Serialization;

namespace Nohros
{
  /// <summary>
  /// Thrown by a data provider when a query that expects results is executed
  /// and there is no result to return.
  /// </summary>
  [Serializable]
  public class NoResultException : RepositoryException
  {
    #region .ctor
    /// <summary>
    /// Creates a new instance of the <see cref="NoResultException"/> class.
    /// </summary>
    public NoResultException() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoResultException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="NoResultException"/>was throw</param>
    public NoResultException(string message) : base(message) {
    }

    /// <summary>
    /// Creates a new instance if the <see cref="NoResultException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the information to
    /// deserialize.</param>
    /// <param name="context">Contextual information about the source or
    /// destination.</param>
    protected NoResultException(SerializationInfo info, StreamingContext context)
      : base(info, context) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoResultException"/> class.
    /// </summary>
    /// <param name="inner_exception">
    /// The exception that caused this <see cref="NoResultException"/> to be
    /// throw.
    /// </param>
    public NoResultException(Exception inner_exception) : base(inner_exception) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoResultException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="NoResultException"/>was throw.</param>
    /// <param name="inner_exception">The exception that caused this
    /// <see cref="NoResultException"/>to be throw.</param>
    public NoResultException(string message, Exception inner_exception)
      : base(message, inner_exception) {
    }
    #endregion
  }
}
