using System;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// The default implementation of the <see cref="ITransactionContext"/>.
  /// </summary>
  public sealed class TransactionContext : ITransactionContext
  {
    bool complete_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionContext"/>
    /// class by using the specified <see cref="IConnectionProvider"/> object.
    /// </summary>
    public TransactionContext(IConnectionProvider connection_provider) {
      Transaction.Current = new Transaction(connection_provider);
      complete_ = false;
    }
    #endregion

    public void Dispose() {
      Transaction transaction = Transaction.Current;
      if (transaction != null && !complete_) {
        transaction.Rollback();
        transaction.Dispose();
      }
    }


    /// <inheritdoc/>
    public void Complete() {
      Transaction transaction = Transaction.Current;
      if (transaction == null) {
        throw new InvalidOperationException(
          Resources.Resources.DataProvider_InvalidOperation_NoTransaction);
      }
      if (complete_) {
        throw new InvalidOperationException(
          Resources.Resources.InvalidOperation_CalledMoreThanOnce);
      }
      complete_ = true;
      transaction.Commit();
    }
  }
}
