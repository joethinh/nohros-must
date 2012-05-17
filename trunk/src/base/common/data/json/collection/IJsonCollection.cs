using System;

namespace Nohros.Data.Json
{
  /// <summary>
  /// Represents a collection of <see cref="IJsonToken"/> objects.
  /// </summary>
  public interface IJsonCollection
  {
    /// <summary>
    /// Adds an <see cref="IJsonToken"/> to the
    /// <seealso cref="IJsonCollection"/>.
    /// </summary>
    /// <param name="token">
    /// The <seealso cref="IJsonToken"/> object to be added to the
    /// <see cref="IJsonToken"/>
    /// </param>
    void Add(IJsonToken token);

    /// <summary>
    /// Gets the number of elements contained in the
    /// <see cref="IJsonCollection"/>.
    /// </summary>
    /// <value>
    /// The number of elements contained in the <see cref="IJsonCollection"/>.
    /// </value>
    int Count { get; }
  }
}
