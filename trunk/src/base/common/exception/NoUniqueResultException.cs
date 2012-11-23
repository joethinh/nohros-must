using System;
using System.Runtime.Serialization;

namespace Nohros
{
  /// <summary>
  /// Thrown by a data provider when a query that expects a single results is
  /// executed and there is more than one result from the query.
  /// </summary>
  public class NoUniqueResultException : ProviderException
  {
    #region .ctor
    /// <summary>
    /// Creates a new instance of the <see cref="NoUniqueResultException"/> class.
    /// </summary>
    public NoUniqueResultException() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoUniqueResultException"/> class.
    /// </summary>
    /// <param name="message">
    /// A message describing why this <see cref="NoUniqueResultException"/>was
    /// throw.
    /// </param>
    public NoUniqueResultException(string message) : base(message) {
    }

    /// <summary>
    /// Creates a new instance if the <see cref="NoUniqueResultException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the information to
    /// deserialize.</param>
    /// <param name="context">Contextual information about the source or
    /// destination.</param>
    protected NoUniqueResultException(SerializationInfo info,
      StreamingContext context)
      : base(info, context) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoUniqueResultException"/> class.
    /// </summary>
    /// <param name="inner_exception">
    /// The exception that caused this <see cref="NoUniqueResultException"/> to be
    /// throw.
    /// </param>
    public NoUniqueResultException(Exception inner_exception)
      : base(inner_exception) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoUniqueResultException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="NoUniqueResultException"/>was throw.</param>
    /// <param name="inner_exception">The exception that caused this
    /// <see cref="NoUniqueResultException"/>to be throw.</param>
    public NoUniqueResultException(string message, Exception inner_exception)
      : base(message, inner_exception) {
    }
    #endregion
  }
}
