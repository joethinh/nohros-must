using System;

namespace Nohros.CQRS.Domain
{
  public static class AggregateFactories
  {
    /// <summary>
    /// Creates a <see cref="IAggregateFactory{T}"/> that uses the given
    /// <see cref="Func{T, TResult}"/> delegate to create instances of the
    /// <typeparamref name="T"/> class.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the aggregate that should be created by the returned
    /// <see cref="IAggregateFactory{T}"/> object.
    /// </typeparam>
    /// <param name="func">
    /// A <see cref="Func{T, TResult}"/> delegate that should be used to create
    /// instances of the <typeparamref name="T"/> class.
    /// </param>
    /// <returns>
    /// A <see cref="IAggregateFactory{T}"/> that uses the given
    /// <see cref="Func{T, TResult}"/> delegate to create instances of the
    /// <typeparamref name="T"/> class.
    /// </returns>
    public static IAggregateFactory<T> FromFunc<T>(Func<Guid, T> func)
      where T : AggregateRoot {
      return new FuncAggregateFactory<T>(func);
    }
  }
}
