using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Nohros.Caching.Providers;

namespace Nohros.Caching
{
  [TestFixture]
  public class LoadingCacheMockTests
  {
    [Test]
    public void ShouldLoadTheValueForMissingKey() {
      CacheBuilder<string> cache_builder = new CacheBuilder<string>();
      LoadingCacheMock<string> cache =
        new LoadingCacheMock<string>(cache_builder);

      CacheLoader<string> loader = new StringCacheLoader();
      string cached = cache.GetIfPresent("missing-key");
      Assert.IsNull(cached);

      cached = cache.Get("missing-key", loader);
      Assert.IsNotNull(cached);
    }

    [Test]
    public void ShouldNotThrowExceptionWhenRemovingNonExistentKey() {
      CacheBuilder<string> cache_builder = new CacheBuilder<string>();
      LoadingCacheMock<string> ref_cache =
        new LoadingCacheMock<string>(cache_builder);
      ref_cache.Remove("missing-key");
    }

    [Test]
    public void ShouldReplaceCachedValueWhenKeyAlreadyExists() {
      CacheBuilder<string> cache_builder = new CacheBuilder<string>();
      LoadingCacheMock<string> ref_cache =
        new LoadingCacheMock<string>(cache_builder);

      ref_cache.Put("already-in-cache-key", "original-value");
      string cached = ref_cache.GetIfPresent("already-in-cache-key");
      Assert.AreEqual("original-value", cached);

      ref_cache.Put("already-in-cache-key", "new-value");
      cached = ref_cache.GetIfPresent("already-in-cache-key");
      Assert.AreEqual("new-value", cached);
    }

    [Test]
    public void ShouldReturnsDefaultValueWhenKeyIsNotFound() {
      CacheBuilder<string> ref_cache_builder = new CacheBuilder<string>();
      LoadingCacheMock<string> ref_cache =
        new LoadingCacheMock<string>(ref_cache_builder);

      string cached = ref_cache.GetIfPresent("missing-key");
      Assert.AreEqual(null, cached);

      CacheBuilder<long> val_cache_builder = new CacheBuilder<long>();
      LoadingCacheMock<long> val_cache =
        new LoadingCacheMock<long>(val_cache_builder);

      long val_cached = val_cache.GetIfPresent("missing-key");
      Assert.AreEqual(default(long), val_cached);
    }

    [Test]
    public void ShouldStoreValueInCache() {
      CacheBuilder<string> ref_cache_builder = new CacheBuilder<string>();
      LoadingCacheMock<string> ref_cache =
        new LoadingCacheMock<string>(ref_cache_builder);

      CacheBuilder<long> val_cache_builder = new CacheBuilder<long>();
      LoadingCacheMock<long> val_cache =
        new LoadingCacheMock<long>(val_cache_builder);

      ref_cache.Put("ref-cache-key", "ref-cache-value");
      string ref_cached = ref_cache.GetIfPresent("ref-cache-key");
      Assert.AreEqual(ref_cached, "ref-cache-value");

      val_cache.Put("val-cache-key", 50);
      long val_cached = val_cache.GetIfPresent("val-cache-key");
      Assert.AreEqual(50, val_cached);
    }

    [Test]
    public void ShouldThrowExceptionWhenCacheLoaderReturnsNull() {
      CacheBuilder<string> ref_cache_builder = new CacheBuilder<string>();
      LoadingCacheMock<string> ref_cache =
        new LoadingCacheMock<string>(ref_cache_builder);

      CacheBuilder<long> val_cache_builder = new CacheBuilder<long>();
      LoadingCacheMock<long> val_cache =
        new LoadingCacheMock<long>(val_cache_builder);

      CacheLoader<string> ref_loader = new StringCacheLoader();
      try {
        ref_cache.Get("missing-ref-key", ref_loader);
      } catch (ExecutionException exception) {
        Assert.IsAssignableFrom<InvalidCacheLoadException>(
          exception.InnerException);
      }

      CacheLoader<long> val_loader =
        CacheLoader<long>.From(delegate(string key) { return default(long); });

      Assert.DoesNotThrow(
        delegate() { val_cache.Get("missing-ref-key", val_loader); });
    }

    [Test]
    public void ShouldThrowExceptionWhenKeyIsNull() {
      CacheBuilder<string> cache_builder = new CacheBuilder<string>();
      LoadingCacheMock<string> cache =
        new LoadingCacheMock<string>(cache_builder);

      CacheLoader<string> loader = new StringCacheLoader();
      Assert.Throws<ArgumentNullException>(
        delegate() { cache.Get(null, loader); });

      Assert.Throws<ArgumentNullException>(
        delegate() { cache.GetIfPresent(null); });

      Assert.Throws<ArgumentNullException>(
        delegate() { cache.Put(null, string.Empty); });

      Assert.Throws<ArgumentNullException>(delegate() { cache.Remove(null); });
    }

    [Test]
    public void ShouldThrowExceptionWhenLoadFail() {
      CacheBuilder<long> cache_builder = new CacheBuilder<long>();
      LoadingCacheMock<long> cache = new LoadingCacheMock<long>(cache_builder);

      ThrowableLongCacheLoader long_cache_loader =
        new ThrowableLongCacheLoader();
      try {
        cache.Get("missing-key", long_cache_loader);
      } catch (ExecutionException exception) {
        Assert.IsAssignableFrom<ArgumentException>(exception.InnerException);
      }
    }
  }
}
