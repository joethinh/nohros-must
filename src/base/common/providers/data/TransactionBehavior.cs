using System;
using System.Data;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Provides a description of the effects of the transaction on the
  /// associated connection.
  /// </summary>
  public enum TransactionBehavior
  {
    /// <summary>
    /// When the transaction is commited or rolled back, the associated
    /// <see cref="IDbConnection"/> object is closed.
    /// </summary>
    Default = 0,

    /// <summary>
    /// When the transaction is commited or rolled back, the associated
    /// <see cref="IDbConnection"/> state is not changed.
    /// </summary>
    /// <remarks>
    /// When a command was executed against the transaction, the associated
    /// connection is open and remains in that state until the transaction is
    /// commited or rolled back. By using this flag the connection will remain
    /// opened if a command was executed and will be closed only when it is
    /// disposed.
    /// </remarks>
    KeepState = 1
  }
}
