using System;

using System.Data;

namespace Nohros.Data.Json
{
  /// <summary>
  /// A <see cref="IJsonDataField"/> represents a relationship between a data
  /// field (a field retrieved from a <see cref="IDataReader"/>) and a
  /// <see cref="IJsonToken"/> object.
  /// </summary>
  /// <remarks>
  /// A <see cref="IJsonToken"/> can improve the performance of operations that
  /// need to instantiate <see cref="IJsonToken"/> objects from a generic
  /// <see cref="IDataReader"/>.
  /// <example>
  /// The sample below demonstrate the gain of performance that is obtained by
  /// the use of the <see cref="IJsonDataField"/>.
  /// <code>
  /// // Get the data fields once using the information of the reader.
  /// IJsonDataField[] fields = GetDataFields(IDataReader reader);
  /// 
  /// // Read the rows from the data reader and dynamically crete the right
  /// // json token.
  /// int length = fields.Length;
  /// while (reader.Read()) {
  ///   for (int i = 0, i &lt; length; i++) {
  ///     IJsonDataField field = fields[i];
  ///     
  ///     // Get the right json token. Note that we do not need to check the
  ///     // type of the field to get, the field object already know the right
  ///     // type.
  ///     IJsonToken token = field.GetJsonToken(reader, i);
  /// 
  ///     // do something with the token.
  ///   }
  /// }
  /// 
  /// IJsonDataField[] GetDataFields(IDataReader reader) {
  ///   int field_count = reader.FieldCount;
  ///   IJsonDataField[] fields = new IJsonDataField[field_count];
  ///   for (int i = 0, i &lt; field_count; i++) {
  ///     fields[i] = FUNCTION_TO_GET_JSON_DATA_FIELD_FROM_FIELD_TYPE(
  ///       reader.GetFieldType(i));
  ///   }
  ///   return field[i];
  /// }
  /// </code>
  /// </example>
  /// </remarks>
  public interface IJsonDataField : IDataField<IJsonToken>
  {
  }
}
