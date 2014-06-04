using System;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A long value that may be updated atomically.
  /// </summary>
  /// <remark>An <see cref="AtomicLong"/> is used in applications such as
  /// atomically incremented sequence numbers, and cannot be used as a
  /// a replacement for a <see cref="Long"/>.</remark>
  public struct AtomicLong
  {
    long value_;
    
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AtomicLong"/> with the
    /// specified initial value.
    /// </summary>
    public AtomicLong(long value) {
      value_ = value;
    }
    #endregion

    /// <summary>
    /// Adds the specified value to the current value, as an atomic operation.
    /// </summary>
    /// <param name="value">The value to be added to the current value.</param>
    /// <returns>A long that is the result of the sum operation.</returns>
    public long Add(long value) {
      return Interlocked.Add(ref value_, value);
    }

    /// <summary>
    /// Set the current value to a specified value and returns the original
    /// value, as an atomic operation.
    /// </summary>
    /// <param name="value">The value to which the current value is set.</param>
    /// <returns>The original value.</returns>
    public long Exchange(long value) {
      return Interlocked.Exchange(ref value_, value);
    }

    /// <summary>
    /// Atomically increment by one the current value.
    /// </summary>
    /// <returns>
    /// The incremented value.
    /// </returns>
    public long Increment() {
      return Interlocked.Increment(ref value_);
    }

    /// <summary>
    /// Atomically decrement by one the current value.
    /// </summary>
    /// <returns>The decremented value.</returns>
    public long Decrement() {
      return Interlocked.Decrement(ref value_);
    }

    /// <summary>
    /// Atomically compare and set the current value to the specified value
    /// if the current value is equals to the expected value.
    /// </summary>
    /// <param name="expect">The expected value.</param>
    /// <param name="update">The value that replaces the current value if
    /// the comparison results in equality.</param>
    /// <returns>The original value.</returns>
    public long CompareExchange(long expect, long update) {
      return Interlocked.CompareExchange(ref value_, update, expect);
    }

    /// <summary>
    /// Atomically compare and set the current value to the specified value
    /// if the current value is equals to the expected value.
    /// </summary>
    /// <param name="expect">
    /// The expected value.
    /// </param>
    /// <param name="update">
    /// The value that replaces the current value if the comparison results in
    /// equality.
    /// </param>
    /// <returns>
    /// <c>true</c> if successfull.<c>false</c> indicates that the actual value
    /// is not equals to the expected value.
    /// </returns>
    public bool CompareSet(long expect, long update) {
      long old_value = Interlocked.CompareExchange(ref value_, update, expect);
      return old_value != update;
    }

    /// <summary>
    /// Gets or sets the current value, as an atomic operation.
    /// </summary>
    public long Value {
      get {
        return Interlocked.Read(ref value_);
      }
      set {
        Exchange(value);
      }
    }

    /// <summary>
    /// Explict Converts the specified <c>long</c> value to an
    /// <see cref="AtomicLong"/>.
    /// </summary>
    /// <param name="value">The <c>long</c> value to be converted.</param>
    /// <returns>A new <see cref="AtomicLong"/> structure whose initial value
    /// is equals to the specified long.</returns>
    public static explicit operator AtomicLong(long value) {
      return new AtomicLong(value);
    }

    /// <summary>
    /// Explict Converts the specified <c>long</c> value to an
    /// <see cref="AtomicLong"/>.
    /// </summary>
    /// <param name="value">The <c>long</c> value to be converted.</param>
    /// <returns>A new <see cref="AtomicLong"/> structure whose initial value
    /// is equals to the specified long.</returns>
    public static explicit operator long(AtomicLong value) {
      return value.Value;
    }

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <returns>
    /// true if <paramref name="obj"/> and this instance are the same type and
    /// represent the same value; otherwise, false.
    /// </returns>
    /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) return false;
      if (obj.GetType() != typeof (AtomicLong)) return false;
      return Equals((AtomicLong) obj);
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer that is the hash code for this instance.
    /// </returns>
    public override int GetHashCode() {
      return Value.GetHashCode();
    }

    /// <summary>
    /// Returns the fully qualified type name of this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"/> containing a fully qualified type name.
    /// </returns>
    public override string ToString() {
      return Value.ToString();
    }

    /// <summary>
    /// Compare the value of <see cref="other"/> with the current
    /// <see cref="AtomicLong"/> value.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(AtomicLong other) {
      // We are assuming that value does not change.
      return Value == other.Value;
    }
  }
}
