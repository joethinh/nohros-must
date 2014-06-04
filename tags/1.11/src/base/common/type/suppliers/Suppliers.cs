using System;

namespace Nohros
{
  /// <summary>
  /// A collection of useful suppliers.
  /// </summary>
  public sealed class Suppliers
  {
    /// <summary>
    /// Gets a supplier which caches the instance retrieved during the first
    /// call to <see cref="ISupplier{T}.Supply"/> and returns that value on
    /// subsequent calls to <see cref="ISupplier{T}.Supply"/>.
    /// </summary>
    /// <typeparam name="T">The type of the objects returned by the supplier.
    /// </typeparam>
    /// <param name="supplier">A <see cref="ISupplier{T}.Supply"/> that is
    /// used to create the first instance of the type <typeparamref name="T"/>
    /// </param>
    /// <returns>A supplier which caches the instance of retrieved during the
    /// first call the <see cref="ISupplier{T}.Supply"/>.</returns>
    /// <remarks>If <see cref="supplier"/> is an instance created by an earlier
    /// call to <see cref="Memoize{T}(ISupplier{T})"/></remarks>
    public static ISupplier<T> Memoize<T>(ISupplier<T> supplier) {
      if (supplier == null)
        throw new ArgumentNullException("supplier");

      return (supplier is MemoizingSupplier<T>)
               ? supplier
               : new MemoizingSupplier<T>(supplier);
    }

    /// <summary>
    /// Gets a supplier that caches the instance of supplied by the supplier
    /// and removes the cached value after the specified time has passed.
    /// </summary>
    /// <param name="supplier">A <see cref="ISupplier{T}.Supply"/> that is
    /// used to create the first instance of the type <typeparamref name="T"/>
    /// </param>
    /// <param name="duration">The length of time after a value is created
    /// that it should be stop beign returned by subsequent
    /// <see cref="ISupplier{T}.Supply"/> calls.</param>
    /// <param name="unit">The unit that duration is expressed in.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="duration"/> is not positive.</exception>
    /// <returns></returns>
    /// <remarks>
    /// Subsequent calls to <see cref="ISupplier{T}.Supply()"/> return the
    /// cached vale if the expiration time has not passed. After the expiration
    /// time, a new value is retrieved, cached, and returned.
    /// <para>
    /// The returned supplier is thread-safe.
    /// </para>
    /// </remarks>
    public static ISupplier<T> MemoizeWithExpiration<T>(ISupplier<T> supplier,
      long duration, TimeUnit unit) where T: class {
      return new ExpiringMemoizingSupplier<T>(supplier, duration, unit);
    }

    /// <summary>
    /// Gets a supplier that always supplies <param name="instance">.</param>
    /// </summary>
    /// <typeparam name="T">The type of <param name="instance"></param></typeparam>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static ISupplier<T> OfInstance<T>(T instance) {
      return new SupplierOfInstance<T>(instance);
    }
  }
}
