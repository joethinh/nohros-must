using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Caching.Providers
{
  public class CacheProviderMock : ICacheProvider
  {
    Hashtable cache_;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Object"/> class.
    /// </summary>
    public CacheProviderMock() {
      cache_ = new Hashtable();
    }

    public T Get<T>(string key) {
      object obj = cache_[key];
      if(obj == null) {
        return default(T);
      }
      return (T) obj;
    }

    public bool Get<T>(string key, out T item) {
      object obj = cache_[key];
      if (obj == null) {
        item = default(T);
        return false;
      }
      item = (T)obj;
      return true;
    }

    public void Set(string key, object value) {
      cache_[key] = value;
    }

    public void Set(string key, object value, long duration, TimeUnit unit) {
      cache_[key] = value;
    }

    public void Add(string key, object value) {
      cache_[key] = value;
    }

    public void Add(string key, object value, long duration, TimeUnit unit) {
      cache_[key] = value;
    }

    public void Remove(string key) {
      cache_.Remove(key);
    }

    public void Clear() {
      cache_.Clear();
    }
  }
}
