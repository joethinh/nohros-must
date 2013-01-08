using System;
using System.Data;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Defines a <see cref="ITransaction"/> that is managed by the underlying
  /// <see cref="IConnectionProvider"/>.
  /// </summary>
  /// <remarks>
  /// A explicit <see cref="IDbTransaction"/> is not created by this class. The
  /// creation of a <see cref="IDbTransaction"/> is delegated to the underlying
  /// <see cref="IConnectionProvider"/> that has the option to create or not
  /// a <see cref="IDbTransaction"/> object.
  /// </remarks>
  public class ImplicitTransaction : ITransaction
  {
    readonly IConnectionProvider connection_provider_;
    IDbConnection connection_;
    TransactionBehavior transaction_behavior_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ImplicitTransaction"/>
    /// </summary>
    /// <param name="connection_provider"></param>
    public ImplicitTransaction(IConnectionProvider connection_provider)
      : this(connection_provider, TransactionBehavior.Default) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImplicitTransaction"/>
    /// </summary>
    /// <param name="connection_provider"></param>
    /// <param name="behavior"></param>
    public ImplicitTransaction(IConnectionProvider connection_provider,
      TransactionBehavior behavior) {
      connection_provider_ = connection_provider;
      connection_ = null;
      transaction_behavior_ = behavior;
    }
    #endregion

    public void Dispose() {
      if (connection_ != null &&
        transaction_behavior_ == TransactionBehavior.Default) {
        connection_.Close();
        connection_.Dispose();
      }
    }

    public void Commit() {
      // The transaction is auto commited, so this method should do nothing.
    }

    public void Rollback() {
      // The transaction is auto rolled back, so this method should do nothing.
    }

    public object ExecuteScalar(IDbCommand cmd) {
      return Execute(cmd, cmd.ExecuteScalar);
    }

    public int ExecuteNonQuery(IDbCommand cmd) {
      return Execute(cmd, cmd.ExecuteNonQuery);
    }

    public IDataReader ExecuteReader(IDbCommand cmd) {
      if (transaction_behavior_ == TransactionBehavior.Default) {
        return ExecuteReader(cmd, CommandBehavior.CloseConnection);
      }
      return ExecuteReader(cmd, CommandBehavior.Default);
    }

    public IDataReader ExecuteReader(IDbCommand cmd, CommandBehavior behavior) {
      return Execute(cmd, delegate { return cmd.ExecuteReader(behavior); });
    }

    public CommandBuilder GetCommandBuilder() {
      if (connection_ == null) {
        connection_ = connection_provider_.CreateConnection();
      }
      return new CommandBuilder(connection_);
    }

    T Execute<T>(IDbCommand cmd, CallableDelegate<T> runnable) {
      if (connection_ == null) {
        connection_ = connection_provider_.CreateConnection();
      }
      if (connection_.State != ConnectionState.Open) {
        connection_.Open();
      }
      cmd.Connection = connection_;
      return runnable();
    }
  }
}
