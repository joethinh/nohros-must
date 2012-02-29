using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// An object reference that may be updated atomically.
  /// </summary>
  /// <typeparam name="T">The type of the object referred by this interface.
  /// </typeparam>
  public class AtomicReference<T> where T : class
  {
    T value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AtomicReference"/> class
    /// by using the specified initial value.
    /// </summary>
    public AtomicReference(T value) {
      value_ = value;
    }
    #endregion

    /// <summary>
    /// Sets a current value to a specified value and returns the original
    /// value, as an atomic operation.
    /// </summary>
    /// <param name="value">The value to which the current value is set.
    /// </param>
    /// <returns>The original value.</returns>
    public T Exchange(T value) {
      return Interlocked.Exchange<T>(ref value_, value);
    }

    /// <summary>
    /// Atomically set the current value to the spacified updated value if
    /// the current value is equals to the expected value.
    /// </summary>
    /// <param name="expect">The value that is compared to the currrent
    /// value.</param>
    /// <param name="update">The value that replaces the current value if the
    /// comparison results in equality.</param>
    /// <returns>The original value.</returns>
    public T CompareExchange(T expect, T update) {
      return Interlocked.CompareExchange<T>(ref value_, update, expect);
    }

    /// <summary>
    /// Gets or sets the current <typeparam name="T"/>value.
    /// </summary>
    public T Value {
      get { return value_; }
      set { Exchange(value); }
    }
  }
}
