using System;

namespace Nohros.CQRS.Messaging
{
  internal class NoConflictEvaluator : IConflictEvaluator
  {
    public bool ConflictWith(Event e1, Event e2) {
      return false;
    }
  }
}
