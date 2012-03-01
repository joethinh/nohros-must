using System;
using System.Collections.Generic;

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
      T[] values_;
      Chunk previous_;
      Chunk next_;
      volatile int current_pos_;

      #region .ctor
      /// <summary>
      /// Initializes a new instance of the <see cref="Chunk"/> class by using
      /// the specified capacity.
      /// </summary>
      /// <param name="capacity">The number of elements that the chunk can
      /// hold.</param>
      public Chunk(int capacity) {
        values_ = new T[capacity];
        current_pos_ = 0;
        previous_ = null;
        next_ = null;
      }
      #endregion

      /// <summary>
      /// Gets or sets the previous chunk.
      /// </summary>
      public Chunk Previous {
        get { return previous_; }
        set { previous_ = value; }
      }

      /// <summary>
      /// Gets or sets the next chunk.
      /// </summary>
      public Chunk Next {
        get { return next_; }
        set { next_ = value; }
      }

      /// <summary>
      /// Gets the values that this chunk contains.
      /// </summary>
      public T[] Values {
        get { return values_; }
      }
    }
    #endregion

    int granularity_;
    volatile int head_current_pos_, tail_current_pos_;
    volatile int head_end_pos, tail_end_pos_;
    Chunk head_chunk_;
    Chunk tail_chunk_;

    static Chunk divider_;

    // People are likely to produce and consume at similar rates. In this
    // scenario holding onto the most recently freed chunk saves us from
    // having to instantiate new chunks.
    AtomicReference<Chunk> spare_chunk_;

    #region .ctor
    /// <summary>
    /// Initializes the static variables.
    /// </summary>
    static YQueue() {
      divider_ = new Chunk(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YQueue"/> class by using
    /// the specified granularity.
    /// </summary>
    /// <param name="granularity">A number that defines the granularity of the
    /// queue(how many pushes have to be done till actual memory allocation
    /// is required).</param>
    public YQueue(int granularity) {
      granularity_ = granularity;
      head_chunk_ = tail_chunk_ = divider_;
      head_pos_ = tail_pos_ = 0;
    }
    #endregion

    /// <summary>
    /// Adds an element to the back end of the queue.
    /// </summary>
    /// <param name="element">The element to be added to the back end of the
    /// queue.</param>
    public void Enqueue(T element) {
      int tail_current_pos = tail_current_pos_;

      if (++tail_current_pos < granularity_) {
        // Add the element to the back end of the current tail chunk.
        tail_chunk_.Values[tail_current_pos] = element;

        // "Commit" the newly added item and "publish" it atomically
        // to the consumer thread.
        tail_current_pos_ = tail_current_pos;
        return;
      }

      // The tail chunk is full, create a new one.
      Chunk spare_chunk = spare_chunk_.Exchange(null);
      if (spare_chunk == null) {
        spare_chunk = new Chunk(granularity_);
      }

      // Append the newly created chunk to the queue chain.
      //
      // NOTE: We need to set the previous chunk of the newly created
      // chunk before set the next chunk of the tail to keep the
      // queue state consistent for the consumer thread.
      spare_chunk.Previous = tail_chunk_;
      tail_chunk_.Next = spare_chunk;
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
      if (divider_ != tail_chunk_) {
      }

      if (++head_pos_ == granularity_) {
        Chunk chunk = head_chunk_;
        head_chunk_ = head_chunk_.Next;
        head_chunk_.Previous = null;
        head_pos_ = 0;

        // 'chunk' has been more recently used than 'spare_chunk_', so for
        // cache reasons we'll get rid of the spare and use 'chunk' as the 
        // spare.
        spare_chunk_.Exchange(chunk);
      }
    }
  }
}
