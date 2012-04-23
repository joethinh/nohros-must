using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Nohros.Collections;
namespace Nohros
{
  /// <summary>
  /// Represents a string that has embedded parameters.
  /// </summary>
  /// <remarks></remarks>
  public class ParameterizedString
  {
    protected const string kDefaultDelimiter = "$";

    /// <summary>
    /// The original version of parameterized string.
    /// </summary>
    protected string flat_string_;

    readonly string delimiter_;
    readonly Queue<ParameterizedStringPart> parts_;
    readonly ParameterizedStringPartCollection parameters_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedString"/>
    /// class.
    /// </summary>
    /// <remarks>
    /// Implementors should initialize the <see cref="flat_string_"/> member.
    /// </remarks>
    protected ParameterizedString() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedString"/>
    /// class.
    /// </summary>
    public ParameterizedString(string str) : this(str, kDefaultDelimiter) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedString"/>
    /// class by using the specified parameterized string and parameter
    /// delimiter.
    /// </summary>
    /// <param name="str">A string that contains literal text and paramters
    /// delimited by <paramref name="delimiter"/>.</param>
    /// <param name="delimiter">A string that delimits a parameter within the
    /// given <paramref name="str"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="str"/> or
    /// <paramref name="delimiter"/> is <c>null</c>.</exception>
    public ParameterizedString(string str, string delimiter) {
      if (str == null || delimiter == null)
        throw new ArgumentNullException((str == null) ? "str" : "delimiter");

      delimiter_ = delimiter;
      flat_string_ = str;
      parts_ = new Queue<ParameterizedStringPart>();
      parameters_ = new ParameterizedStringPartCollection();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedString"/>
    /// class by using the provided list of
    /// <see cref="ParameterizedStringPart"/>
    /// </summary>
    /// <param name="parts">A list of <see cref="ParameterizedStringPart"/>
    /// objects that compose this instance.</param>
    /// <param name="delimiter">The delimiter used to delimits the parameters.
    /// within the string.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parts"/> or
    /// <paramref name="delimiter"/> is null.</exception>
    public static ParameterizedString FromParameterizedStringPartCollection(
      IEnumerable<ParameterizedStringPart> parts, string delimiter) {
      if (parts == null || delimiter == null)
        throw new ArgumentNullException((parts == null) ? "parts" : "delimiter");

      ParameterizedString str = new ParameterizedString();

      StringBuilder builder = new StringBuilder();
      foreach (ParameterizedStringPart part in parts) {
        str.parts_.Enqueue(part);
        if (part.IsParameter) {
          builder.Append(delimiter).Append(part.LiteralValue).Append(delimiter);
          str.parameters_.Add(part);
          continue;
        }
        builder.Append(part.LiteralValue);
      }
      str.flat_string_ = builder.ToString();

      return str;
    }
    #endregion

    /// <summary>
    /// Parses a string extracting the paramters and literal parts.
    /// </summary>
    public virtual void Parse() {
      int begin, end, length, i = 0;
      length = flat_string_.Length;

      while (i < length) {
        begin = NextDelimiterPos(flat_string_, delimiter_, i);

        // no delimiter was found after position "i", put the last
        // literal into the stack.
        if (begin == -1) {
          parts_.Enqueue(
            new ParameterizedStringPart(flat_string_.Substring(i)));
          return;
        } else {
          // save the literal part that comes before the starting delimiter,
          // if it exists.
          if (begin - i > 0)
            parts_.Enqueue(
              new ParameterizedStringPart(
                flat_string_.Substring(i, begin - i)));

          i = begin + delimiter_.Length; // next character after delimiter
          end = NextDelimiterPos(flat_string_, delimiter_, i);

          // If the start delimiter is found but the end not, the string part
          // is a literal. The string part must have at least one character
          // between delimiters to be considered as a parameter.
          if (end == -1 || end - begin - delimiter_.Length == 0) {
            // the delimiters are not part of the parameters, but it is not a
            // parameter and we need to include the delimiter into it, so the
            // "i" pointer must be decremented by len(delimiter).
            parts_.Enqueue(
              new ParameterizedStringPart(
                flat_string_.Substring(i - delimiter_.Length)));
            return;
          } else {
            i = end + delimiter_.Length; // next character after delimiter
            ParameterizedStringPart part = new ParameterizedStringPart(
              flat_string_.Substring(
                begin + delimiter_.Length, end - begin - delimiter_.Length),
              string.Empty);
            parts_.Enqueue(part);
            parameters_.Add(part);
          }
        }
      }
    }

    /// <summary>
    /// Reports the index of the first occurrence of the
    /// <paramref name="delimiter"/> in the <paramref name="str"/>.The search
    /// starts at a specified character position.
    /// </summary>
    /// <param name="str">The string to seek.</param>
    /// <param name="delimiter"></param>
    /// <param name="current_position">The search stating positon.</param>
    /// <returns>The zero-based index position of the first character of
    /// <paramref name="delimiter"/> if that string is found, or -1 if it is
    /// not.</returns>
    int NextDelimiterPos(string str, string delimiter, int current_position) {
      int i = current_position - 1, k = 0;

      while (++i < str.Length) {
        if (str[i] == delimiter[0]) {
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
    /// Gets a collection of parameters associated with this instance.
    /// </summary>
    public ParameterizedStringPartCollection Parameters {
      get { return parameters_; }
    }

    /// <summary>
    /// Serializes this instance into a string object.
    /// </summary>
    /// <returns>A string representation of this instance.</returns>
    public override string ToString() {
      StringBuilder builder = new StringBuilder();
      while (parts_.Count > 0)
        builder.Append(parts_.Dequeue().LiteralValue);
      return builder.ToString();
    }
  }
}
