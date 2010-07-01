using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    public interface IDataTransferObject
    {
        /// <summary>
        /// Gets a JSON-compliant string of characters that represents the underlying class and
        /// is formatted like a JSON array element.
        /// </summary>
        /// <example>
        ///     <code>
        ///         [ToJsElement(), "somedata", ...]
        ///     </code>
        /// </example>
        /// <returns>A JSON-compliant string of characters formatted like a JSON array element.</returns>
        /// <remarks>
        /// JSON spec: "http://www.ietf.org/rfc/rfc4627.txt"
        /// The returned string will be scaped with quotation marks.
        /// </remarks>
        string ToJsElement();

        /// <summary>
        /// Gets a JSON-compliant string of characters that represents the underlying class
        /// and is formmated like a JSON object.
        /// </summary>
        /// <example>
        ///     <code>
        ///         { "name": "nohros systems", "surname": "nohros" }
        ///     </code>
        /// </example>
        /// <returns>A JSON-compliant string of characters formatted like a JSON object.</returns>
        /// <remarks>
        /// JSON spec: "http://www.ietf.org/rfc/rfc4627.txt"
        /// The strings inside the object will be escaped with quotation marks.
        /// </remarks>
        string ToJsObject();
    }
}
