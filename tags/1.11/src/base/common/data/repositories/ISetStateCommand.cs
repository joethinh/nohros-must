using System;

namespace Nohros.Data
{
  /// <summary>
  /// Associates a string representing a state with a given key.
  /// </summary>
  [Obsolete("This interface is obsolete. Check the IStateDao interface.")]
  public interface ISetStateCommand
  {
    /// <summary>
    /// Associates the state represented by <paramref name="state"/> with the
    /// given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// A string that can be used to identify the state within a state
    /// repository.
    /// </param>
    /// <param name="state">
    /// The state to be persisted.
    /// </param>
    void Execute(string name, string state);
  }
}
