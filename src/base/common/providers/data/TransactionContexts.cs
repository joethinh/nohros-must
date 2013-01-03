using System;
using System.Data;

namespace Nohros.Data.Providers
{
  public sealed class TransactionContexts
  {
    [ThreadStatic] static ITransactionContext current_transaction_context_;

    /// <summary>
    /// Gets or sets the current transaction context.
    /// </summary>
    /// <remarks>
    /// Although you can set the current transaction using this property, you
    /// should use a <see cref="ITransactionContext"/> object to manipulate the
    /// current transaction whenever possible.
    /// <para>
    /// This property is thread static.
    /// </para>
    /// </remarks>
    public static ITransactionContext Current {
      get { return current_transaction_context_; }
      set { current_transaction_context_ = value; }
    }
  }
}
