using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Nohros.Caching
{
  [TestFixture]
  public class AbstractCacheTests
  {
    class ThrowableLongCacheLoader : CacheLoader<long> {
      public override long Load(string key) {
        throw new ArgumentException();
      }
    }

    class RandonLongCacheLoader : CacheLoader<long> {
      public override long Load(string key) {
        return new Random().Next()*1000;
      }
    }

    class StringCacheLoader : CacheLoader<string> {
      public override string Load(string key) {
        return "loaded-string";
      }
    }

    class LongCacheLoader: CacheLoader<long> {
      public override long Load(string key) {
        return 30;
      }
    }

    [Test]
    public void ShouldReturnsDefaultValueWhenKeyIsNotFound() {
      CacheBuilder<string> ref_cache_builder = new CacheBuilder<string>();
      AbstractCacheMock<string> ref_cache =
        new AbstractCacheMock<string>(ref_cache_builder);

      string cached = ref_cache.GetIfPresent("missing-key");
      Assert.AreEqual(null, cached);

      CacheBuilder<long> val_cache_builder = new CacheBuilder<long>();
      AbstractCacheMock<long> val_cache =
        new AbstractCacheMock<long>(val_cache_builder);

      long val_cached = val_cache.GetIfPresent("missing-key");
      Assert.AreEqual(default(long), val_cached);
    }

    [Test]
    public void ShouldThrowExceptionWhenLoadFail() {
      CacheBuilder<long> cache_builder = new CacheBuilder<long>();
      AbstractCacheMock<long> cache = new AbstractCacheMock<long>(cache_builder);

      ThrowableLongCacheLoader long_cache_loader = new ThrowableLongCacheLoader();
      try {
        cache.Get("missing-key", long_cache_loader);
      } catch(ExecutionException exception) {
        Assert.IsAssignableFrom<ArgumentException>(exception.InnerException);
      }
    }

    [Test]
    public void ShouldThrowExceptionWhenCacheLoaderReturnsNull() {
      CacheBuilder<string> ref_cache_builder = new CacheBuilder<string>();
      AbstractCache<string> ref_cache =
        new AbstractCacheMock<string>(ref_cache_builder);

      CacheBuilder<long> val_cache_builder = new CacheBuilder<long>();
      AbstractCache<long> val_cache =
        new AbstractCacheMock<long>(val_cache_builder);

      CacheLoader<string> ref_loader = new StringCacheLoader();
      try {
        ref_cache.Get("missing-ref-key", ref_loader);
      } catch(ExecutionException exception) {
        Assert.IsAssignableFrom<InvalidCacheLoadException>(
          exception.InnerException);
      }

      CacheLoader<long> val_loader =
        CacheLoader<long>.From(delegate(string key) {
          return default(long);
        });

      Assert.DoesNotThrow(delegate() {
        val_cache.Get("missing-ref-key", val_loader);
      });
    }

    [Test]
    public void ShouldLoadTheValueForMissingKey() {
      CacheBuilder<string> cache_builder = new CacheBuilder<string>();
      AbstractCache<string> cache = new AbstractCacheMock<string>(cache_builder);

      CacheLoader<string> loader = new StringCacheLoader();
      string cached = cache.GetIfPresent("missing-key");
      Assert.IsNull(cached);

      cached = cache.Get("missing-key", loader);
      Assert.IsNotNull(cached);
    }

    [Test]
    public void ShouldThrowExceptionWhenKeyIsNull() {
      CacheBuilder<string> cache_builder = new CacheBuilder<string>();
      AbstractCache<string> cache = new AbstractCacheMock<string>(cache_builder);

      CacheLoader<string> loader = new StringCacheLoader();
      Assert.Throws<ArgumentNullException>(delegate() {
        cache.Get(null, loader);
      });

      Assert.Throws<ArgumentNullException>(delegate() {
        cache.GetIfPresent(null);
      });

      Assert.Throws<ArgumentNullException>(delegate() {
        cache.Put(null, string.Empty);
      });

      Assert.Throws<ArgumentNullException>(delegate() {
        cache.Remove(null);
      });
    }

    [Test]
    public void ShouldStoreValueInCache() {
      CacheBuilder<string> ref_cache_builder = new CacheBuilder<string>();
      AbstractCache<string> ref_cache =
        new AbstractCacheMock<string>(ref_cache_builder);

      CacheBuilder<long> val_cache_builder = new CacheBuilder<long>();
      AbstractCache<long> val_cache =
        new AbstractCacheMock<long>(val_cache_builder);

      ref_cache.Put("ref-cache-key", "ref-cache-value");
      string ref_cached = ref_cache.GetIfPresent("ref-cache-key");
      Assert.AreEqual(ref_cached, "ref-cache-value");

      val_cache.Put("val-cache-key", 50);
      long val_cached = val_cache.GetIfPresent("val-cache-key");
      Assert.AreEqual(50, val_cached);
    }

    [Test]
    public void ShouldReplaceCachedValueWhenKeyAlreadyExists() {
      CacheBuilder<string> cache_builder = new CacheBuilder<string>();
      AbstractCache<string> ref_cache = new AbstractCacheMock<string>(cache_builder);

      ref_cache.Put("already-in-cache-key", "original-value");
      string cached = ref_cache.GetIfPresent("already-in-cache-key");
      Assert.AreEqual("original-value", cached);

      ref_cache.Put("already-in-cache-key", "new-value");
      cached = ref_cache.GetIfPresent("already-in-cache-key");
      Assert.AreEqual("new-value", cached);
    }

    [Test]
    public void ShouldNotThrowExceptionWhenRemovingNonExistentKey() {
      CacheBuilder<string> cache_builder = new CacheBuilder<string>();
      AbstractCache<string> ref_cache =
        new AbstractCacheMock<string>(cache_builder);
      ref_cache.Remove("missing-key");
    }
  }
}
