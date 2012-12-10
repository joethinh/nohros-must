using System;
using System.Runtime.Serialization;

namespace Nohros
{
  /// <summary>
  /// Exception throw by Repositories when an exception occur.
  /// </summary>
  /// <remarks>
  /// A repository is a provider that provides data.
  /// </remarks>
  public class RepositoryException : ProviderException
  {
    #region .ctor
    /// <summary>
    /// Creates a new instance of the <see cref="RepositoryException"/> class.
    /// </summary>
    public RepositoryException() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RepositoryException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="RepositoryException"/>was throw</param>
    public RepositoryException(string message) : base(message) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RepositoryException"/> class.
    /// </summary>
    /// <param name="inner_exception">
    /// The exception that caused this <see cref="RepositoryException"/> to be
    /// throw.
    /// </param>
    public RepositoryException(Exception inner_exception)
      : base(inner_exception) {
    }

    /// <summary>
    /// Creates a new instance if the <see cref="RepositoryException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the information to
    /// deserialize.</param>
    /// <param name="context">Contextual information about the source or
    /// destination.</param>
    protected RepositoryException(SerializationInfo info,
      StreamingContext context) : base(info, context) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RepositoryException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="RepositoryException"/>was throw.</param>
    /// <param name="inner_exception">The exception that caused this
    /// <see cref="RepositoryException"/>to be throw.</param>
    public RepositoryException(string message, System.Exception inner_exception)
      : base(message, inner_exception) {
    }
    #endregion
  }
}
