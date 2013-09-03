using System;
using Nohros.CQRS.Messaging;

namespace Nohros.CQRS.EventStore
{
  /// <summary>
  /// A collection of factory methods for the <see cref="IConflictEvaluator"/>
  /// class.
  /// </summary>
  public static class ConflictEvaluators
  {
    /// <summary>
    /// Creates a <see cref="IConflictEvaluator"/> that determines if two
    /// events conflict each other by using their types.
    /// </summary>
    /// <returns>
    /// A <see cref="IConflictEvaluator"/> that determines if two events
    /// conflicts each other by using their types.
    /// </returns>
    public static IConflictEvaluator ByType() {
      return new TypeConflictEvaluator();
    }

    /// <summary>
    /// Creates a <see cref="IConflictEvaluator"/> that determines if two
    /// events conflicts each other by using the given
    /// <see cref="Func{T1, T2, TResult}"/> delegate.
    /// </summary>
    /// <returns>
    /// A <see cref="IConflictEvaluator"/> that determines if two events
    /// conflicts each other by using the given
    /// <see cref="Func{T1, T2, TResult}"/> delegate.
    /// </returns>
    public static IConflictEvaluator FromFunc(Func<Event, Event, bool> func) {
      return new FuncConflictEvaluator(func);
    }
  }
}
