using System;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// Makes a code block transactional.
  /// </summary>
  /// <remarks>
  /// <see cref="ITransactionContext"/> manages the ambient transaction. The
  /// ambient transaction is the transaction your code executes in.
  /// <para>
  /// If no execption occurs within the transaction context(that is, between
  /// the initialization of the <see cref="ITransactionContext"/> object and
  /// the calling of its <see cref="ITransactionContext.Dispose"/> method),
  /// then the transaction in which the scope participates is allowed to
  /// proceed. If an exception does occur within the transaction context, the
  /// transaction in which it participates will be rolled back.
  /// </para>
  /// <para>
  /// When your application completes all work it wants to perform in a
  /// transaction, you should call the <see cref="Complete"/> method only once
  /// to inform that it is acceptable to commit the transaction. Failing to
  /// call this method aborts the transaction.
  /// </para>
  /// <para>
  /// A call to the <see cref="ITransactionContext.Dispose"/> method marks the
  /// end of the transaction context. Exceptions that occur after calling this
  /// method may not affect the transaction.
  /// </para>
  /// </remarks>
  public interface ITransactionContext : IDisposable
  {
    /// <summary>
    /// Indicates that all operations within a context are completed
    /// successfully.
    /// </summary>
    /// <remarks>
    /// When you are satisfied that all operations within the context are
    /// competed successfully, you should call this methos only once to inform
    /// that the state accross all resources is consistent, and the transaction
    /// can be commited. It is very good practive to put the call as the last
    /// statement in the using block.
    /// <para>
    /// Falling to call this methos aborts the transaction because the
    /// <see cref="ITransactionContext"/> interprets this as a system failure,
    /// or exception thrown whithin the context of a transaction. However, you
    /// shold also note that calling this methos does not guarantee a commit of
    /// the transaction. It is merely a way of informing the
    /// <see cref="ITransactionContext"/> of your status.
    /// </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// There is no transaction in progress or this method has laready called
    /// once.
    /// </exception>
    void Complete();
  }
}
