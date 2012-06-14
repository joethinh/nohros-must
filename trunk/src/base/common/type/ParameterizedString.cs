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

    readonly string delimiter_;
    readonly ParameterizedStringPartParameterCollection parameters_;
    readonly List<ParameterizedStringPart> parts_;

    /// <summary>
    /// The original version of parameterized string.
    /// </summary>
    protected string flat_string;

    /// <summary>
    /// A value that indicates if spaces should be used to terminate paramter
    /// parsing.
    /// </summary>
    protected bool use_space_as_terminator;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedString"/>
    /// class.
    /// </summary>
    /// <remarks>
    /// Implementors should initialize the <see cref="flat_string"/> member.
    /// </remarks>
    protected ParameterizedString() : this(string.Empty) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedString"/>
    /// class.
    /// </summary>
    public ParameterizedString(string str) : this(str, kDefaultDelimiter) {
    }

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
      if (str == null || delimiter == null) {
        throw new ArgumentNullException((str == null) ? "str" : "delimiter");
      }

      delimiter_ = delimiter;
      flat_string = str;
      parts_ = new List<ParameterizedStringPart>();
      parameters_ = new ParameterizedStringPartParameterCollection();
      use_space_as_terminator = false;
    }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedString"/>
    /// class by using the provided list of
    /// <see cref="ParameterizedStringPart"/>
    /// </summary>
    /// <param name="parts">A list of <see cref="ParameterizedStringPart"/>
    /// objects that compose this instance.</param>
    /// <param name="delimiter">The delimiter used to delimits the parameters.
    /// within the string.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parts"/> or
    /// <paramref name="delimiter"/> is <c>null</c></exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="parts"/> contains one or more <c>null</c> elements.
    /// </exception>
    public static ParameterizedString FromParameterizedStringPartCollection(
      IEnumerable<ParameterizedStringPart> parts, string delimiter) {
      if (parts == null || delimiter == null) {
        throw new ArgumentNullException((parts == null)
          ? "parts"
          : "delimiter");
      }

      ParameterizedString str = new ParameterizedString();

      StringBuilder builder = new StringBuilder();
      foreach (ParameterizedStringPart part in parts) {
        if (part == null) {
          throw new ArgumentException("part");
        }

        str.parts_.Add(part);
        if (part.IsParameter) {
          builder.Append(delimiter).Append(part.Value).Append(delimiter);
          str.parameters_.AddIfAbsent((ParameterizedStringPartParameter) part);
          continue;
        }
        builder.Append(part.Value);
      }
      str.flat_string = builder.ToString();

      return str;
    }

    /// <summary>
    /// Parses a string extracting the paramters and literal parts.
    /// </summary>
    public virtual void Parse() {
      int begin, end, length, i = 0;
      length = flat_string.Length;

      while (i < length) {
        // get the paramater begin position, note that spaces could be
        // only used as parameter terminator.
        begin = NextDelimiterPos(
          flat_string, delimiter_, false, i);

        // no delimiter was found after position "i", put the last
        // literal into the stack.
        if (begin == -1) {
          parts_.Add(
            new ParameterizedStringPartLiteral(flat_string.Substring(i)));
          break;
        }

        // save the literal part that comes before the starting delimiter,
        // if it exists.
        if (begin - i > 0) {
          parts_.Add(
            new ParameterizedStringPartLiteral(
              flat_string.Substring(i, begin - i)));
        }

        i = begin + delimiter_.Length; // next character after delimiter
        end = NextDelimiterPos(
          flat_string, delimiter_, use_space_as_terminator, i);

        // If the start delimiter is found but the end not, the string part
        // is a literal. The string part must have at least one character
        // between delimiters to be considered as a parameter.
        if (end == -1 || end - begin - delimiter_.Length == 0) {
          // the delimiters are not part of the parameters, but it is not a
          // parameter and we need to include the delimiter into it, so the
          // "i" pointer must be decremented by len(delimiter).
          parts_.Add(
            new ParameterizedStringPartLiteral(
              flat_string.Substring(i - delimiter_.Length)));
          break;
        }

        // point "i" to next character after delimiter, not consider
        // spaces.
        if (use_space_as_terminator && end < flat_string.Length &&
          flat_string[end] == ' ') {
          i = end;
        } else {
          i = end + delimiter_.Length;
        }

        ParameterizedStringPartParameter part =
          new ParameterizedStringPartParameter(
            flat_string.Substring(
              begin + delimiter_.Length, end - begin - delimiter_.Length),
            string.Empty);
        parts_.Add(part);
        parameters_.AddIfAbsent(part);
      }
    }

    /// <summary>
    /// Reports the index of the first occurrence of the
    /// <paramref name="delimiter"/> in the <paramref name="str"/>.The search
    /// starts at a specified character position.
    /// </summary>
    /// <param name="str">
    /// The string to seek.
    /// </param>
    /// <param name="delimiter"></param>
    /// <param name="current_position">
    /// The search starting positon.
    /// </param>
    /// <param name="use_space_as_delimiter">
    /// </param>
    /// <returns>
    /// The zero-based index position of the first character of
    /// <paramref name="delimiter"/> if that string is found, or -1 if it is
    /// not.
    /// </returns>
    int NextDelimiterPos(string str, string delimiter,
      bool use_space_as_delimiter, int current_position) {
      int i = current_position - 1, k = 0;

      while (++i < str.Length) {
        if (use_space_as_delimiter && str[i] == ' ') {
          return i;
        }

        if (str[i] == delimiter[0]) {
          if (i + delimiter.Length - 1 < str.Length) {
            while (++k < delimiter.Length) {
              if (delimiter[k] != str[i + k])
                return -1;
            }
            return i;
          }
          return -1;
        }
      }
      return (use_space_as_delimiter && str.Length == i) ? i : -1;
    }

    /// <summary>
    /// Serializes this instance into a string object.
    /// </summary>
    /// <returns>
    /// A string representation of this instance.
    /// </returns>
    /// <remarks>
    /// If the parameterized string is not parsed yet a empty string will be
    /// returned.
    /// </remarks>
    public override string ToString() {
      StringBuilder builder = new StringBuilder();
      foreach(ParameterizedStringPart part in parts_) {
        builder.Append(part.Value);
      }
      return builder.ToString();
    }

    /// <summary>
    /// Gets a collection of parameters associated with this instance.
    /// </summary>
    public ParameterizedStringPartParameterCollection Parameters {
      get { return parameters_; }
    }

    /// <summary>
    /// Gets a value indicating if the space should be used as the parameter
    /// terminator in conjuction with the specified delimiter.
    /// </summary>
    /// <remarks>
    /// When this property is set to true a parameter will be identified by
    /// a string enclosed between two delimiters or a delimiter and a space.
    /// The "name" and "name_with_space" of the string above is considered
    /// parameters when this property is true:
    /// <para>
    ///   "The $name$ is a parameter and $name_with_space is a a parameter too."
    /// </para>
    /// <para>
    /// Note that the space is used only as parameter terminator delimiter.
    /// </para>
    /// </remarks>
    public bool UseSpaceAsTerminator {
      get { return use_space_as_terminator; }
      set { use_space_as_terminator = value; }
    }
  }
}
