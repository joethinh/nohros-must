using System;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Makes a code block transactional.
  /// </summary>
  public interface ITransactionContext : IDisposable
  {
    /// <summary>
    /// Indicates that all operations within a context are completed
    /// successfully.
    /// </summary>
    void Complete();
  }
}
