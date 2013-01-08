using System;
using System.Data;
using Nohros.Resources;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Represents a transaction.
  /// </summary>
  public interface ITransaction : IDisposable
  {
    /// <summary>
    /// Attempts to commit the transaction.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The transaction is already completed.
    /// </exception>
    /// <exception cref="ProviderException">
    /// The transaction cannot be commited on the server.
    /// </exception>
    void Commit();

    /// <summary>
    /// Rolls back(aborts) the transaction.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The transaction was already completed.
    /// </exception>
    /// <exception cref="ProviderException">
    /// The transaction cannot be rolled back on the server.
    /// </exception>
    void Rollback();

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
    object ExecuteScalar(IDbCommand cmd);

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
    int ExecuteNonQuery(IDbCommand cmd);

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
    IDataReader ExecuteReader(IDbCommand cmd);

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
    IDataReader ExecuteReader(IDbCommand cmd, CommandBehavior behavior);

    /// <summary>
    /// Gets a <see cref="CommandBuilder"/> that can build
    /// <see cref="IDbCommand"/> that is associated with a
    /// <see cref="ITransaction"/>
    /// </summary>
    /// <returns></returns>
    CommandBuilder GetCommandBuilder();
  }
}
