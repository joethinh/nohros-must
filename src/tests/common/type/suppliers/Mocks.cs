using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Tests
{
  internal class TMock
  {
    public string Name { get; set; }

    public TMock(string name) {
      Name = name;
    }
  }

  internal class TSupplierMock: ISupplier<TMock>
  {
    public string Name { get; set; }

    public TSupplierMock(string name) { }

    public TMock Supply() {
      return new TMock(Name);
    }
  }
}
