using System;
using System.Data;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Describes a clone of a transaction providing guarantee that the
  /// transaction cannot be committed until the application comes to rest
  /// regarding work on the transaction.
  /// </summary>
  /// <remarks>
  /// A <see cref="DependentTransaction"/> cannot be rolled back or be commited,
  /// so will can use it inside a using block, without causing any interference
  /// on the parent transaction.
  /// </remarks>
  public sealed class DependentTransaction : ITransaction
  {
    readonly Transaction parent_transaction_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="DependentTransaction"/>
    /// class by using the specified <see cref="IConnectionProvider"/> object.
    /// </summary>
    /// <param name="parent">
    /// The transaction on which this transaction depends on.
    /// </param>
    internal DependentTransaction(Transaction parent) {
      parent_transaction_ = parent;
    }
    #endregion

    public void Commit() {
    }

    public void Dispose() {
    }

    public int ExecuteNonQuery(IDbCommand cmd) {
      return parent_transaction_.ExecuteNonQuery(cmd);
    }

    public IDataReader ExecuteReader(IDbCommand cmd, CommandBehavior behavior) {
      return parent_transaction_.ExecuteReader(cmd, behavior);
    }

    public IDataReader ExecuteReader(IDbCommand cmd) {
      return parent_transaction_.ExecuteReader(cmd);
    }

    public object ExecuteScalar(IDbCommand cmd) {
      return parent_transaction_.ExecuteScalar(cmd);
    }

    public CommandBuilder GetCommandBuilder() {
      return parent_transaction_.GetCommandBuilder();
    }

    public void Rollback() {
    }
  }
}
