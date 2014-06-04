using System;
using System.Data;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Repesents the method that is used to get an instance of a
  /// <see cref="IDbCommand"/> that should be executed within the
  /// context of the <paramref name="context"/> object.
  /// </summary>
  /// <param name="context">
  /// A <see cref="ITransactionContext"/> object representing the transaction
  /// context on which the returned command should be executed.
  /// </param>
  public delegate IDbCommand PrepareCommandDelegate(ITransactionContext context);
}
