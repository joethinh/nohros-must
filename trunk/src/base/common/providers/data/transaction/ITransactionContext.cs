using System;
using System.Data;

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

    /// <summary>
    /// Enlists a command to participate in a transaction.
    /// </summary>
    /// <param name="command">
    /// A <see cref="PrepareCommandDelegate"/> that returns a
    /// <see cref="IDbCommand"/> that should be enlisted to participate in a
    /// transaction.
    /// </param>
    /// <remarks>
    /// The <see cref="PrepareCommandDelegate"/> should be called at the time
    /// the <see cref="Complete"/> method runs, so a <see cref="IDbCommand"/>
    /// can use the result of another <see cref="IDbCommand"/> as a parameter.
    /// <para>
    /// The returned <see cref="IDbCommand"/> should be attached to a
    /// transaction at the time the <see cref="Complete"/> method runs.
    /// </para>
    /// <para>
    /// The returned <see cref="IDbCommand"/> will be associated with the
    /// same <see cref="IDbConnection"/> that was specified at the construction
    /// time.
    /// </para>
    /// </remarks>
    void Enlist(PrepareCommandDelegate command);

    /// <summary>
    /// Enlists a command that returns a result to participate in a transaction.
    /// </summary>
    /// <param name="preparing">
    /// A <see cref="PrepareCommandDelegate"/> that returns the
    /// <see cref="IDbCommand"/> that should be enlisted to participate in a
    /// transaction.
    /// </param>
    /// <param name="executor">
    /// A <see cref="ExecuteCommandDelegate"/> that should be called to
    /// execute the <see cref="IDbCommand"/>.
    /// </param>
    /// <remarks>
    /// The <paramref name="preparing"/> delegate will be called at the time
    /// the <see cref="Complete"/> method runs, so a <see cref="IDbCommand"/>
    /// can use the result of another <see cref="IDbCommand"/> as a parameter.
    /// <para>
    /// The returned <see cref="IDbCommand"/> will be attached to a
    /// transaction at the time the <see cref="Complete"/> method runs.
    /// </para>
    /// <para>
    /// The returned <see cref="IDbCommand"/> will be associated with the
    /// same <see cref="IDbConnection"/> that was specified at the construction
    /// time.
    /// </para>
    /// <para>
    /// The <see cref="executor"/> delegate will be called after the command is
    /// prepared and attached to the transaction associated with the
    /// <see cref="ITransactionContext"/>.
    /// </para>
    /// </remarks>
    void Enlist(PrepareCommandDelegate preparing,
      ExecuteCommandDelegate executor);

    /// <summary>
    /// Creates a command that is related with the same connection that is
    /// associated with the <see cref="ITransactionContext"/>.
    /// </summary>
    /// <remarks>
    /// A <see cref="IDbCommand"/> that associated with the
    /// <see cref="ITransactionContext"/>.
    /// </remarks>
    IDbCommand CreateCommand();
  }
}
