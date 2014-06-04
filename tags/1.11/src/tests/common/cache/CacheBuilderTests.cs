using System;
using NUnit.Framework;

namespace Nohros.Caching
{
  [TestFixture]
  public class CacheBuilderTests
  {
    [Test]
    public void ShouldSetExpirationAndRefreshToGivenValues() {
      CacheBuilder<string> builder = new CacheBuilder<string>();

      builder.ExpireAfterWrite(1, TimeUnit.Seconds);
      Assert.AreEqual(builder.ExpiryAfterWriteNanos,
        TimeUnitHelper.ToNanos(1, TimeUnit.Seconds));

      builder.ExpireAfterAccess(2, TimeUnit.Seconds);
      Assert.AreEqual(builder.ExpiryAfterAccessNanos,
        TimeUnitHelper.ToNanos(2, TimeUnit.Seconds));
    }

    [Test]
    public void ShouldNotAcceptNegativeExpirarionOrRefreshValues() {
      CacheBuilder<string> builder = new CacheBuilder<string>();

      Assert.Throws<ArgumentOutOfRangeException>(
        delegate()
        {
          builder.ExpireAfterAccess(-1, TimeUnit.Seconds);
        });

      Assert.Throws<ArgumentOutOfRangeException>(
        delegate()
        {
          builder.ExpireAfterWrite(-1, TimeUnit.Seconds);
        });

      Assert.Throws<ArgumentOutOfRangeException>(
        delegate()
        {
          builder.RefreshAfterWrite(-1, TimeUnit.Seconds);
        });
    }
  }
}
