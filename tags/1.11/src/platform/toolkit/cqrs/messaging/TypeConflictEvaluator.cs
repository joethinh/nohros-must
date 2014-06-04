using System;

namespace Nohros.CQRS.Messaging
{
  /// <summary>
  /// An implementation of the <see cref="IConflictEvaluator"/> that uses the
  /// event types to determine if they conflict.
  /// </summary>
  internal class TypeConflictEvaluator : IConflictEvaluator
  {
    public bool ConflictWith(Event e1, Event e2) {
      return (e1.GetType() == e2.GetType());
    }
  }
}
