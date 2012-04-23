using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  class ThrowableLongCacheLoader : CacheLoader<long>
  {
    public override long Load(string key)
    {
      throw new ArgumentException();
    }
  }

  class RandonLongCacheLoader : CacheLoader<long>
  {
    public override long Load(string key)
    {
      return new Random().Next() * 1000;
    }
  }

  class StringCacheLoader : CacheLoader<string>
  {
    public override string Load(string key)
    {
      return "loaded-string";
    }
  }

  class LongCacheLoader : CacheLoader<long>
  {
    public override long Load(string key)
    {
      return 30;
    }
  }

  class LoadingCacheMock<T> : LoadingCache<T>
  {
    public LoadingCacheMock(CacheBuilder<T> builder)
      : base(new CacheProviderMock(), builder) {
    }
  }
}
