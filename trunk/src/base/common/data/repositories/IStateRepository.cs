using System;

namespace Nohros.Data
{
  /// <summary>
  /// Represents a state repository which is a repository that contains a
  /// collection of key/value pairs representing states of something.
  /// </summary>
  [Obsolete("This interface was obsolete and was replaced by the IStateDao", false)]
  public interface IStateRepository
  {
    /// <summary>
    /// Creates a query that returns the state that is associated with a
    /// given name.
    /// </summary>
    /// <param name="query">
    /// The newly create <see cref="IStateByNameQuery"/> query.
    /// </param>
    /// <returns>
    /// The newly create <see cref="IStateByNameQuery"/> query.
    /// </returns>
    IStateByNameQuery Query(out IStateByNameQuery query);

    /// <summary>
    /// Creates a query that associates a string representing a state with a
    /// given name.
    /// </summary>
    /// <param name="query">
    /// The newly create <see cref="ISetStateCommand"/> query.
    /// </param>
    /// <returns>
    /// The newly create <see cref="ISetStateCommand"/> query.
    /// </returns>
    ISetStateCommand Query(out ISetStateCommand query);
  }
}
