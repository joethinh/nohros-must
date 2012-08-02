using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data.Json
{
  /// <summary>
  /// A <see cref="JsonStringBuilder"/> is a builder that is used to build
  /// string-like json elements.
  /// </summary>
  /// <remarks>
  /// This class is a simple builder and should be used only to simplify the
  /// json serialization process. The Write(..) methods do not perform any
  /// validation of the correctness of the JSON format, it simple writes the
  /// data (escaping when nescessary). We do not guarantee that the final
  /// string is a valid json string, but we can ensure that each Write(..)
  /// method writes a valid json element.
  /// </remarks>
  public class JsonStringBuilder
  {
    #region State
    enum State
    {
      None = 0,
      ReservedBeginToken = 1,
      ReservedEndToken = 2,
      ContentToken = 3
    }
    #endregion

    const string kBeginObject = "{";
    const string kEndObject = "}";
    const string kBeginArray = "[";
    const string kEndArray = "]";
    const string kNameSeparator = ":";
    const string kValueSeparator = ",";
    const string kDefaultNumberFormat = "G";

    const char kLargestEscapableChar = '\\';
    static readonly char[] escape_chars_ = new char[]
    {'"', '\n', '\t', '\\', '\f', '\b'};

    readonly StringBuilder string_builder_;

    State current_state_;
    int last_written_token_begin_;
    int last_written_token_end_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonStringBuilder"/> class.
    /// </summary>
    public JsonStringBuilder() {
      string_builder_ = new StringBuilder();
      current_state_ = State.None;
    }
    #endregion

    /// <summary>
    /// Appends the begin array token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteBeginArray() {
      return WriteReservedBeginToken(kBeginArray);
    }

    /// <summary>
    /// Appends the end array token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteEndArray() {
      return WriteReservedEndToken(kEndArray);
    }

    /// <summary>
    /// Appends the begin object token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteBeginObject() {
      return WriteReservedBeginToken(kBeginObject);
    }

    /// <summary>
    /// Appends the end object token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteEndObject() {
      return WriteReservedEndToken(kEndObject);
    }

    /// <summary>
    /// Appends the string <paramref name="value"/> to the current json string.
    /// </summary>
    /// <remarks>
    /// This method encloses the string in a double quotes.
    /// <paramref name="value"/>.
    /// </remarks>
    public JsonStringBuilder WriteString(string value) {
      return WriteContentToken("\"" + (value ?? "null") + "\"");
    }

    /// <summary>
    /// Appends the string <paramref name="value"/> to the current json string.
    /// </summary>
    /// <remarks>
    /// This method write the specified string as is, which means that it do
    /// not encloses the string in a double quotes.
    /// </remarks>
    public JsonStringBuilder WriteUnquotedString(string value) {
      return WriteContentToken(value ?? "null");
    }

    public JsonStringBuilder WriteStringArray(string[] data) {
      WriteBeginArray();
      for (int i = 0, j = data.Length; i < j; i++) {
        WriteString(data[i]);
      }
      return WriteEndArray();
    }

    public JsonStringBuilder WriteTokenArray(IJsonToken[] tokens) {
      WriteBeginArray();
      for (int i = 0, j = tokens.Length; i < j; i++) {
        WriteUnquotedString(tokens[i].AsJson());
      }
      return WriteEndArray();
    }

    public JsonStringBuilder WriteTokenArray(IEnumerable<IJsonToken> tokens) {
      WriteBeginArray();
      foreach (IJsonToken token in tokens) {
        WriteUnquotedString(token.AsJson());
      }
      return WriteEndArray();
    }

    /// <summary>
    /// Appends the string representation of the integer
    /// <paramref name="value"/> to the current json string.
    /// </summary>
    /// <param name="value">
    /// The integer value to be appended to the current json string.
    /// </param>
    public JsonStringBuilder WriteNumber(int value) {
      return WriteNumber(value, kDefaultNumberFormat);
    }

    /// <summary>
    /// Writes the string representation of the long
    /// <paramref name="value"/> to the current json string.
    /// </summary>
    /// <param name="value">
    /// The long value to be appended to the current json string.
    /// </param>
    public JsonStringBuilder WriteNumber(long value) {
      return WriteNumber(value, kDefaultNumberFormat);
    }

    /// <summary>
    /// Writes the string representation of the double
    /// <paramref name="value"/> to the current json string.
    /// </summary>
    /// <param name="value">
    /// The double value to be appended to the current json string.
    /// </param>
    public JsonStringBuilder WriteNumber(double value) {
      return WriteNumber(value, kDefaultNumberFormat);
    }

    /// <summary>
    /// Writes the string representation of the integer
    /// <paramref name="value"/> to the current json string using the specified
    /// format.
    /// </summary>
    /// <param name="value">
    /// The integer value to be appended to the current json string.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string.
    /// </param>
    /// <remarks>
    /// This method does not check if the specified format is a valid json
    /// format.
    /// </remarks>
    public JsonStringBuilder WriteNumber(int value, string format) {
      return WriteContentToken(value.ToString(format));
    }

    /// <summary>
    /// Writes the string representation of the long
    /// <paramref name="value"/> to the current json string using the specified
    /// format.
    /// </summary>
    /// <param name="value">
    /// The long value to be appended to the current json string.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string.
    /// </param>
    /// <exception cref="FormatException">
    /// <paramref name="format"/> is invalid or not supported.
    /// </exception>
    /// <remarks>
    /// This method does not check if the specified format is a valid json
    /// format.
    /// </remarks>
    public JsonStringBuilder WriteNumber(long value, string format) {
      return WriteContentToken(value.ToString(format));
    }

    /// <summary>
    /// Writes the string representation of the double
    /// <paramref name="value"/> to the current json string using the specified
    /// format.
    /// </summary>
    /// <param name="value">
    /// The double value to be appended to the current json string.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string.
    /// </param>
    /// <exception cref="FormatException">
    /// <paramref name="format"/> is invalid or not supported.
    /// </exception>
    /// <remarks>
    /// This method does not check if the specified format is a valid json
    /// format.
    /// </remarks>
    public JsonStringBuilder WriteNumber(double value, string format) {
      return WriteContentToken(value.ToString(format));
    }

    /// <summary>
    /// Appends the json string that represents a member which name is
    /// <paramref name="name"/> and value is <paramref name="value"/>.
    /// </summary>
    /// <param name="name">
    /// The name part of the json member.
    /// </param>
    /// <param name="value">
    /// The value part of the json member.
    /// </param>
    /// <remarks>
    /// This method encloses the name and value in a double quotes.
    /// <para>
    /// The string that will be append should be something like the string
    /// above:
    /// </para>
    /// <para>
    /// "name":"value"
    /// </para>
    /// </remarks>
    public JsonStringBuilder WriteMember(string name, string value) {
      return WriteContentToken(
        "\"" + name + "\"" + kNameSeparator + "\"" + value + "\"");
    }

    /// <summary>
    /// Appends the json string that represents a member which name is
    /// <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    /// The name part of the json member.
    /// </param>
    /// <remarks>
    /// This method encloses the name and in a double quotes.
    /// <para>
    /// The string that will be append should be something like the string
    /// above:
    /// </para>
    /// <para>
    /// "name":
    /// </para>
    /// </remarks>
    public JsonStringBuilder WriteMemberName(string name) {
      return WriteReservedBeginToken(
        "\"" + name + "\"" + kNameSeparator);
    }

    /// <summary>
    /// Appends the json string that represents a member which name is
    /// <paramref name="name"/> and value is <paramref name="value"/>.
    /// </summary>
    /// <param name="name">
    /// The name part of the json member.
    /// </param>
    /// <param name="value">
    /// The value part of the json member.
    /// </param>
    /// <remarks>
    /// This method encloses the name in a double quotes.
    /// <para>
    /// The string that will be append should be something like the string
    /// above:
    /// </para>
    /// <para>
    /// "name":"value"
    /// </para>
    /// </remarks>
    public JsonStringBuilder WriteMember(string name, int value) {
      return WriteMember(name, value, kDefaultNumberFormat);
    }

    /// <summary>
    /// Appends the json string that represents a member which name is
    /// <paramref name="name"/> and value is <paramref name="value"/>.
    /// </summary>
    /// <param name="name">
    /// The name part of the json member.
    /// </param>
    /// <param name="value">
    /// The value part of the json member.
    /// </param>
    /// <remarks>
    /// This method encloses the name in a double quotes.
    /// <para>
    /// The string that will be append should be something like the string
    /// above:
    /// </para>
    /// <para>
    /// "name":"value"
    /// </para>
    /// </remarks>
    public JsonStringBuilder WriteMember(string name, long value) {
      return WriteMember(name, value, kDefaultNumberFormat);
    }

    /// <summary>
    /// Appends the json string that represents a member which name is
    /// <paramref name="name"/> and value is <paramref name="value"/>.
    /// </summary>
    /// <param name="name">
    /// The name part of the json member.
    /// </param>
    /// <param name="value">
    /// The value part of the json member.
    /// </param>
    /// <remarks>
    /// This method encloses the name in a double quotes.
    /// <para>
    /// The string that will be append should be something like the string
    /// above:
    /// </para>
    /// <para>
    /// "name":"value"
    /// </para>
    /// </remarks>
    public JsonStringBuilder WriteMember(string name, double value) {
      return WriteMember(name, value, kDefaultNumberFormat);
    }

    /// <summary>
    /// Appends the json string that represents a member which name is
    /// <paramref name="name"/> and value is the string representation of
    /// <paramref name="value"/> formatted using the specified
    /// <paramref name="format"/>.
    /// </summary>
    /// <param name="name">
    /// The name part of the json member.
    /// </param>
    /// <param name="value">
    /// The value part of the json member.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string.
    /// </param>
    /// <remarks>
    /// This method encloses the name in a double quotes.
    /// <para>
    /// The string that will be append should be something like the string
    /// above:
    /// </para>
    /// <para>
    /// "name":"value"
    /// </para>
    /// <para>
    /// This method does not check if the specified format is a valid json
    /// format.
    /// </para>
    /// </remarks>
    public JsonStringBuilder WriteMember(string name, int value, string format) {
      return WriteContentToken("\"" + name + "\":" + value.ToString(format));
    }

    /// <summary>
    /// Appends the json string that represents a member which name is
    /// <paramref name="name"/> and value is the string representation of
    /// <paramref name="value"/> formatted using the specified
    /// <paramref name="format"/>.
    /// </summary>
    /// <param name="name">
    /// The name part of the json member.
    /// </param>
    /// <param name="value">
    /// The value part of the json member.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string.
    /// </param>
    /// <remarks>
    /// This method encloses the name in a double quotes.
    /// <para>
    /// The string that will be append should be something like the string
    /// above:
    /// </para>
    /// <para>
    /// "name":"value"
    /// </para>
    /// <para>
    /// This method does not check if the specified format is a valid json
    /// format.
    /// </para>
    /// </remarks>
    public JsonStringBuilder WriteMember(string name, long value, string format) {
      return WriteContentToken("\"" + name + "\":" + value.ToString(format));
    }

    /// <summary>
    /// Appends the json string that represents a member which name is
    /// <paramref name="name"/> and value is the string representation of
    /// <paramref name="value"/> formatted using the specified
    /// <paramref name="format"/>.
    /// </summary>
    /// <param name="name">
    /// The name part of the json member.
    /// </param>
    /// <param name="value">
    /// The value part of the json member.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string.
    /// </param>
    /// <remarks>
    /// This method encloses the name in a double quotes.
    /// <para>
    /// The string that will be append should be something like the string
    /// above:
    /// </para>
    /// <para>
    /// "name":"value"
    /// </para>
    /// <para>
    /// This method does not check if the specified format is a valid json
    /// format.
    /// </para>
    /// </remarks>
    public JsonStringBuilder WriteMember(string name, double value,
      string format) {
      return WriteContentToken("\"" + name + "\":" + value.ToString(format));
    }

    /// <summary>
    /// Escapes a minimal set of characters (\n,\\,\r,\t,",\f,\b) by replacing
    /// them with their escapes codes.
    /// </summary>
    /// <returns>The escaped version of <see cref="token"/></returns>
    public static string Escape(string token) {
      StringBuilder escaped = new StringBuilder();
      for (int i = 0, j = token.Length; i < j; i++) {
        char c = token[i];
        if (c < kLargestEscapableChar) {
          switch(c) {
            case '\n':
              escaped.Append("\\n");
              break;

            case '\r':
              escaped.Append("\\r");
              break;

            case '\t':
              escaped.Append("\\t");
              break;

            case '"':
            case '\\':
              escaped.Append("\\");
              escaped.Append(c);
              break;

            case '\f':
              escaped.Append("\\f");
              break;

            case '\b':
              escaped.Append("\\b");
              break;

            default:
              escaped.Append(c);
              break;
          }
        } else {
          escaped.Append(c);
        }
      }
      return escaped.ToString();
    }

    /// <summary>
    /// Escapes a minimal set of characters (\n,\\,\r,\t,",\f,\b) by replacing
    /// them with their escapes codes within the last written token.
    /// </summary>
    public JsonStringBuilder Escape() {
      for (int i = last_written_token_begin_, j = last_written_token_end_ ; i < j; i++) {
        char c = string_builder_[i];
        if (c < kLargestEscapableChar) {
          switch (c) {
            case '\n':
              string_builder_.Replace("\n", "\\n", i, 1);
              break;

            case '\r':
              string_builder_.Replace("\r", "\\r", i, 1);
              break;

            case '\t':
              string_builder_.Replace("\t", "\\t", i, 1);
              break;

            case '"':
              string_builder_.Replace("\"", "\\\"", i, 1);
              break;

            case '\\':
              string_builder_.Replace("\\", "\\\\", i, 1);
              break;

            case '\f':
              string_builder_.Replace("\f", "\\f", i, 1);
              break;

            case '\b':
              string_builder_.Replace("\b", "\\b", i, 1);
              break;
          }
        }
      }
      return this;
    }

    JsonStringBuilder WriteReservedBeginToken(string token) {
      last_written_token_begin_ = string_builder_.Length;
      switch (current_state_) {
        case State.None:
        case State.ReservedBeginToken:
        case State.ReservedEndToken:
          string_builder_.Append(token);
          break;
        case State.ContentToken:
          string_builder_.Append(kValueSeparator + token);
          break;
      }
      last_written_token_end_ = string_builder_.Length;
      current_state_ = State.ReservedBeginToken;
      return this;
    }

    JsonStringBuilder WriteReservedEndToken(string token) {
      last_written_token_begin_ = string_builder_.Length;
      string_builder_.Append(token);
      last_written_token_end_ = string_builder_.Length;
      current_state_ = State.ReservedEndToken;
      return this;
    }

    JsonStringBuilder WriteContentToken(string token) {
      last_written_token_begin_ = string_builder_.Length;
      switch (current_state_) {
        case State.None:
        case State.ReservedBeginToken:
          string_builder_.Append(token);
          break;
        case State.ReservedEndToken:
        case State.ContentToken:
          string_builder_.Append(kValueSeparator + token);
          break;
      }
      last_written_token_end_ = string_builder_.Length;
      current_state_ = State.ContentToken;
      return this;
    }

    /// <summary>
    /// Gets the string representing the current json string contained in
    /// this builder.
    /// </summary>
    public override string ToString() {
      return string_builder_.ToString();
    }
  }
}
