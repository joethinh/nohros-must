using System;
using System.Data;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// An base implementation of the <see cref="ITransactionContext"/> class
  /// that was created to reduce the efforts needed to implement the
  /// <see cref="ITransactionContext"/> class.
  /// </summary>
  public abstract class AbstractTransactionContext : ITransactionContext
  {
    readonly IDbConnection connection_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// </summary>
    /// <param name="connection"></param>
    protected AbstractTransactionContext(IDbConnection connection) {
      connection_ = connection;
    }
    #endregion

    public virtual void Complete() {
    }

    public void Enlist(IDbCommand ) {
    }

    /// <summary>
    /// Gets the <see cref="IDbConnection"/> object tha is associated with the
    /// <see cref="ITransactionContext"/>.
    /// </summary>
    public IDbConnection Connection {
      get { return connection_; }
    }
  }
}
