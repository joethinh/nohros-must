using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// The real type of the object that derivates from the <see cref="Value"/> class.
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// A null reference.
        /// </summary>
        TYPE_NULL = 0,

        /// <summary>
        /// Types used to store boolean values, true and false.
        /// </summary>
        TYPE_BOOLEAN = 1,

        /// <summary>
        /// Types used to store integral types.
        /// </summary>
        TYPE_INTEGER = 2,

        /// <summary>
        /// Types used to store doubles types.
        /// </summary>
        TYPE_REAL = 3,

        /// <summary>
        /// Types used to store strings.
        /// </summary>
        TYPE_STRING = 4,

        /// <summary>
        /// A <see cref="DictionaryValue"/> type.
        /// </summary>
        TYPE_DICTIONARY = 5,

        /// <summary>
        /// A <see cref="ListValue.cs"/> type.
        /// </summary>
        TYPE_LIST = 6,

        /// <summary>
        /// A <see cref="GenericValue.cs"/> type.
        /// </summary>
        TYPE_GENERIC = 7,

        /// <summary>
        /// A custom type that implements the <see cref="IValue"/> interface.
        /// </summary>
        TYPE_CLASS = 8
    }
}
