using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Nohros.Extensions
{
  public class StringsTests
  {
    [Test]
    public void ShouldRemoveDiacritics() {
      string str = "Café";
      Assert.That(str.RemoveDiacritics(), Is.EqualTo("Cafe"));
    }
  }
}
