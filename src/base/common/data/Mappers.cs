using System;
using System.Collections.Generic;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// Factory class for <see cref="IMapper{T}"/> interface.
  /// </summary>
  public static class Mappers
  {
    /// <summary>
    /// Creates a new instance of the <see cref="IMapper{T}"/> that uses
    /// <paramref name="mapping"/> to map between the columns of
    /// <paramref name="reader"/> to the properties of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the interface to map.
    /// </typeparam>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> containing the data to be mapped.
    /// </param>
    /// <param name="mapping">
    /// An array of <see cref="KeyValuePair{TKey,TValue}"/> containg the map
    /// between the columns of <paramref name="reader"/> and the properties
    /// of <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      KeyValuePair<string, string>[] mapping) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IMapper{T}"/> that uses
    /// <paramref name="mapping"/> to map between the columns of
    /// <paramref name="reader"/> to the properties of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the interface to map.
    /// </typeparam>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> containing the data to be mapped.
    /// </param>
    /// <param name="mapping">
    /// A <see cref="CallableDelegate{T}"/> that can be used to get an array of
    /// <see cref="KeyValuePair{TKey,TValue}"/> containing the map
    /// between the columns of <paramref name="reader"/> and the properties
    /// of <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      CallableDelegate<KeyValuePair<string, string>[]> mapping) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader);
    }

    public static IMapper<T> GetMapper<T>(IDataReader reader) {
      return new DataReaderMapper<T>
        .Builder()
        .Build(reader);
    }
  }
}
