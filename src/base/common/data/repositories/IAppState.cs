using System;

namespace Nohros.Data
{
  /// <summary>
  /// Represents an application state repository which is an object that
  /// contains a collection of key/value pairs representing application states.
  /// </summary>
  public interface IAppState
  {
    /// <summary>
    /// Gets the state associated with the key <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <returns>
    /// The value of the state associated with the key <paramref name="name"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The type <typeparamref name="T"/> is not supported by the local
    /// database.
    /// <typeparamref name="T"/>.
    /// <exception cref="NoResultException">
    /// There is no state associated with the key <paramref name="name"/>.
    /// </exception>
    /// </exception>
    T Get<T>(string name);

    /// <summary>
    /// Returns the state associated with the key <paramref name="name"/> or
    /// the given <paramref name="def"/> if a <paramref name="name"/> state is
    /// not found.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <param name="def">
    /// The value to be returned if <paramref name="name"/> state is not found.
    /// </param>
    /// <returns>
    /// The value of the state associated with the key <paramref name="name"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The type <typeparamref name="T"/> is not supported by the local
    /// database.
    /// <typeparamref name="T"/>.
    /// <exception cref="NoResultException">
    /// There is no state associated with the key <paramref name="name"/>.
    /// </exception>
    /// </exception>
    T Get<T>(string name, T def);

    /// <summary>
    /// Gets the state associated with the key <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the state to get.
    /// </param>
    /// <param name="state">
    /// The value of the state associated with the key <paramref name="name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if there is a state associated with the key
    /// <paramref name="name"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The type <typeparamref name="T"/> is not supported by the local
    /// database.
    /// <typeparamref name="T"/>.
    /// </exception>
    bool Get<T>(string name, out T state);

    /// <summary>
    /// Sets the values of the state associated with the key
    /// <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the state to set.
    /// </param>
    /// <param name="state">
    /// The value of the state to set.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// The local database does not support the type <typeparamref name="T"/>.
    /// </exception>
    void Set<T>(string name, T state);

    /// <summary>
    /// Sets the values of the state associated with the key
    /// <paramref name="name"/> if the current value is greater than
    /// <see cref="comparand"/> in an atomic operation.
    /// </summary>
    /// <param name="name">
    /// The name of the state to set.
    /// </param>
    /// <param name="state">
    /// The value of the state to set.
    /// </param>
    /// <param name="comparand">
    /// The value that should be lesser than the current state value.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// The local database does not support the type <see cref="int"/>
    /// </exception>
    void SetIfGreaterThan(string name, int state, int comparand);

    /// <summary>
    /// Sets the values of the state associated with the key
    /// <paramref name="name"/> if the current value is greater than
    /// <see cref="comparand"/> in an atomic operation.
    /// </summary>
    /// <param name="name">
    /// The name of the state to set.
    /// </param>
    /// <param name="state">
    /// The value of the state to set.
    /// </param>
    /// <param name="comparand">
    /// The value that should be lesser than the current state value.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// The local database does not support the type <see cref="long"/>
    /// </exception>
    void SetIfGreaterThan(string name, long state, long comparand);

    /// <summary>
    /// Sets the values of the state associated with the key
    /// <paramref name="name"/> if the current value is lesser than
    /// <see cref="comparand"/> in an atomic operation.
    /// </summary>
    /// <param name="name">
    /// The name of the state to set.
    /// </param>
    /// <param name="state">
    /// The value of the state to set.
    /// </param>
    /// <param name="comparand">
    /// The value that should be lesser than the current state value.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// The local database does not support the type <see cref="int"/>
    /// </exception>
    void SetIfLessThan(string name, int state, int comparand);

    /// <summary>
    /// Sets the values of the state associated with the key
    /// <paramref name="name"/> if the current value is lesser than
    /// <see cref="comparand"/> in an atomic operation.
    /// </summary>
    /// <param name="name">
    /// The name of the state to set.
    /// </param>
    /// <param name="state">
    /// The value of the state to set.
    /// </param>
    /// <param name="comparand">
    /// The value that should be lesser than the current state value.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// The local database does not support the type <see cref="long"/>
    /// </exception>
    void SetIfLessThan(string name, long state, long comparand);
  }
}
