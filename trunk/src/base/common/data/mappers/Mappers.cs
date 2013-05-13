using System;
using System.Collections.Generic;
using System.Data;

namespace Nohros.Data
{
  /// <summary>
  /// Factory class for <see cref="IDataReaderMapper{T}"/> interface.
  /// </summary>
  public static class Mappers
  {
    /// <summary>
    /// Creates a new instance of the <see cref="IDataReaderMapper{T}"/> that
    /// uses the specified <paramref name="mapping"/> to map between the
    /// columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the interface to map.
    /// </typeparam>
    /// <param name="mapping">
    /// A <see cref="CallableDelegate{T}"/> that can be used to get an array of
    /// <see cref="KeyValuePair{TKey,TValue}"/> containing the map
    /// between the columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </param>
    public static IDataReaderMapper<T> GetMapper<T>(
      KeyValuePair<string, string>[] mapping) {
      return GetMapper<T>(mapping, typeof (T).Namespace);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IDataReaderMapper{T}"/> that
    /// uses the specified <paramref name="mapping"/> to map between the
    /// columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the interface to map.
    /// </typeparam>
    /// <param name="mapping">
    /// A <see cref="CallableDelegate{T}"/> that can be used to get an array of
    /// <see cref="KeyValuePair{TKey,TValue}"/> containing the map
    /// between the columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </param>
    /// <param name="prefix">
    /// A string that can be used to distinguish two mappers that map the same
    /// class using distinct ways.
    /// </param>
    public static IDataReaderMapper<T> GetMapper<T>(
      KeyValuePair<string, string>[] mapping, string prefix) {
      return new DataReaderMapperBuilder<T>(prefix)
        .Map(mapping)
        .Build();
    }


    /// <summary>
    /// Creates a new instance of the <see cref="IDataReaderMapper{T}"/> that
    /// uses the specified <paramref name="mapping"/> to map between the
    /// columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the interface to map.
    /// </typeparam>
    /// <param name="mapping">
    /// A <see cref="CallableDelegate{T}"/> that can be used to get an array of
    /// <see cref="KeyValuePair{TKey,TValue}"/> containing the map
    /// between the columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </param>
    public static IDataReaderMapper<T> GetMapper<T>(
      KeyValuePair<string, string>[] mapping,
      CallableDelegate<T> factory) {
      return GetMapper(mapping, factory, typeof (T).Namespace);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IDataReaderMapper{T}"/> that
    /// uses the specified <paramref name="mapping"/> to map between the
    /// columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the interface to map.
    /// </typeparam>
    /// <param name="mapping">
    /// A <see cref="CallableDelegate{T}"/> that can be used to get an array of
    /// <see cref="KeyValuePair{TKey,TValue}"/> containing the map
    /// between the columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </param>
    /// <param name="prefix">
    /// A string that can be used to distinguish two mappers that map the same
    /// class in distinct forms.
    /// </param>
    public static IDataReaderMapper<T> GetMapper<T>(
      KeyValuePair<string, string>[] mapping,
      CallableDelegate<T> factory,
      string prefix) {
      return new DataReaderMapperBuilder<T>(prefix)
        .Map(mapping)
        .Build();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IDataReaderMapper{T}"/> that
    /// uses the specified <paramref name="mapping"/> to map between the
    /// columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the interface to map.
    /// </typeparam>
    /// <param name="mapping">
    /// A <see cref="CallableDelegate{T}"/> that can be used to get an array of
    /// <see cref="KeyValuePair{TKey,TValue}"/> containing the map
    /// between the columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </param>
    /// <remarks></remarks>
    public static IDataReaderMapper<T> GetMapper<T>(
      KeyValuePair<string, ITypeMap>[] mapping) {
      return GetMapper<T>(mapping, typeof (T).Namespace);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IDataReaderMapper{T}"/> that
    /// uses the specified <paramref name="mapping"/> to map between the
    /// columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the interface to map.
    /// </typeparam>
    /// <param name="mapping">
    /// A <see cref="CallableDelegate{T}"/> that can be used to get an array of
    /// <see cref="KeyValuePair{TKey,TValue}"/> containing the map
    /// between the columns of a <see cref="IDataReader"/> to the properties of
    /// the <typeparamref name="T"/>.
    /// </param>
    /// <param name="prefix">
    /// A string that can be used to distinguish two mappers that map the same
    /// class in distinct forms.
    /// </param>
    public static IDataReaderMapper<T> GetMapper<T>(
      KeyValuePair<string, ITypeMap>[] mapping,
      string prefix) {
      return new DataReaderMapperBuilder<T>(prefix)
        .Map(mapping)
        .Build();
    }

    public static IDataReaderMapper<T> GetMapper<T>(
      KeyValuePair<string, ITypeMap>[] mapping,
      CallableDelegate<T> factory,
      string prefix) {
      return new DataReaderMapperBuilder<T>(prefix)
        .Map(mapping)
        .SetFactory(factory)
        .Build();
    }

    public static IDataReaderMapper<T> GetMapper<T>(
      CallableDelegate<KeyValuePair<string, ITypeMap>[]> mapping,
      CallableDelegate<T> factory) {
      return GetMapper(mapping, factory);
    }

    public static IDataReaderMapper<T> AutoMapper<T>() {
      return AutoMapper<T>(typeof (T).Namespace);
    }

    public static IDataReaderMapper<T> AutoMapper<T>(string prefix) {
      return new DataReaderMapperBuilder<T>(prefix)
        .AutoMap()
        .Build();
    }
  }
}
