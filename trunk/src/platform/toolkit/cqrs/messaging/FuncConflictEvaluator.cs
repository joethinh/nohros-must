using System;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.Messaging
{
  /// <summary>
  /// An implementation of the <see cref="IConflictEvaluator"/> class that uses
  /// the a <see cref="Func{TResult}"/> to determine if two events conflicts.
  /// </summary>
  public class FuncConflictEvaluator : IConflictEvaluator
  {
    readonly Func<Event, Event, bool> func_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="FuncConflictEvaluator"/>
    /// class by using the given <see cref="Func{T, T, TResult}"/> delegate
    /// </summary>
    /// <param name="func"></param>
    public FuncConflictEvaluator(Func<Event, Event, bool> func) {
      func_ = func;
    }
    #endregion

    /// <inheritdoc/>
    public bool ConflictWith(Event e1, Event e2) {
      return func_(e1, e2);
    }
  }
}
