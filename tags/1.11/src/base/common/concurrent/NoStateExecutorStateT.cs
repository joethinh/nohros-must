using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A <see cref="ExecutorState{T}"/> that has no state associated.
  /// </summary>
  /// <typeparam name="T">The type of the state.s</typeparam>
  /// <remarks>
  /// The state of this class will be set to the default value of the type
  /// <typeparamref name="T"/>.
  /// </remarks>
  internal class NoStateExecutorState<T> : ExecutorState<T> {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutorState{T}"/>
    /// class with no state associated.
    /// </summary>
    public NoStateExecutorState() : base(default(T)) { }
  }
}
