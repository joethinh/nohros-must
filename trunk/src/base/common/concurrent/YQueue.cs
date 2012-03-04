using System;
using System.Threading;
using System.Collections.Generic;

using Nohros.Resources;

namespace Nohros.Concurrent
{
  /// <summary>
  /// <see cref="YQueue"/> is an efficient queue implementation ported from
  /// the zeromq library. <see cref="YQueue"/> allows one thread to use the
  /// <see cref="Enqueue"/> function while another one use the
  /// <see cref="Dequeue"/> function without locking. <typeparam name="T"> is
  /// the type of the objects in the queue.</typeparam>
  /// </summary>
  public class YQueue<T>
  {
    #region Chunk
    class Chunk
    {
      public T[] values;
      public volatile int tail_pos, head_pos;
      public volatile Chunk next;

      #region .ctor
      /// <summary>
      /// Initializes a new instance of the <see cref="Chunk"/> class by using
      /// the specified capacity.
      /// </summary>
      /// <param name="capacity">The number of elements that the chunk can
      /// hold.</param>
      public Chunk(int capacity) {
        values = new T[capacity];
        head_pos = 0;
        tail_pos = 0;
        next = null;
      }
      #endregion
    }
    #endregion

    int granularity_;
    volatile Chunk head_chunk_, tail_chunk_, divider_;

    // People are likely to produce and consume at similar rates. In this
    // scenario holding onto the most recently freed chunk saves us from
    // having to instantiate new chunks.
    volatile Chunk spare_chunk_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="YQueue&lt;T&gt;"/> class
    /// by using the specified granularity.
    /// </summary>
    /// <param name="granularity">A number that defines the granularity of the
    /// queue(how many pushes have to be done till actual memory allocation
    /// is required).</param>
    public YQueue(int granularity) {
      granularity_ = granularity;
      divider_ = new Chunk(0);
      divider_.next = tail_chunk_;
      head_chunk_ = tail_chunk_ = divider_;
      spare_chunk_ = null;
    }
    #endregion

    /// <summary>
    /// Adds an element to the back end of the queue.
    /// </summary>
    /// <param name="element">The element to be added to the back end of the
    /// queue.</param>
    public void Enqueue(T element) {
      int tail_pos = tail_chunk_.tail_pos;

      // If either the queue is not empty or the tail chunk is not full, adds
      // the specified element to the back end of the current tail chunk.
      if (tail_chunk_ != divider_ && ++tail_pos < granularity_) {
        tail_chunk_.values[tail_pos] = element;

        // Prevents any kind of instruction reorderring or caching.
        Thread.MemoryBarrier();

        // "Commit" the newly added item and "publish" it atomically
        // to the consumer thread.
        tail_chunk_.tail_pos = tail_pos;
        return;
      }

      // Create a new chunk if a cached one does not exists yet and links it
      // to the current last node.
      Chunk chunk = (spare_chunk_ == null) ?
        new Chunk(granularity_) : spare_chunk_;
      tail_chunk_.next = chunk;

      // Reset the chunk and append the specified element to the first slot.
      chunk.tail_pos = 0; // An unconsumed element is added to the first slot.
      chunk.head_pos = 0;
      chunk.next = null;
      chunk.values[0] = element;

      // At this point the newly created chunk(or the last cached chunk) is
      // not yet shared, but still private to the producer; the consumer will
      // not follow the linked chunk unless the value of |tail_chunk_| says
      // it may follow. The line above "commit" the update and publish it
      // atomically to the consumer thread.
      tail_chunk_ = tail_chunk_.next;

      // Performs a lazy cleanup of now-unused nodes. Because we always stop
      // before |divider_|,this can't conflict with anything the consumer
      // might be doing later in the list. Each time we read |divider_|, we
      // see it either before or after any concurrent update by the consumer,
      // both if which let the producer see the list in a consistent state.
      while (head_chunk_.head_pos > head_chunk_.tail_pos &&
        head_chunk_ != divider_) {

        // |head_chunk_| has been more recently used than |spare_chunk_|, so
        // for cache reasons we'll get rid of the spare and use |head_chunk_|
        // as the spare.
        spare_chunk_ = head_chunk_;

        // Advance the head chunk to the next used chunk or divider.
        head_chunk_ = head_chunk_.next;
      }
    }

    /// <summary>
    /// Removes and returns the object at the beginning of the
    /// <see cref="YQueue&lt;T&gt;"/>.
    /// </summary>
    /// <returns><typeparamref name="T"/> The object that is removed from the
    /// <see cref="YQueue&lt;T&gt;"/></returns>
    /// <exception cref="InvalidOperationException">The
    /// <see cref="YQueue&lt;T&gt;"/> is empty.</exception>
    public T Dequeue() {
      T t;
      bool ok = Dequeue(out t);
      if (!ok) {
        throw new InvalidOperationException(
          StringResources.InvalidOperation_EmptyQueue);
      }
      return t;
    }

    /// <summary>
    /// Removes and returns the object at the beginning of the
    /// <see cref="YQueue&lt;T&gt;"/>.
    /// </summary>
    /// <param name="t">When this method returns, contains the object that was
    /// removed from the beginning of the <see cref="YQueue&lt;T&gt;"/>, if
    /// the object was successfully removed; otherwise the default value
    /// of the type <typeparamref name="T"/>.</param>
    /// <returns><c>true</c> if the queue is not empty and the object at the
    /// beginning of it was successfully removed; otherwise, false.
    /// </returns>
    public bool Dequeue(out T t) {
      // checks if the queue is empty
      while (divider_ != tail_chunk_) {
        // The chunks that sits between the |divider_| and the |tail_chunk_|,
        // excluding the |divider_| and including the |tail_chunk_|, are
        // unconsumed.
        Chunk current_chunk = divider_.next;

        // We need to compare the current chunk |tail_pos| with the |head_pos|
        // and |granularity|. Since, the |tail_pos| can be modified by the
        // producer thread we need to cache it's value.
        int tail_pos;
        tail_pos = current_chunk.tail_pos;
        
        if (current_chunk.head_pos > tail_pos) {
          // we have reached the end of the chunk, go to the next.
          if (tail_pos == granularity_) {
            divider_ = current_chunk.next;
            continue;
          } else {
            // we already consume all the available itens.
            t = default(T);
            return false;
          }
        } else {
          // Ensure that we are reading the freshness value from the chunk
          // values array.
          Thread.MemoryBarrier();

          // Here the |head_pos| is less than or equals to |tail_pos|, get
          // the first unconsumed element and increments |head_pos| to publish
          // that the queue item was removed.
          t = current_chunk.values[current_chunk.head_pos++];
          return true;
        }
      }
      t = default(T);
      return false;
    }
  }
}
