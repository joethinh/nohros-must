using System;
using System.Data;

using Nohros.Configuration;

namespace Nohros.Data.Json
{
  /// <summary>
  /// A factory used to create instances of the <see cref="IJsonCollection"/>
  /// class.
  /// </summary>
  /// <remarks>
  /// This interface implies a constructor that receives a
  /// <see cref="Nohros.Configuration.IConfiguration"/> object or a constructor with no
  /// parameters. The constructor that receives a
  /// <see cref="Nohros.Configuration.IConfiguration"/> should be the preferred constructor to
  /// be use for instantiate the <see cref="IJsonCollectionFactory"/> class.
  /// </remarks>
  public interface IJsonCollectionFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="IJsonCollection"/> object by
    /// using the specified <paramref name="name"/> and
    /// <paramref name="reader"/>.
    /// </summary>
    /// <param name="name">
    /// A string that identifies the name of the json collection to be created.
    /// The meaning of this argument is factory specific.
    /// </param>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> object that contains the data that will be
    /// used to populate the <see cref="IJsonCollection"/>.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IJsonCollection"/> populated with the
    /// data readed from the <paramref name="reader"/>.
    /// </returns>
    IJsonCollection CreateJsonCollection(string name, IDataReader reader);

    /// <summary>
    /// Creates an instance of the <see cref="IJsonCollection"/> object by
    /// using the specified collection <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// A string that identifies the name of the json collection to be created.
    /// The meaning of this argument is factory specific.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IJsonCollection"/> object.
    /// </returns>
    IJsonCollection CreateJsonCollection(string name);
  }
}
