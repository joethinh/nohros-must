using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  internal enum StrengthType
  {
    /// <summary>
    /// Strong references.
    /// </summary>
    Strong = 0,

    /// <summary>
    /// Soft references.
    /// </summary>
    Soft = 1,

    /// <summary>
    /// Weak references.
    /// </summary>
    Weak = 2
  }
}
