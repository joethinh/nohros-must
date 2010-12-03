using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Nohros.Data.Collections;
namespace Nohros.Data
{
    /// <summary>
    /// Represents a string that has embedded parameters.
    /// </summary>
    /// <remarks></remarks>
    public class ParameterizedString
    {
        string flat_string_;

        Queue<ParameterizedStringPart> parts_;
        ParameterizedStringPartCollection parameters_;

        #region .ctor
        /// <summary>
        /// Static initializer.
        /// </summary>
        static ParameterizedString() { }

        /// <summary>
        /// Initializes a new instance of the ParameterizedString class.
        /// </summary>
        public ParameterizedString():this(string.Empty, string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the ParameterizedString class by using the specified string and parameter delimiter.
        /// </summary>
        /// <param name="str">A string that can be made into a parameterized string.</param>
        /// <param name="delimiter">A string that delimits a parameter parameters in the
        /// parameterized string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="str"/> or <paramref name="delimiter"/> is a null reference.</exception>
        public ParameterizedString(string str, string delimiter) {
            if (str == null || delimiter == null)
                throw new ArgumentNullException((str == null) ? "str" : "delimiter");

            flat_string_ = str;
            parts_ = new Queue<ParameterizedStringPart>();
            parameters_ = new ParameterizedStringPartCollection();

            Parse(str, delimiter);
        }

        /// <summary>
        /// Initializes a new instance of the ParameterizedString class by using the provided list
        /// of parameter parts.
        /// </summary>
        /// <param name="parts">A list of <see cref="ParameterizedStringPart"/>object.</param>
        /// <param name="delimiter">A string that delimits a parameter in the
        /// parameterized string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parts"/> or <paramref name="delimiter"/> is null.</exception>
        public ParameterizedString(IEnumerable<ParameterizedStringPart> parts, string delimiter) {
            if (parts == null || delimiter == null)
                throw new ArgumentNullException((parts == null) ? "parts" : "delimiter");

            StringBuilder builder = new StringBuilder();
            foreach (ParameterizedStringPart part in parts) {
                parts_.Enqueue(part);
                if (part.IsParameter) {
                    builder.Append(delimiter).Append(part.LiteralValue).Append(delimiter);
                    parameters_.Add(part);
                    continue;
                }
                builder.Append(part.LiteralValue);
            }
            flat_string_ = builder.ToString();
        }
        #endregion

        /// <summary>
        /// Parses a string extracting the paramters and literal parts.
        /// </summary>
        /// <param name="str">A string with embedded parameters within it</param>
        /// <param name="delimiter">A string that delimits a parameter.</param>
        void Parse(string str, string delimiter) {
            int begin, end, length, i = 0;
            length = str.Length;

            while (i < length) {
                begin = NextDelimiterPos(str, delimiter, i);

                // no delimiter was found after position "i", put the last
                // literal into the stack.
                if (begin == -1) {
                    parts_.Enqueue(new ParameterizedStringPart(str.Substring(i)));
                    return;
                } else {
                    // save the literal part that comes before the starting delimiter, if exists.
                    if (begin - i > 0)
                        parts_.Enqueue(new ParameterizedStringPart(str.Substring(i, begin - i)));

                    i = begin + delimiter.Length; // next character after delimiter
                    end = NextDelimiterPos(str, delimiter, i);

                    // if the start delimiter is found but the end not, the string part is a literal.
                    // the string part must have at least one character between delimiters to be considered
                    // as a parameter.
                    if (end == -1 || end - begin - delimiter.Length == 0) {
                        // the delimiters are not part of the paramters, but it is not a parameter and
                        // we need to include the delimiter into it, so the "i" pointer must be decremented by
                        // len(delimiter).
                        parts_.Enqueue(new ParameterizedStringPart(str.Substring(i - delimiter.Length)));
                        return;
                    } else {
                        i = end + delimiter.Length; // next character after delimiter
                        ParameterizedStringPart part = new ParameterizedStringPart(str.Substring(begin + delimiter.Length, end - begin - delimiter.Length), string.Empty);
                        parts_.Enqueue(part);
                        parameters_.Add(part);
                    }
                }
            }
        }

        /// <summary>
        /// Reports the index of the first occurrence of the <see cref="delimiter"/> in the <paramref name="str"/>.
        /// The search starts at a specified character position.
        /// </summary>
        /// <param name="str">The string to seek.</param>
        /// <param name="delimiter"></param>
        /// <param name="start_index">The search stating positon.</param>
        /// <returns>The zero-based index position of the first character of <paramref name="delimiter"/> if that
        /// string is found, or -1 if it is not.</returns>
        int NextDelimiterPos(string str, string delimiter, int current_position) {
            int i = current_position - 1, k = 0;

            while (++i < str.Length) {
                if(str[i] == delimiter[0]) {
                    if (i + delimiter.Length - 1 < str.Length) {
                        while (++k < delimiter.Length) {
                            if (delimiter[k] != str[i + k])
                                return -1;
                        }
                        return i;
                    } else {
                        return -1;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets a collection of parameters associated with this <see cref="ParameterizedString"/> instance.
        /// </summary>
        public ParameterizedStringPartCollection Parameters {
            get { return parameters_; }
        }

        /// <summary>
        /// Serializes this instance into a string object.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            while (parts_.Count > 0)
                builder.Append(parts_.Dequeue().LiteralValue);
            return builder.ToString();
        }
    }
}
