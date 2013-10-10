using System;
using NUnit.Framework;

namespace Nohros.Data
{
  public class HiLoGeneratorTests
  {
    [Test]
    public void should_generate_ids_between_hi_and_hi_plus_max_lo() {
      int first_hi = 1, next_hi = first_hi;
      const int max_lo = 100;
      var generator = new HiLoGenerator(() => {
        var hi = next_hi;
        next_hi += 1000;
        return hi;
      }, max_lo);
      long id = 0;
      for (int i = 0; i <= max_lo; i++) {
        id = generator.Generate();
      }
      Assert.That(id, Is.EqualTo(first_hi + max_lo));
      Assert.That(generator.Generate(), Is.EqualTo(first_hi + 1000));
    }
  }
}
