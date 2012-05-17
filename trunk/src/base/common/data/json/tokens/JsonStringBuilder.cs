using System;
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
      AfterBegin = 1,
      AfterEnd = 2
    }
    #endregion

    const string kBeginObject = "{";
    const string kEndObject = "}";
    const string kBeginArray = "[";
    const string kEndArray = "]";
    const string kNameSeparator = ":";
    const string kValueSeparator = ",";
    const string kDefaultNumberFormat = "G";

    State current_state_;
    StringBuilder string_builder_ = new StringBuilder();

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
      return WriteBeginToken(kBeginArray);
    }

    /// <summary>
    /// Appends the end array token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteEndArray() {
      return WriteEndToken(kEndArray);
    }

    /// <summary>
    /// Appends the begin object token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteBeginObject() {
      return WriteBeginToken(kBeginObject);
    }

    /// <summary>
    /// Appends the end object token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteEndObject() {
      return WriteEndToken(kEndObject);
    }

    /// <summary>
    /// Appends the string <paramref name="value"/> to the current json string.
    /// </summary>
    /// <remarks>
    /// This method encloses the string in a double quotes.
    /// <paramref name="value"/>.
    /// </remarks>
    public JsonStringBuilder WriteString(string value) {
      return WriteEndToken("\"" + (value ?? "null") + "\"");
    }

    /// <summary>
    /// Appends the string <paramref name="value"/> to the current json string.
    /// </summary>
    /// <remarks>
    /// This method write the specified string as is, which means that it do
    /// not encloses the string in a double quotes.
    /// </remarks>
    public JsonStringBuilder WriteUnquotedString(string value) {
      return WriteEndToken(value ?? "null");
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
      return WriteEndToken(value.ToString(format));
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
      return WriteEndToken(value.ToString(format));
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
      return WriteEndToken(value.ToString(format));
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
      return WriteEndToken(
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
      return WriteBeginToken(
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
      return WriteEndToken("\"" + name + "\":" + value.ToString(format));
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
      return WriteEndToken("\"" + name + "\":" + value.ToString(format));
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
      return WriteEndToken("\"" + name + "\":" + value.ToString(format));
    }

    JsonStringBuilder WriteBeginToken(string token) {
      WriteToken(token);
      current_state_ = State.AfterBegin;
      return this;
    }

    JsonStringBuilder WriteEndToken(string token) {
      WriteToken(token);
      current_state_ = State.AfterEnd;
      return this;
    }

    void WriteToken(string token) {
      switch (current_state_) {
        case State.AfterBegin:
          string_builder_.Append(token);
          break;
        case State.AfterEnd:
          string_builder_.Append("," + token);
          break;
      }
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
