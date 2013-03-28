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
      return GetMapper<T>(reader, mapping, typeof (T).Namespace);
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
    /// <param name="prefix">
    /// A string that can be used to distinguish two mappers that map the same
    /// class in distinct forms.
    /// </param>
    /// <param name="mapping">
    /// An array of <see cref="KeyValuePair{TKey,TValue}"/> containg the map
    /// between the columns of <paramref name="reader"/> and the properties
    /// of <typeparamref name="T"/>.
    /// </param>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      KeyValuePair<string, string>[] mapping, string prefix) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader, prefix);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IMapper{T}"/> that uses
    /// <paramref name="mapping"/> to map between the columns of
    /// <paramref name="reader"/> to the properties of <typeparamref name="T"/>.
    /// and the <paramref name="instantiator"/> to create an new instance of the
    /// <typeparamref name="T"/>.
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
    /// <param name="instantiator">
    /// A <see cref="CallableDelegate{T}"/> that can be used to create a new
    /// instance of the type <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      KeyValuePair<string, string>[] mapping, CallableDelegate<T> instantiator) {
      return GetMapper(reader, mapping, instantiator, typeof (T).Namespace);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IMapper{T}"/> that uses
    /// <paramref name="mapping"/> to map between the columns of
    /// <paramref name="reader"/> to the properties of <typeparamref name="T"/>.
    /// and the <paramref name="instantiator"/> to create an new instance of the
    /// <typeparamref name="T"/>.
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
    /// <param name="instantiator">
    /// A <see cref="CallableDelegate{T}"/> that can be used to create a new
    /// instance of the type <typeparamref name="T"/>.
    /// </param>
    /// <param name="prefix">
    /// A string that can be used to distinguish two mappers that map the same
    /// class in distinct forms.
    /// </param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      KeyValuePair<string, string>[] mapping, CallableDelegate<T> instantiator,
      string prefix) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader, instantiator);
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
    /// An array of <see cref="KeyValuePair{TKey,TValue}"/> containg the map
    /// between the columns of <paramref name="reader"/> and the properties
    /// of <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      KeyValuePair<string, ITypeMap>[] mapping) {
      return GetMapper<T>(reader, mapping, typeof (T).Namespace);
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
    /// An array of <see cref="KeyValuePair{TKey,TValue}"/> containg the map
    /// between the columns of <paramref name="reader"/> and the properties
    /// of <typeparamref name="T"/>.
    /// </param>
    /// <param name="prefix">
    /// A string that can be used to distinguish two mappers that map the same
    /// class in distinct forms.
    /// </param>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      KeyValuePair<string, ITypeMap>[] mapping, string prefix) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader, prefix);
    }

    public static IMapper<T> GetMapper<T>(IDataReader reader,
      KeyValuePair<string, ITypeMap>[] mapping, CallableDelegate<T> instantiator) {
      return GetMapper(reader, mapping, instantiator, typeof (T).Namespace);
    }

    public static IMapper<T> GetMapper<T>(IDataReader reader,
      KeyValuePair<string, ITypeMap>[] mapping, CallableDelegate<T> instantiator,
      string prefix) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader, instantiator, prefix);
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
      return GetMapper<T>(reader, mapping, typeof (T).Namespace);
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
    /// <param name="prefix">
    /// A string that can be used to distinguish two mappers that map the same
    /// class in distinct forms.
    /// </param>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      CallableDelegate<KeyValuePair<string, string>[]> mapping, string prefix) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader, prefix);
    }

    public static IMapper<T> GetMapper<T>(IDataReader reader,
      CallableDelegate<KeyValuePair<string, string>[]> mapping,
      CallableDelegate<T> instantiator) {
      return GetMapper(reader, mapping, instantiator, typeof (T).Namespace);
    }

    public static IMapper<T> GetMapper<T>(IDataReader reader,
      CallableDelegate<KeyValuePair<string, string>[]> mapping,
      CallableDelegate<T> instantiator, string prefix) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader, instantiator, prefix);
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
      CallableDelegate<KeyValuePair<string, ITypeMap>[]> mapping) {
      return GetMapper<T>(reader, mapping, typeof (T).Namespace);
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
    /// <param name="prefix">
    /// A string that can be used to distinguish two mappers that map the same
    /// class in distinct forms.
    /// </param>
    public static IMapper<T> GetMapper<T>(IDataReader reader,
      CallableDelegate<KeyValuePair<string, ITypeMap>[]> mapping, string prefix) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader, prefix);
    }

    public static IMapper<T> GetMapper<T>(IDataReader reader,
      CallableDelegate<KeyValuePair<string, ITypeMap>[]> mapping,
      CallableDelegate<T> instantiator) {
      return new DataReaderMapper<T>
        .Builder(mapping)
        .Build(reader, instantiator);
    }

    public static IMapper<T> GetMapper<T>(IDataReader reader) {
      return GetMapper<T>(reader, typeof (T).Namespace);
    }

    public static IMapper<T> GetMapper<T>(IDataReader reader, string prefix) {
      return new DataReaderMapper<T>
        .Builder()
        .Build(reader, prefix);
    }

    public static IChainMapper<T, T1> GetMapper<T, T1>(IDataReader reader,
      KeyValuePair<string, string>[] mapping_for_t,
      KeyValuePair<string, string>[] mapping_for_t1) where T1 : IMapper<T1> {
      DataReaderMapper<T> mapper_for_t = new DataReaderMapper<T>
        .Builder(mapping_for_t)
        .Build(reader);
      return new ChainDataReaderMapper<T, T1>(mapper_for_t);
    }
  }
}
