using System;
using System.Data;
using Nohros.Resources;

namespace Nohros.Data.Json
{
  /// <summary>
  /// The default implementation of the <see cref="IJsonCollectionFactory"/>
  /// interface. A <see cref="JsonCollectionFactory"/> should be used to
  /// create instances of built-in json collections.
  /// </summary>
  public class JsonCollectionFactory : IJsonCollectionFactory
  {
    const string kJsonTableCollection = "table";
    const string kJsonObjectCollection = "object";
    const string kArrayCollection = "array";

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonCollectionFactory"/>
    /// class.
    /// </summary>
    public JsonCollectionFactory() {
    }
    #endregion

    #region IJsonCollectionFactory Members
    /// <inheritdoc/>
    public IJsonCollection CreateJsonCollection(string name) {
      switch (name) {
        case kJsonObjectCollection:
          return new JsonObject();

        case kArrayCollection:
          return new JsonArray();

        case kJsonTableCollection:
          return new JsonTable();

        default:
          throw new NotSupportedException(string.Format(
            StringResources.NotSupported_CannotCreateType, name));
      }
    }

    /// <inheritdoc/>
    public IJsonCollection CreateJsonCollection(string name, IDataReader reader) {
      switch (name) {
        case kJsonObjectCollection:
          return CreateJsonObject(reader);

        case kArrayCollection:
          return CreateJsonArray(reader);

        case kJsonTableCollection:
          return CreateJsonTable(reader);

        default:
          throw new NotSupportedException(string.Format(
            StringResources.NotSupported_CannotCreateType, name));
      }
    }
    #endregion

    /// <summary>
    /// Creates a <see cref="JsonObject"/> containing the data readed from
    /// <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> object that contains the data that will be
    /// used to populate a <see cref="JsonObject"/>.
    /// </param>
    /// <returns>
    /// A <see cref="JsonObject"/> object that contains the data readed from
    /// the <paramref name="reader"/>.
    /// </returns>
    public JsonObject CreateJsonObject(IDataReader reader) {
      JsonObject json_object = new JsonObject();
      if (reader.Read()) {
        IJsonDataField[] json_data_fields = GetJsonDataFields(reader);
        int length = json_data_fields.Length;
        do {
          for (int i = 0, j = length; i < j; i++) {
            IJsonDataField json_data_field = json_data_fields[i];
            JsonObject.JsonMember json_object_member =
              new JsonObject.JsonMember(json_data_field.Name,
                json_data_field.GetValue(reader));
            json_object.Add(json_object_member);
          }
        } while (reader.Read());
      }
      return json_object;
    }

    /// <summary>
    /// Creates a <see cref="JsonArray"/> containing the data readed from
    /// <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> object that contains the data that will be
    /// used to populate a <see cref="JsonArray"/>.
    /// </param>
    /// <returns>
    /// A <see cref="JsonArray"/> object that contains the data readed from
    /// the <paramref name="reader"/>.
    /// </returns>
    public JsonArray CreateJsonArray(IDataReader reader) {
      JsonArray json_array = new JsonArray();
      if (reader.Read()) {
        IJsonDataField[] json_data_fields = GetJsonDataFields(reader);
        do {
          int length = json_data_fields.Length;
          for (int i = 0, j = length; i < j; i++) {
            IJsonDataField json_data_field = json_data_fields[i];
            json_array.Add(json_data_field.GetValue(reader));
          }
        } while (reader.Read());
      }
      return json_array;
    }

    /// <summary>
    /// Creates a <see cref="JsonTable"/> containing the data readed from
    /// <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> object that contains the data that will be
    /// used to populate a <see cref="JsonTable"/>.
    /// </param>
    /// <returns>
    /// A <see cref="JsonTable"/> object that contains the data readed from
    /// the <paramref name="reader"/>.
    /// </returns>
    public JsonTable CreateJsonTable(IDataReader reader) {
      if (reader.Read()) {
        IJsonDataField[] json_data_fields = GetJsonDataFields(reader);
        int length = json_data_fields.Length;

        // Get the column names for the json table.
        string[] columns = new string[length];
        for (int i = 0, j = length; i < j; i++) {
          columns[i] = json_data_fields[i].Name;
        }

        JsonTable json_table = new JsonTable(columns);

        // Read the values into a sequence of json arrays and append it to
        // the json table.
        do {
          JsonArray json_array = new JsonArray();
          for (int i = 0, j = length; i < j; i++) {
            IJsonDataField json_data_field = json_data_fields[i];
            json_array.Add(json_data_field.GetValue(reader));
          }
          json_table.Add(json_array);
        } while (reader.Read());
      }
      return new JsonTable();
    }

    IJsonDataField[] GetJsonDataFields(IDataReader reader) {
      int count = reader.FieldCount;
      IJsonDataField[] json_data_fields = new IJsonDataField[count];
      for (int i = 0; i < count; i++) {
        json_data_fields[i] = JsonDataFields.CreateDataField(reader.GetName(i),
          i, reader.GetFieldType(i));
      }
      return json_data_fields;
    }
  }
}
