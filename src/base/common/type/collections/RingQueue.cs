using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Nohros.Resources;

namespace Nohros.Collections
{
  /// <summary>
  /// Represents a first-in, first-out cyclic collection of objects.
  /// </summary>
  /// <remarks>
  /// This class is not thread safe.
  /// </remarks>
  public class RingQueue<T> : AbstractRingQueue<T>
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="RingQueue{T}"/> class that
    /// is empty and has the specified size.
    /// </summary>
    public RingQueue(int size) : base(size) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RingQueue{T}"/> class that
    /// contains elements copied from the specified collection and has the same
    /// size as the number of elements copied.
    /// </summary>
    /// <param name="elements"></param>
    public RingQueue(IEnumerable<T> elements) : base(elements) {
    }
    #endregion

    public override void Enqueue(T item) {
      if (IsQueueFull) {
        throw new InvalidOperationException(
          StringResources.InvalidOperation_FullQueue);
      }
      base.Enqueue(item);
    }
  }
}
