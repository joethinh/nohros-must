using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Caching
{
  internal sealed class Strength
  {
    Strength() { }

    /// <summary>
    /// Creates a <see cref="IValueReference{T}"/> for the specified
    /// value according to the given strength type.
    /// </summary>
    public static IValueReference<T> ReferenceValue<T>(
      LoadingCache<T>.CacheEntry<T> entry, T value,
      StrengthType strength_type) {

      switch(strength_type) {
        case StrengthType.Strong:
          return new StrongValueReference<T>(value);

        case StrengthType.Soft:
        case StrengthType.Weak:
        default:
          throw new NotImplementedException("Soft and weak values are not implemented.");
      }
    }
  }
}