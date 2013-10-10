using System;
using NUnit.Framework;

namespace Nohros.Data
{
  public class HiLoGeneratorTests
  {
    class HiLoRange : IHiLoRange
    {
      public long High { get; set; }
      public long MaxLow { get; set; }
    }

    const int kMaxLo = 100;

    IHiLoRange NextHi(ref long current_hi) {
      var hi = current_hi;
      current_hi += kMaxLo + 1;
      return new HiLoRange {
        High = hi,
        MaxLow = kMaxLo
      };
    }

    [Test]
    public void should_generate_ids_between_hi_and_hi_plus_max_lo() {
      int first_hi = 1;
      long next_hi = first_hi;
      var generator = new HiLoGenerator(key => NextHi(ref next_hi));
      long id = 0;
      for (int i = 0; i <= kMaxLo; i++) {
        id = generator.Generate();
      }
      Assert.That(id, Is.EqualTo(first_hi + kMaxLo));
    }
  }
}
