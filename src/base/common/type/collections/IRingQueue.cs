using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Collections
{
  /// <summary>
  /// Represents a first-in, first-out cyclic collection of objects.
  /// </summary>
  public interface IRingQueue<T> : ICollection, IEnumerable<T>, IEnumerable
  {
    /// <summary>
    /// Adds an object to the end of the queue.
    /// </summary>
    /// <param name="item">
    /// The object to add to the <see cref="RingQueue{T}"/>. The value can
    /// be <c>null</c> for reference types.
    /// </param>
    /// <remarks>
    /// <para>
    /// When a queue is full and <see cref="Enqueue"/> method is called, an
    /// exception could be throwed or not, depending on the implementation.
    /// </para>
    /// This method is an O(1) operation.
    /// </remarks>
    void Enqueue(T item);

    /// <summary>
    /// Removes and returns the object at the beginning of the
    /// <see cref="RingQueue{T}"/>.
    /// </summary>
    /// <returns>
    /// The object that is removed from the beginning of the
    /// <see cref="RingQueue{T}"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="RingQueue{T}"/> is empty.
    /// </exception>
    /// <remarks>
    /// This method is similar to the <see cref="Peek"/> method, but
    /// <see cref="Peek"/> does not modify the <see cref="RingQueue{T}"/>.
    /// <para>
    /// If type <typeparamref name="T"/> is a reference type, <c>null</c> can
    /// be added to th <see cref="RingQueue{T}"/> as a value.
    /// </para>
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    /// </remarks>
    T Dequeue();

    /// <summary>
    /// Returns the object at the beginning of the <see cref="RingQueue{T}"/>
    /// without removing it.
    /// </summary>
    /// <returns>
    /// Th object at the beginning of the <see cref="RingQueue{T}"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="RingQueue{T}"/> is empty.
    /// </exception>
    /// <remarks>
    /// This method is similar to the <see cref="Dequeue"/> method, but
    /// <see cref="Dequeue"/> does not modify the <see cref="RingQueue{T}"/>.
    /// <para>
    /// If type <typeparamref name="T"/> is a reference type, <c>null</c> can
    /// be added to th <see cref="RingQueue{T}"/> as a value.
    /// </para>
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    /// </remarks>
    T Peek();
  }
}
