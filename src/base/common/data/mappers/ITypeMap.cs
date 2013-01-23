using System;
using Nohros.Data;

namespace Nohros.Data
{
  public interface ITypeMap
  {
    /// <summary>
    /// Gets the value that is used on the map operation.
    /// </summary>
    object Value { get; }

    /// <summary>
    /// Gets the type
    /// </summary>
    TypeMapType MapType { get; }
  }
}
