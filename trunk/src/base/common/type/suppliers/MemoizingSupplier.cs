using System;

namespace Nohros
{
  /// <summary>
  /// A supplier that caches the instance of the retrieved object during
  /// the first call to <see cref="Supply"/> and returns that value on
  /// subsequent calls to <see cref="Supply"/>.
  /// </summary>
  /// <typeparam name="T">The type of the object that this supplier returns.
  /// </typeparam>
  /// <remarks>The returned supplier is thread-safe.</remarks>
  public class MemoizingSupplier<T> : ISupplier<T>
  {
    readonly ISupplier<T> supplier_;
    volatile bool initialized_;

    readonly object mutex_;

    // "value" does not need to be volatile; visibility piggy-backs
    // on volatile read of "initialized".
    T value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MemoizingSupplier{T}"/>
    /// by using the specified <paramref name="supplier"/>.
    /// </summary>
    /// <param name="supplier">A <see cref="ISupplier{T}"/> that is used to
    /// create the first instance of the type <typeparamref name="T"/></param>
    public MemoizingSupplier(ISupplier<T> supplier) {
      supplier_ = supplier;
      mutex_ = new object();
    }
    #endregion

    /// <summary>
    /// Gets an instance of the type <paramref name="T"/>.
    /// </summary>
    /// <remarks>
    /// The instance that is returned on the first call is cached and returned
    /// on the subsequent calls to this method.
    /// </remarks>
    /// <returns>An instance of the type <typeparamref name="T"/></returns>
    public T Supply() {
      // A 2-field variant of double checked locking.
      if(!initialized_) {
        lock (mutex_) {
          if(!initialized_) {
            T t = supplier_.Supply();
            value_ = t;
            initialized_ = true;
            return t;
          }
        }
      }
      return value_;
    }
  }
}