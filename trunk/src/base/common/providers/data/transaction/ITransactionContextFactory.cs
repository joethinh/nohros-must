using System;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// A factory that create instances of the <see cref="ITransactionContext"/>
  /// class.
  /// </summary>
  public interface ITransactionContextFactory
  {
    /// <summary>
    /// Creates a new instance of the <see cref="ITransactionContext"/> object.
    /// </summary>
    /// <returns>
    /// The newly created <see cref="ITransactionContext"/> object.
    /// </returns>
    ITransactionContext CreateTransactionContext();
  }
}
