using System;
using System.Data;

namespace Nohros.Data.Providers
{
  public sealed class TransactionContext : ITransactionContext
  {
    readonly IDbTransaction transaction_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionContext"/>
    /// using the specified <see cref="IDbTransaction"/>.
    /// </summary>
    /// <param name="transaction"></param>
    internal TransactionContext(IDbTransaction transaction) {
      transaction_ = transaction;
    }
    #endregion

    public void Complete() {
      transaction_.Commit();
    }

    public void Dispose() {
      transaction_.Rollback();
      transaction_.Dispose();
      TransactionContexts.Current = null;
    }

    /// <summary>
    /// Gets the <see cref="IDbTransaction"/> object that is associated with
    /// the context.
    /// </summary>
    public IDbTransaction Transaction {
      get { return transaction_; }
    }
  }
}
