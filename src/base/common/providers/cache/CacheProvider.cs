using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Configuration;
using Nohros.Providers;

namespace Nohros.Caching.Providers
{
  /// <summary>
  /// This class provides a skeletal implementation of the
  /// <see cref="ICacheProvider"/> interface to minimize the effort required to
  /// implement this interface.
  /// </summary>
  public abstract class CacheProvider: ICacheProvider
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheProvider"/> class.
    /// </summary>
    protected CacheProvider() { }
    #endregion

    /// <summary>
    /// Creates an instance of the <see cref="ICacheProvider"/> class by using
    /// the specified <paramref name="provider"/> object and a list of objects
    /// that will be passed to the provider constructor.
    /// </summary>
    /// <param name="provider">A <see cref="ProviderNode"/>
    /// object that contains information about the class that should be
    /// instantiated.</param>
    /// <remarks>
    /// <para>The value of the <see cref="ProviderNode.AssemblyLocation"/>
    /// property will be used as a search location for assemblies that need to
    /// be loaded in order to create the desired cache provider instance.
    /// </para>
    /// </remarks>
    /// <returns>A reference to the newly created object.</returns>
    public static ICacheProvider CreateCacheProvider(IProviderNode provider) {
      ICacheProviderFactory factory = ProviderFactory<ICacheProviderFactory>.
        CreateProviderFactory(provider);
      return factory.CreateCacheProvider(provider.Options);
    }

    /// <summary>
    /// Gets the absolute entry expiration date and time by adding the
    /// specified <see cref="expiry"/> parameter to the current date and time.
    /// </summary>
    /// <param name="expiry">The interval between now and the date that the
    /// entry should expire.</param>
    /// <exception cref="ArgumentOutOfRangeException">You set the
    /// <paramref name="expiry"/> to less than <c>TimeSpan.Zero</c> or to a
    /// value that exceeds the maximum system date time.</exception>
    /// <returns>A <see cref="DateTime"/> object that represents the
    /// date and time that the entry should expire.</returns>
    protected DateTime GetExpiryDateTime(TimeSpan expiry) {
      // store the current date time as soon as possible.
      DateTime now = DateTime.Now;

      // the expiration interval could not be negative.
      if (expiry < TimeSpan.Zero) {
        throw new ArgumentOutOfRangeException("expiry");
      }

      // the specified interval could not overlap the system maximum value for
      // date and time.
      TimeSpan max_time_span = DateTime.MaxValue.Subtract(now);
      if (expiry > max_time_span) {
        throw new ArgumentOutOfRangeException("expiry");
      }

      return now.Add(expiry);
    }

    /// <inheritdoc/>
    public abstract T Get<T>(string key);

    /// <inheritdoc/>
    public abstract void Set(string key, object value);

    /// <inheritdoc/>
    public abstract void Set(string key, object value, TimeSpan expiry);

    /// <inheritdoc/>
    public abstract void Add(string key, object value);

    /// <inheritdoc/>
    public abstract void Add(string key, object value, TimeSpan expiry);

    /// <inheritdoc/>
    public abstract void Remove(string key);

    /// <inheritdoc/>
    public abstract void Clear();
  }
}
