using System;
using System.Data;

namespace Nohros.Data.Providers
{
  /// <summary>
  /// A <see cref="ITransactionContext"/> that commits commands immediatelly
  /// upon enlistment.
  /// </summary>
  public class ImmediateTransactionContext : ITransactionContext
  {
    readonly IDbConnection connection_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ImmediateTransactionContext"/> class using the specified
    /// <see cref="IDbConnection"/> object.
    /// </summary>
    /// <param name="connection">
    /// A <see cref="IDbConnection"/> object that can be used to connects to
    /// the data provider server.
    /// </param>
    public ImmediateTransactionContext(IDbConnection connection) {
      connection_ = connection;
    }
    #endregion

    /// <inheritdoc/>
    public void Enlist(PrepareCommandDelegate preparing,
      ExecuteCommandDelegate executor) {
      bool should_close_connection = false;
      try {
        if (connection_.State != ConnectionState.Open) {
          should_close_connection = true;
          connection_.Open();
        }
        executor(preparing(this));
      } catch (Exception e) {
        throw new ProviderException(e);
      } finally {
        if (should_close_connection) {
          connection_.Close();
        }
      }
    }

    /// <inheritdoc/>
    public void Enlist(PrepareCommandDelegate preparing) {
      Enlist(preparing, delegate(IDbCommand command)
      {
        command.ExecuteNonQuery();
      });
    }

    /// <inheritdoc/>
    public void Enlist(IDbCommand command) {
      Enlist(delegate { return command; });
    }

    /// <inheritdoc/>
    public void Complete() {
    }

    /// <inheritdoc/>
    public IDbCommand CreateCommand() {
      return connection_.CreateCommand();
    }
  }
}
