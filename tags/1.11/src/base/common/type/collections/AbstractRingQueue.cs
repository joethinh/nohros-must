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
  public abstract class AbstractRingQueue<T> : IRingQueue<T>
  {
    protected readonly T[] elements_;
    int count_;
    int head_;
    object sync_root_;
    int tail_;

    #region .ctor
    AbstractRingQueue() {
      head_ = 0;
      tail_ = 0;
      sync_root_ = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractRingQueue{T}"/> class that
    /// is empty and has the specified size.
    /// </summary>
    protected AbstractRingQueue(int size) : this() {
      elements_ = new T[size];
      count_ = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractRingQueue{T}"/> class that
    /// contains elements copied from the specified collection and has the same
    /// size as the number of elements copied.
    /// </summary>
    /// <param name="elements"></param>
    protected AbstractRingQueue(IEnumerable<T> elements) : this() {
      elements_ = new List<T>(elements).ToArray();
      count_ = elements_.Length;
    }
    #endregion

    /// <inheritdoc/>
    public int Count {
      get { return count_; }
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    /// <inheritdoc/>
    public void CopyTo(Array array, int index) {
      Array.Copy(elements_, tail_, array, index, count_);
    }

    object ICollection.SyncRoot {
      get {
        if (sync_root_ == null) {
          Interlocked.CompareExchange(ref sync_root_, new object(), null);
        }
        return sync_root_;
      }
    }

    bool ICollection.IsSynchronized {
      get { return false; }
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() {
      foreach (T t in elements_) {
        yield return t;
      }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// If the queue if full the first unconsumed element will be overritten.
    /// </remarks>
    public virtual void Enqueue(T item) {
      elements_[head_] = item;
      head_ = (head_ + 1)%elements_.Length;
      ++count_;
    }

    /// <summary>
    /// Removes and returns the object at the beginning of the
    /// <see cref="AbstractRingQueue{T}"/>.
    /// </summary>
    /// <returns>
    /// The object that is removed from the beginning of the
    /// <see cref="AbstractRingQueue{T}"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="AbstractRingQueue{T}"/> is empty.
    /// </exception>
    /// <remarks>
    /// This method is similar to the <see cref="Peek"/> method, but
    /// <see cref="Peek"/> does not modify the <see cref="AbstractRingQueue{T}"/>.
    /// <para>
    /// If type <typeparamref name="T"/> is a reference type, <c>null</c> can
    /// be added to th <see cref="AbstractRingQueue{T}"/> as a value.
    /// </para>
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    /// </remarks>
    public virtual T Dequeue() {
      if (head_ == tail_ && count_ == 0) {
        throw new InvalidOperationException(
          StringResources.InvalidOperation_EmptyQueue);
      }
      T item = elements_[tail_];
      tail_ = (tail_ + 1)%elements_.Length;
      --count_;
      return item;
    }

    /// <summary>
    /// Returns the object at the beginning of the <see cref="AbstractRingQueue{T}"/>
    /// without removing it.
    /// </summary>
    /// <returns>
    /// Th object at the beginning of the <see cref="AbstractRingQueue{T}"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="AbstractRingQueue{T}"/> is empty.
    /// </exception>
    /// <remarks>
    /// This method is similar to the <see cref="Dequeue"/> method, but
    /// <see cref="Dequeue"/> does not modify the <see cref="AbstractRingQueue{T}"/>.
    /// <para>
    /// If type <typeparamref name="T"/> is a reference type, <c>null</c> can
    /// be added to th <see cref="AbstractRingQueue{T}"/> as a value.
    /// </para>
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    /// </remarks>
    public virtual T Peek() {
      if (head_ == tail_ && count_ == 0) {
        throw new InvalidOperationException(
          StringResources.InvalidOperation_EmptyQueue);
      }
      return elements_[tail_];
    }

    /// <summary>
    /// Removes all objects from the <see cref="Queue{T}"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="Count"/> is set to zero and references to other objects from
    /// elements of the collection are also relesead.
    /// <para>
    /// This method is a O(n) operation, where n is <see cref="Count"/>.
    /// </para>
    /// </remarks>
    public void Clear() {
      Array.Clear(elements_, tail_, count_);
      count_ = 0;
      head_ = 0;
      tail_ = 0;
    }

    protected bool IsQueueFull {
      get { return head_ == tail_ && count_ != 0; }
    }

    protected bool IsQueueEmpty {
      get { return head_ == tail_ && count_ == 0; }
    }
  }
}
