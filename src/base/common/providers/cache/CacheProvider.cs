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
  public abstract partial class CacheProvider : ICacheProvider
  {
    #region .ctor
    /// <summary>
    /// A construtor implied by the <see cref="ICacheProviderFactory"/>
    /// </summary>
    CacheProvider() {
    }
    #endregion

    #region ICacheProvider Members
    /// <inheritdoc/>
    public abstract T Get<T>(string key);

    /// <inheritdoc/>
    public abstract void Set(string key, object value);

    /// <inheritdoc/>
    public abstract void Add(string key, object value);

    /// <inheritdoc/>
    public abstract void Remove(string key);

    /// <inheritdoc/>
    public abstract void Clear();

    /// <inheritdoc/>
    public abstract void Set(string key, object value, long duration,
      TimeUnit unit);

    /// <inheritdoc/>
    public abstract void Add(string key, object value, long duration,
      TimeUnit unit);
    #endregion
  }
}