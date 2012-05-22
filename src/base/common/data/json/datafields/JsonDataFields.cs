using System;
using System.Data;
using Nohros.Resources;

namespace Nohros.Data.Json
{
  /// <summary>
  /// A factory classes used to create instances of the
  /// <see cref="IJsonDataField"/> interface.
  /// </summary>
  public class JsonDataFields
  {
    /// <summary>
    /// Creates an instance of the <see cref="IJsonDataField"/> class that is
    /// related with the specified type.
    /// </summary>
    /// <param name="name">
    /// The name of the field.
    /// </param>
    /// <param name="position">
    /// The zero-based ordinal position of the field within a
    /// <see cref="IDataReader"/>.
    /// </param>
    /// <param name="type">
    /// The type that is related with the <see cref="IJsonDataField"/> to be
    /// created.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IJsonDataField"/>
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// A <see cref="IJsonDataField"/> that is related with the
    /// <paramref name="type"/> object could not be found.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="type"/> or <paramref name="name"/> are <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This method recognizes the following types: <see cref="String"/>,
    /// <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>,
    /// <see cref="Decimal"/>, <see cref="Single"/>.
    /// </remarks>
    public static IJsonDataField CreateDataField(string name, int position,
      Type type) {
      if (type == typeof (bool)) {
        return new JsonDataFieldBoolean(name, position);
      }
      if (type == typeof (decimal)) {
        return new JsonDataFieldDecimal(name, position);
      }
      if (type == typeof (double)) {
        return new JsonDataFieldDouble(name, position);
      }
      if (type == typeof (float)) {
        return new JsonDataFieldFloat(name, position);
      }
      if (type == typeof (int)) {
        return new JsonDataFieldInteger(name, position);
      }
      if (type == typeof (long)) {
        return new JsonDataFieldLong(name, position);
      }
      if (type == typeof (short)) {
        return new JsonDataFieldShort(name, position);
      }
      if (type == typeof (string)) {
        return new JsonDataFieldString(name, position);
      }

      throw new NotSupportedException(
        string.Format(StringResources.NotSupported_CannotCreateType, type.Name));
    }
  }
}
