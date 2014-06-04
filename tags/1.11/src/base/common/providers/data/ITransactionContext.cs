using System;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Makes a code block transactional.
  /// </summary>
  public interface ITransactionContext
  {
    /// <summary>
    /// Atomically commits all the related operations against the associated
    /// data provider.
    /// </summary>
    /// <remarks>
    /// When you are satisfied that all operations within the context are
    /// ready to be persisted against the associated data provider, you should
    /// call this method to inform that the state across all resources is
    /// consistent, and the operations can be persisted.
    /// <para>
    /// Failing to call this method aborts the transaction, preventing the
    /// associated operations to be executed.
    /// </para>
    /// <para>
    /// Calling this method does not guarantee a commit of the operations. It
    /// is merely a way to inform the context about your status.
    /// </para>
    /// <para>
    /// If one of the operations fails to be executed the operations that was
    /// already executed should be rolled back.
    /// </para>
    /// </remarks>
    /// <exception cref="ProviderException">
    /// A operation fail to be persisted against the associated data provider.
    /// </exception>
    void Complete();
  }
}
