using System;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Describes a clone of a transaction providing guarantee that the
  /// transaction cannot be committed until the application comes to rest
  /// regarding work on the transaction.
  /// </summary>
  public sealed class DependentTransaction : Transaction
  {
    Transaction parent_transaction_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="DependentTransaction"/>
    /// class by using the specified <see cref="IConnectionProvider"/> object.
    /// </summary>
    /// <param name="connection_provider">
    /// A <see cref="IConnectionProvider"/> that provide access to the
    /// underlying data provider.
    /// </param>
    /// <param name="parent_transaction">
    /// The transaction on which this transacton depends on.
    /// </param>
    internal DependentTransaction(IConnectionProvider connection_provider,
      Transaction parent_transaction)
      : base(connection_provider) {
      parent_transaction_ = parent_transaction;
    }
    #endregion
  }
}
