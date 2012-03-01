using System;
using System.Collections.Generic;

namespace Nohros.Concurrent
{
  /// <summary>
  /// <see cref="YQueue"/> is an efficient queue implementation ported from the
  /// zeromq library. <see cref="YQueue"/> allows thread to use
  /// <see cref="Push"/>/<see cref="Back"/> function and another one to use
  /// <see cref="Pop"/>/<see cref="Front"/> functions - at the same time
  /// without locking. However, user must ensure that there's no pop on an
  /// empty queue and that both threads don't access the same element in
  /// unsynchronized manner. <typeparam name="T"> is the type of the objects
  /// in the queue.</typeparam>
  /// </summary>
  public class YQueue<T>
  {
    #region Chunk
    class Chunk
    {
      int granularity_;
      T[] values_;
      Chunk previous_;
      Chunk next_;

      #region .ctor
      /// <summary>
      /// Initializes a new instance of the <see cref="Chunk"/> class.
      /// </summary>
      public Chunk(int granularity) {
        granularity_ = granularity;
        values_ = new T[granularity];
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
    int head_pos_;
    int tail_pos_;
    int end_pos_;
    Chunk head_chunk_;
    Chunk tail_chunk_;
    Chunk end_chunk_;

    // People are likely to produce and consume at similar rates. In this
    // scenario holding onto the most recently freed chunk saves us from
    // having to instantiate new chunks.
    AtomicReference<Chunk> spare_chunk_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="YQueue"/> class by using
    /// the specified granularity.
    /// </summary>
    /// <param name="granularity">A number that defines the granularity of the
    /// queue(how many pushes have to be done till actual memory allocation
    /// is required).</param>
    public YQueue(int granularity) {
      granularity_ = granularity;
      head_chunk_ = new Chunk(granularity);
      tail_chunk_ = null;
      end_chunk_ = head_chunk_;

      head_pos_ = tail_pos_ = end_pos_ = 0;
    }
    #endregion

    /// <summary>
    /// Adds an element to the back end of the queue.
    /// </summary>
    /// <param name="element">The element to be added to the back end of the
    /// queue.</param>
    public void Enqueue(T element) {
      // add the element to the back end of the current tail chunk and
      // set the
      Back = element;
      tail_chunk_ = end_chunk_;
      tail_pos_ = end_pos_;

      if (++end_pos_ != granularity_) {
        return;
      }

      Chunk spare_chunk = spare_chunk_.Exchange(null);
      if (spare_chunk != null) {
        end_chunk_.Next = spare_chunk;
        spare_chunk.Previous = end_chunk_;
      } else {
        end_chunk_.Next = new Chunk(granularity_);
        end_chunk_.Next.Previous = end_chunk_;
      }
      end_chunk_ = end_chunk_.Next;
      end_pos_ = 0;
    }

    /// <summary>
    /// Removes and returns the object at the beginning of the
    /// <see cref="YQueue"/>.
    /// </summary>
    /// <returns></returns>
    public T Dequeue() {
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

    /// <summary>
    /// Gets the front element from the queue. If the queue is empty an
    /// <see cref="IndexOutOfBoundException"/> exception is throwed.
    /// </summary>
    /// <exception cref="IndexOutOfBoundException">The queue is empty.
    /// </exception>
    public T Front {
      get { return head_chunk_.Values[head_pos_]; }
    }

    /// <summary>
    /// Gets the back element from the queue. If the queue is empty an
    /// <see cref="IndexOutOfBoundException"/> exception is throwed.
    /// </summary>
    /// <exception cref="IndexOutOfBoundException">The queue is empty.
    /// </exception>
    public T Back {
      get { return tail_chunk_.Values[tail_pos_]; }
      private set { tail_chunk_.Values[tail_pos_] = value; }
    }
  }
}
