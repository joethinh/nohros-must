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
  public class TransactionContext : ITransactionContext, IDisposable
  {
    const string kClassName = "Nohros.Data.Providers.TransactionContext";

    readonly IList<IDbCommand> commands_;
    readonly IDbConnection connection_;
    readonly MustLogger logger_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionContext"/>
    /// using the specified <see cref="IDbConnection"/> object.
    /// </summary>
    /// <param name="connection"></param>
    protected TransactionContext(IDbConnection connection) {
      connection_ = connection;
      commands_ = new List<IDbCommand>();
      logger_ = MustLogger.ForCurrentProcess;
    }
    #endregion

    /// <inheritdoc/>
    public void Dispose() {
      foreach (IDbCommand command in commands_) {
        command.Dispose();
      }
    }

    /// <inheritdoc/>
    public virtual void Complete() {
      bool should_close_connection = false;
      try {
        if (connection_.State != ConnectionState.Open) {
          should_close_connection = true;
          connection_.Open();
        }

        IDbTransaction transaction = null;
        try {
          using (transaction = connection_.BeginTransaction()) {
            foreach (IDbCommand command in commands_) {
              command.Transaction = transaction;
              command.ExecuteNonQuery();
            }

            // Attempt to commit the transaction. Both commit and rollback
            // operations should be enclosed by a try/catch block because they
            // could generate a Exception if the connection is terminated or
            // if the transaction has already been rolled back on the server.
            transaction.Commit();
          }
        } catch (Exception e) {
          try {
            if (transaction != null) {
              transaction.Rollback();
            }
          } catch (Exception ie) {
            LogError(ie, "Compete");
          }
          throw new ProviderException(e);
        }
      } catch (Exception e) {
        LogError(e, "Compete");
        throw new ProviderException(e);
      } finally {
        if (should_close_connection) {
          connection_.Close();
        }
      }
    }

    void LogError(Exception e, string method) {
      logger_.Error(string.Format(StringResources.Log_MethodThrowsException,
        method, kClassName), e);
    }

    /// <summary>
    /// Enlists a command to participate in a transaction.
    /// </summary>
    /// <remarks>
    /// A <see cref="IDbCommand"/> that participates in a transaction.
    /// <para>
    /// The returned <see cref="IDbCommand"/> will be attached to a transaction
    /// at the time the <see cref="Complete"/> method runs.
    /// </para>
    /// <para>
    /// The returned <see cref="IDbCommand"/> will be associated with the
    /// same <see cref="IDbConnection"/> that was specified at the construction
    /// time.
    /// </para>
    /// </remarks>
    public IDbCommand Enlist() {
      var command = connection_.CreateCommand();
      commands_.Add(command);
      return command;
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
