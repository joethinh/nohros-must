using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
  /// <summary>
  /// Represents an class that can be serialized using the json format. Classes
  /// that implement this interface is usually used to transfer json encoded
  /// data from the data access layer to another layer (usually a web
  /// front-end).
  /// </summary>
  public interface IDataTransferObject
  {
    /// <summary>
    /// Gets a json-compliant string of characters that represents the
    /// underlying class and is formatted like a json array element.
    /// </summary>
    /// <example>
    /// The example uses the AsJsonArray to serialize the MyDataTransferObject
    /// to a string that represents an json array.
    /// <code>
    ///   class MyDataTransferObject : IDataTransferObject {
    ///     string first_name_, las_name;
    ///     
    ///     public MyDataTransferObject(string first_name, string last_name) {
    ///       first_name_ = first_name;
    ///       last_name_ = last_name;
    ///     }
    /// 
    ///     public string AsJsonArray() {
    ///       return "['" + first_name + "','" + last_name + "']"
    ///     }
    ///   }
    /// </code>
    /// </example>
    /// <returns>
    /// A json compliant string of characters formatted like a json array
    /// element.
    /// </returns>
    string AsJsonArray();

    /// <summary>
    /// Gets a json-compliant string of characters that represents the
    /// underlying class and is formatted like a json object.
    /// </summary>
    /// <example>
    /// The example uses the AsJsonObject method to serialize the
    /// MyDataTransferObject to a string that represents an json object.
    /// <code>
    ///   class MyDataTransferObject : IDataTransferObject {
    ///     string first_name_, las_name;
    ///     
    ///     public MyDataTransferObject(string first_name, string last_name) {
    ///       first_name_ = first_name;
    ///       last_name_ = last_name;
    ///     }
    /// 
    ///     public string AsJsonObject() {
    ///       return "{\"first_name\":\"" + first_name + "\",\"last_name\":\"" + last_name + "\"}";
    ///     }
    ///   }
    /// </code>
    /// </example>
    /// <returns>
    /// A json compliant string of characters formatted like a json object.
    /// </returns>
    string AsJsonObject();
  }
}
