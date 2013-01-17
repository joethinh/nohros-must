using System;
using System.Collections.Generic;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// The default implementation of the <see cref="ITransactionContext"/>.
  /// </summary>
  public sealed class TransactionContext : ITransactionContext
  {
    [ThreadStatic] static TransactionContext current_transaction_context_;

    readonly IDictionary<string, InternalTransaction> transactions_;
    bool complete_;

    #region .ctor
    static TransactionContext() {
      current_transaction_context_ = null;
    }
    #endregion

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionContext"/>
    /// class by using the specified <see cref="IConnectionProvider"/> object.
    /// </summary>
    public TransactionContext() {
      current_transaction_context_ = this;
      complete_ = false;
      transactions_ = new Dictionary<string, InternalTransaction>();
    }
    #endregion

    public void Dispose() {
      if (!complete_) {
        foreach (KeyValuePair<string, InternalTransaction> pair in transactions_) {
            InternalTransaction transaction = pair.Value;
          transaction.Rollback();
          transaction.Dispose();
        }
      }
      TransactionContext.Current = null;
    }

    /// <inheritdoc/>
    public void Complete() {
      if (complete_) {
        throw new InvalidOperationException(
          Resources.Resources.InvalidOperation_CalledMoreThanOnce);
      }
      complete_ = true;
      foreach (KeyValuePair<string, InternalTransaction> pair in transactions_) {
        InternalTransaction transaction = pair.Value;
        try {
          transaction.Commit();
        } catch {
          // Make sure that one transaction does not interfere in another
        }
      }
      transactions_.Clear();
    }

    /// <summary>
    /// Enlist a transaction within the current transaction context.
    /// </summary>
    /// <remarks>
    /// If a transaction context does not exists this method performs no
    /// operation.
    /// </remarks>
    internal static bool TryGetInternalTransaction(string key,
      out InternalTransaction transaction) {
      if (current_transaction_context_ != null) {
        return current_transaction_context_.transactions_.TryGetValue(key,
          out transaction);
      }
      transaction = null;
      return false;
    }

    internal static void Enlist(string key, InternalTransaction transaction) {
      if (current_transaction_context_ != null) {
        current_transaction_context_.transactions_.Add(key, transaction);
      }
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
    public static TransactionContext Current {
      get { return current_transaction_context_; }
      set { current_transaction_context_ = value; }
    }
  }
}
