using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data.TransferObjects
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
    ///   <code>
    ///     [ToJsElement(), "somedata", ...]
    ///   </code>
    /// </example>
    /// <returns>
    /// A json compliant string of characters formatted like a json array
    /// element.
    /// </returns>
    /// <remarks>
    /// The returned string will be scaped with quotation marks.
    /// </remarks>
    string AsJsonArray();

    /// <summary>
    /// Gets a json compliant string of characters that represents the
    /// underlying class and is formmated like a json object.
    /// </summary>
    /// <example>
    ///   <code>
    ///     { "name": "nohros systems", "surname": "nohros" }
    ///   </code>
    /// </example>
    /// <returns>
    /// A json compliant string of characters formatted like a json object.
    /// </returns>
    /// <remarks>
    /// The strings inside the object will be escaped with quotation marks.
    /// </remarks>
    string AsJsonObject();
  }
}
