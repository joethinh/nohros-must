using System;
using System.Data;
using Nohros.Logging;
using Nohros.Resources;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Represents a transaction.
  /// </summary>
  internal class InternalTransaction : ITransaction, IDisposable
  {
    const string kClassName = "Nohros.Data.Providers.InternalTransaction";

    readonly IDbConnection connection_;

    readonly MustLogger logger_;
    bool complete_, disposed_;
    IDbTransaction transaction_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Transaction"/>
    /// class by using the specified <see cref="IConnectionProvider"/> object
    /// and <see cref="TransactionBehavior"/>.
    /// </summary>
    /// <param name="connection">
    /// A <see cref="IDbConnection"/> that provide access to the
    /// underlying data provider.
    /// </param>
    public InternalTransaction(IDbConnection connection) {
      logger_ = MustLogger.ForCurrentProcess;
      complete_ = false;
      connection_ = connection;
      disposed_ = false;
    }
    #endregion

    public void Dispose() {
      if (!complete_) {
        try {
          if (transaction_ != null) {
            transaction_.Rollback();
            transaction_.Dispose();
          }
        } catch (Exception e) {
          logger_.Warn(string.Format(StringResources.Log_MethodThrowsException,
            "Dispose", kClassName), e);
        }
      }

      if (connection_ != null && !disposed_) {
        connection_.Close();
        connection_.Dispose();
      }

      disposed_ = true;
    }

    /// <summary>
    /// Attempts to commit the transaction.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The transaction is already completed.
    /// </exception>
    public void Commit() {
      if (!complete_) {
        try {
          complete_ = true;
          transaction_.Commit();
        } catch (Exception e) {
          logger_.Error(string.Format(StringResources.Log_MethodThrowsException,
            "Commit", kClassName), e);
        }
      }
    }

    /// <summary>
    /// Rolls back(aborts) the transaction.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The transaction was already completed.
    /// </exception>
    public void Rollback() {
      if (!complete_) {
        try {
          complete_ = true;
          transaction_.Rollback();
        } catch (Exception e) {
          logger_.Error(string.Format(StringResources.Log_MethodThrowsException,
            "Rollback", kClassName), e);
        }
      }
    }

    /// <summary>
    /// Executes the <see cref="IDbCommand.ExecuteScalar"/> against the
    /// specified <paramref name="cmd"/> object under the current transaction.
    /// </summary>
    /// <param name="cmd">
    /// The <see cref="IDbCommand"/> object that should be used to execute the
    /// scalar query.
    /// </param>
    /// <returns>
    /// The result of execution of <see cref="IDbCommand.ExecuteScalar"/> 
    /// against the <paramref name="cmd"/> object.
    /// </returns>
    public object ExecuteScalar(IDbCommand cmd) {
      return Execute(cmd, cmd.ExecuteScalar);
    }

    /// <summary>
    /// Executes the <see cref="IDbCommand.ExecuteNonQuery"/> against the
    /// specified <paramref name="cmd"/> object under the current transaction.
    /// </summary>
    /// <param name="cmd">
    /// The <see cref="IDbCommand"/> object that should be used to execute the
    /// scalar query.
    /// </param>
    /// <returns>
    /// The result of execution of <see cref="IDbCommand.ExecuteNonQuery"/> 
    /// against the <paramref name="cmd"/> object.
    /// </returns>
    public int ExecuteNonQuery(IDbCommand cmd) {
      return Execute(cmd, cmd.ExecuteNonQuery);
    }

    /// <summary>
    /// Executes the <see cref="IDbCommand.ExecuteReader()"/> against the
    /// specified <paramref name="cmd"/> object under the current transaction.
    /// </summary>
    /// <param name="cmd">
    /// The <see cref="IDbCommand"/> object that should be used to execute the
    /// scalar query.
    /// </param>
    /// <returns>
    /// The result of execution of <see cref="IDbCommand.ExecuteReader()"/> 
    /// against the <paramref name="cmd"/> object.
    /// </returns>
    public IDataReader ExecuteReader(IDbCommand cmd) {
      return ExecuteReader(cmd, CommandBehavior.Default);
    }

    /// <summary>
    /// Executes the <see cref="IDbCommand.ExecuteReader()"/> against the
    /// specified <paramref name="cmd"/> object under the current transaction
    /// and using the specified <see cref="CommandBehavior"/>.
    /// </summary>
    /// <param name="cmd">
    /// The <see cref="IDbCommand"/> object that should be used to execute the
    /// scalar query.
    /// </param>
    /// <param name="behavior">
    /// On of the <see cref="CommandBehavior"/> values.
    /// </param>
    /// <returns>
    /// The result of execution of <see cref="IDbCommand.ExecuteReader()"/> 
    /// against the <paramref name="cmd"/> object.
    /// </returns>
    public IDataReader ExecuteReader(IDbCommand cmd,
      CommandBehavior behavior) {
      return Execute(cmd, delegate { return cmd.ExecuteReader(behavior); });
    }

    public CommandBuilder GetCommandBuilder() {
      return new CommandBuilder(connection_);
    }

    public static InternalTransaction Get(IDbConnection connection) {
      InternalTransaction transaction;
      if (TransactionContext.TryGetInternalTransaction(
        connection.ConnectionString, out transaction)) {
        return transaction;
      }
      transaction = new InternalTransaction(connection);
      TransactionContext.Enlist(connection.ConnectionString, transaction);
      return transaction;
    }

    T Execute<T>(IDbCommand cmd, CallableDelegate<T> runnable) {
      if (transaction_ == null) {
        try {
          if (connection_.State != ConnectionState.Open) {
            connection_.Open();
          }
          transaction_ = connection_.BeginTransaction();
        } catch (Exception e) {
          logger_.Error(
            string.Format(StringResources.Log_MethodThrowsException, "Execute",
              kClassName), e);
          // make sure that connection_ and transaction_ is always valid or
          // null
          if (transaction_ != null) {
            transaction_.Dispose();
          }

          if (connection_ != null) {
            connection_.Close();
          }
          transaction_ = null;

          throw e;
        }
      }
      cmd.Connection = connection_;
      cmd.Transaction = transaction_;
      return runnable();
    }

    internal IDbConnection Connection {
      get { return connection_; }
    }
  }
}
