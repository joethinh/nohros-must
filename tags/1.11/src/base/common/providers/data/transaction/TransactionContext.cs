using System;
using System.Collections.Generic;
using System.Data;
using Nohros.Logging;
using Nohros.Resources;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// An simPle implementation of the <see cref="ITransactionContext"/> class.
  /// </summary>
  public class TransactionContext : ITransactionContext
  {
    const string kClassName = "Nohros.Data.Providers.TransactionContext";

    readonly Queue<EnlistedCommand> commands_;
    readonly IDbConnection connection_;
    readonly MustLogger logger_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionContext"/>
    /// using the specified <see cref="IDbConnection"/> object.
    /// </summary>
    /// <param name="connection"></param>
    public TransactionContext(IDbConnection connection) {
      if (connection == null) {
        throw new ArgumentNullException("connection");
      }
      connection_ = connection;
      commands_ = new Queue<EnlistedCommand>();
      logger_ = MustLogger.ForCurrentProcess;
    }
    #endregion

    /// <inheritdoc/>
    public virtual void Complete() {
      bool should_close_connection = false;
      try {
        if (connection_.State != ConnectionState.Open) {
          should_close_connection = true;
          connection_.Open();
        }

        IDbTransaction transaction = null;
        using (transaction = connection_.BeginTransaction()) {
          while (commands_.Count > 0) {
            EnlistedCommand enlisted = commands_.Dequeue();
            IDbCommand command = enlisted.Prepare(this);
            command.Transaction = transaction;
            enlisted.Execute(command);
            command.Dispose();
          }

          // Attempt to commit the transaction. Both commit and rollback
          // operations should be enclosed by a try/catch block because they
          // could generate a Exception if the connection is terminated or
          // if the transaction has already been rolled back on the server.
          try {
            transaction.Commit();
          } catch (Exception e) {
            try {
              transaction.Rollback();
            } catch (Exception ie) {
              LogError(ie, "Compete");
            }
            LogError(e, "Compete");
          }
        }
      } finally {
        if (should_close_connection) {
          connection_.Close();
        }
        commands_.Clear();
      }
    }

    public void Enlist(PrepareCommandDelegate preparing) {
      Enlist(preparing, delegate(IDbCommand command) {
        command.ExecuteNonQuery();
      });
    }

    /// <inheritdoc/>
    public void Enlist(PrepareCommandDelegate command,
      ExecuteCommandDelegate executor) {
      var enlisted = new EnlistedCommand(delegate(ITransactionContext context)
      {
        var cmd = command(context);
        if (cmd.Connection != connection_) {
          throw new ArgumentException(
            Resources.Resources.Arg_TransactionContext_Command_Connection);
        }
        return cmd;
      }, executor);
      commands_.Enqueue(enlisted);
    }

    /// <summary>
    /// Creates a command that is related with the same connection that is
    /// associated with the <see cref="ITransactionContext"/>.
    /// </summary>
    /// <remarks>
    /// A <see cref="IDbCommand"/> that associated with the
    /// <see cref="ITransactionContext"/>.
    /// </remarks>
    public IDbCommand CreateCommand() {
      return connection_.CreateCommand();
    }

    /// <inheritdoc/>
    public void Enlist(IDbCommand command) {
      Enlist(delegate { return command; });
    }

    void LogError(Exception e, string method) {
      logger_.Error(string.Format(StringResources.Log_MethodThrowsException,
        method, kClassName), e);
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
