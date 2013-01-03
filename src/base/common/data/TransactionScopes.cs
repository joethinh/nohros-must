using System;
using System.Transactions;

namespace Nohros.Data
{
  /// <summary>
  /// A collection of factory methods for the <see cref="TransactionScopes"/>
  /// class.
  /// </summary>
  public sealed class TransactionScopes
  {
    /// <summary>
    /// Creates a instance of <see cref="TransactionScope"/> class that uses
    /// <see cref="IsolationLevel.ReadCommitted"/> as isolation level.
    /// </summary>
    /// <returns></returns>
    public static TransactionScope ReadCommited() {
      return new TransactionScope(TransactionScopeOption.Required,
        new TransactionOptions {
          IsolationLevel = IsolationLevel.ReadCommitted,
          Timeout = TransactionManager.MaximumTimeout
        });
    }

    /// <summary>
    /// Creates a instance of <see cref="TransactionScope"/> class that uses
    /// <see cref="IsolationLevel.ReadCommitted"/> as isolation level.
    /// </summary>
    /// <returns></returns>
    public static TransactionScope ReadUncommitted() {
      return new TransactionScope(TransactionScopeOption.Required,
        new TransactionOptions {
          IsolationLevel = IsolationLevel.ReadUncommitted,
          Timeout = TransactionManager.MaximumTimeout
        });
    }
  }
}
