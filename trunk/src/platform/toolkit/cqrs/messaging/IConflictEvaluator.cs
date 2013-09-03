using System;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.Messaging
{
  /// <summary>
  /// A interface that is used to check if an event conflicts with other.
  /// </summary>
  public interface IConflictEvaluator
  {
    /// <summary>
    /// Determines whether the event <paramref name="e1"/> conflicts with the
    /// event <paramref name="e2"/>.
    /// </summary>
    /// <param name="e1">
    /// The first event to compare.
    /// </param>
    /// <param name="e2">
    /// The second event to compare.
    /// </param>
    /// <returns>
    /// <c>true</c> if the event <paramref name="e1"/> conflicts with the
    /// event <paramref name="e2"/>.
    /// </returns>
    bool ConflictWith(Event e1, Event e2);
  }
}
