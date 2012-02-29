using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nohros.Data.Concurrent
{
  /// <summary>
  /// A long array in which elements may be updated atomically.
  /// </summary>
  public class AtomicLongArray
  {
    long[] elements_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AtomicLong"/> class using
    /// the specified array length.
    /// </summary>
    /// <param name="length"></param>
    public AtomicLongArray(int length) {
      if (length < 0)
        throw new ArgumentOutOfRangeException("length");
      elements_ = new long[length];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AtomicLong"/> class using
    /// the same length as, and all elements copied from, the given array.
    /// </summary>
    /// <param name="array"></param>
    public AtomicLongArray(long[] array) {
      if (array == null)
        throw new ArgumentNullException("array");

      elements_ = new long[array.Length];
      Array.Copy(array, elements_, array.Length);
    }
    #endregion

    /// <summary>
    /// Set the value of the element at index i to the specified value and
    /// returns the original value, as an atomic operation.
    /// </summary>
    /// <param name="i">The index.</param>
    /// <param name="delta">The value to add.</param>
    /// <returns>The original value.</returns>
    public long Exchange(int i, long delta) {
      return Interlocked.Exchange(ref elements_[i], delta);
    }

    /// <summary>
    /// Atomically increment by one the value of the element at index i.
    /// </summary>
    /// <param name="i">The index of the element to increment.</param>
    /// <returns>The incremented value.</returns>
    public long Increment(int i) {
      return Interlocked.Increment(ref elements_[i]);
    }

    /// <summary>
    /// Atomically decrement by one the value of the element at index i.
    /// </summary>
    /// <param name="i">The index of the element to decrement.</param>
    /// <returns>The decremented value.</returns>
    public long Decrement(int i) {
      return Interlocked.Decrement(ref elements_[i]);
    }

    /// <summary>
    /// Gets or sets the value at index i, as an atomic operation.
    /// </summary>
    /// <param name="i"></param>
    /// <value>The value at index i.</value>
    public long this[int i] {
      get {
        return Interlocked.Read(ref elements_[i]);
      } set {
        Exchange(i, value);
      }
    }

    /// <summary>
    /// Gets a 32-bit integer that represents the total number of elements in
    /// the array.
    /// </summary>
    public int Length {
      get { return elements_.Length; }
    }

    /// <summary>
    /// Explicit converts the specified <c>long[]</c> array to an
    /// <see cref="AtomicLongArray"/>.
    /// </summary>
    /// <param name="array">The <c>long[]</c> value to be converted.</param>
    /// <returns>A new <see cref="AtomiclongArray"/> class that contains
    /// the elements copied from the specified <c>long[]</c> array.</returns>
    public static explicit operator AtomicLongArray(long[] array) {
      return new AtomicLongArray(array);
    }

    /// <summary>
    /// Explicit converts the specified <see cref="AtomicLongArray"/> to
    /// an <c>long[]</c> array.
    /// </summary>
    /// <param name="value">The <see cref="AtomicLongArray"/> object that
    /// will be convertet to a long array.</param>
    /// <returns>A <c>long</c> array contained the elements copied from the
    /// specified <see cref="AtomicLongArray"/> object.</returns>
    /// <remarks>This operation is not atomic. If a element value changes while
    /// this method is executing, the returned array could be diferrent from
    /// the current internal array of the specified <paramref name="array"/>.
    /// </remarks>
    public static explicit operator long[](AtomicLongArray array) {
      long[] new_array = new long[array.elements_.Length];
      Array.Copy(array.elements_, new_array, new_array.Length);
      return new_array;
    }
  }
}
