using System;
using System.Data;
using Nohros.Logging;
using Nohros.Resources;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Represents a transaction.
  /// </summary>
  public class Transaction : ITransaction, IDisposable
  {
    const string kClassName = "Nohros.Data.Providers.Transaction";

    [ThreadStatic] static Transaction current_transaction_;

    readonly IConnectionProvider connection_provider_;
    readonly MustLogger logger_;
    bool complete_;
    IDbConnection connection_;
    IDbTransaction transaction_;
    TransactionBehavior transaction_behavior_;

    #region .ctor
    static Transaction() {
      current_transaction_ = null;
    }
    #endregion

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Transaction"/>
    /// class by using the specified <see cref="IConnectionProvider"/> object.
    /// </summary>
    /// <param name="connection_provider">
    /// A <see cref="IConnectionProvider"/> that provide access to the
    /// underlying data provider.
    /// </param>
    public Transaction(IConnectionProvider connection_provider)
      : this(connection_provider, TransactionBehavior.Default) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Transaction"/>
    /// class by using the specified <see cref="IConnectionProvider"/> object
    /// and <see cref="TransactionBehavior"/>.
    /// </summary>
    /// <param name="connection_provider">
    /// A <see cref="IConnectionProvider"/> that provice access to the
    /// underlying data provider.
    /// </param>
    /// <param name="behavior">
    /// A <see cref="TransactionBehavior"/> that defines the effects of the
    /// transaction on the associated connection.
    /// </param>
    public Transaction(IConnectionProvider connection_provider,
      TransactionBehavior behavior) {
      connection_provider_ = connection_provider;
      logger_ = MustLogger.ForCurrentProcess;
      complete_ = false;
      transaction_behavior_ = behavior;
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

      if (connection_ != null &&
        transaction_behavior_ == TransactionBehavior.Default) {
        connection_.Close();
        connection_.Dispose();
      }
    }

    /// <summary>
    /// Attempts to commit the transaction.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The transaction is already completed.
    /// </exception>
    /// <exception cref="ProviderException">
    /// The transaction cannot be commited on the server.
    /// </exception>
    public virtual void Commit() {
      if (complete_) {
        throw new InvalidOperationException(
          Resources.Resources.DataProvider_InvalidOperation_TransactionCompleted);
      }

      try {
        complete_ = true;
        transaction_.Commit();
      } catch (Exception ex) {
        throw new ProviderException(ex);
      }
    }

    /// <summary>
    /// Rolls back(aborts) the transaction.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The transaction was already completed.
    /// </exception>
    /// <exception cref="ProviderException">
    /// The transaction cannot be rolled back on the server.
    /// </exception>
    public virtual void Rollback() {
      if (complete_) {
        throw new InvalidOperationException(
          Resources.Resources.DataProvider_InvalidOperation_TransactionCompleted);
      }

      try {
        transaction_.Rollback();
      } catch (Exception e) {
        throw new ProviderException(e);
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
    public virtual object ExecuteScalar(IDbCommand cmd) {
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
    public virtual int ExecuteNonQuery(IDbCommand cmd) {
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
    public virtual IDataReader ExecuteReader(IDbCommand cmd) {
      if (transaction_behavior_ == TransactionBehavior.Default) {
        return ExecuteReader(cmd, CommandBehavior.CloseConnection);
      }
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
    public virtual IDataReader ExecuteReader(IDbCommand cmd,
      CommandBehavior behavior) {
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
        try {
          connection_ = connection_provider_.CreateConnection();
          connection_.Open();
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
          connection_ = null;

          throw e;
        }
      }
      if (connection_.State != ConnectionState.Open) {
        connection_.Open();
      }
      cmd.Connection = connection_;
      return runnable();
    }

    /// <summary>
    /// Gets or sets the ambient transaction.
    /// </summary>
    /// <remarks>
    /// Although you can set the ambient transaction using this property, you
    /// should use the <see cref="TransactionContext"/> object to manipulate
    /// the ambient transaction whenever possible.
    /// <para>
    /// This property is thread static.
    /// </para>
    /// </remarks>
    public static Transaction Current {
      get { return current_transaction_; }
      set { current_transaction_ = value; }
    }
  }
}
