using System;
using System.Collections.Generic;
using System.Globalization;
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

    #region Token
    /// <summary>
    /// Wraps a token and its type into a single object.
    /// </summary>
    struct Token
    {
      readonly TokenType type_;
      readonly string value_;

      #region .ctor
      /// <summary>
      /// Initializes a new instance of the <see cref="Token"/> class by using
      /// the token value and type.
      /// </summary>
      /// <param name="value">
      /// The token's values.
      /// </param>
      /// <param name="type">
      /// The type of the token.
      /// </param>
      public Token(string value, TokenType type) {
        value_ = value;
        type_ = type;
      }
      #endregion

      /// <summary>
      /// Gets the  token's value.
      /// </summary>
      public string Value {
        get { return value_; }
      }

      /// <summary>
      /// Gets the type of the token.
      /// </summary>
      public TokenType Type {
        get { return type_; }
      }
    }
    #endregion

    #region TokenType
    /// <summary>
    /// Provides a way to identify if a token is a structural token or a
    /// token whose value was specified by the user.
    /// </summary>
    enum TokenType
    {
      Structural = 0,
      Value = 1
    }
    #endregion

    const string kBeginObject = "{";
    const string kEndObject = "}";
    const string kBeginArray = "[";
    const string kEndArray = "]";
    const string kNameValueSeparator = ":";
    const string kValueSeparator = ",";
    const string kDefaultNumberFormat = "G";
    const string kDoubleQuote = "\"";

    const char kLargestEscapableChar = '\\';

    static readonly char[] escape_chars_ = new char[]
    {'"', '\n', '\t', '\\', '\f', '\b'};

    readonly List<Token> tokens_;
    State current_state_;
    int last_written_token_position_;
    string memoized_json_string_;

    // Json numbers uses a format that is culture invariant and is equals to
    // the numeric format used by US culture.
    CultureInfo numeric_format_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonStringBuilder"/> class.
    /// </summary>
    public JsonStringBuilder() {
      tokens_ = new List<Token>();
      current_state_ = State.None;
      last_written_token_position_ = 0;

      // Json numbers uses a format that is culture invariant and is equals to
      // the numeric format used by US culture.
      numeric_format_ = new CultureInfo("en-US");
    }
    #endregion

    /// <summary>
    /// Appends the begin array token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteBeginArray() {
      ++last_written_token_position_;
      return
        WriteReservedBeginToken(new Token(kBeginArray, TokenType.Structural));
    }

    /// <summary>
    /// Appends the end array token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteEndArray() {
      ++last_written_token_position_;
      return WriteReservedEndToken(new Token(kEndArray, TokenType.Structural));
    }

    /// <summary>
    /// Appends the begin object token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteBeginObject() {
      ++last_written_token_position_;
      return
        WriteReservedBeginToken(new Token(kBeginObject, TokenType.Structural));
    }

    /// <summary>
    /// Appends the end object token to the current json string.
    /// </summary>
    public JsonStringBuilder WriteEndObject() {
      ++last_written_token_position_;
      return WriteReservedEndToken(new Token(kEndObject, TokenType.Structural));
    }

    /// <summary>
    /// Appends the string <paramref name="value"/> to the current json string.
    /// </summary>
    /// <remarks>
    /// This method encloses the string in a double quotes.
    /// <paramref name="value"/>.
    /// </remarks>
    public JsonStringBuilder WriteString(string value) {
      ++last_written_token_position_;
      return
        WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token((value ?? "null"), TokenType.Value))
          .WriteContentToken(new Token(kDoubleQuote, TokenType.Structural));
    }

    /// <summary>
    /// Appends the string <paramref name="value"/> to the current json string.
    /// </summary>
    /// <remarks>
    /// This method write the specified string as is, which means that it do
    /// not encloses the string in a double quotes.
    /// </remarks>
    public JsonStringBuilder WriteUnquotedString(string value) {
      ++last_written_token_position_;
      return WriteContentToken(new Token(value ?? "null", TokenType.Value));
    }

    public JsonStringBuilder WriteStringArray(string[] data) {
      WriteBeginArray();
      for (int i = 0, j = data.Length; i < j; i++) {
        WriteString(data[i]);
      }
      // rewind the pointer to the begining of the JSON array
      last_written_token_position_ -= data.Length;

      // Write end array advances the pointer, which causes the pointer to
      // points to the first token after the begin array token. This is ok,
      // since the begin array token should not be escaped.
      return WriteEndArray();
    }

    public JsonStringBuilder WriteTokenArray(IJsonToken[] tokens) {
      WriteBeginArray();
      for (int i = 0, j = tokens.Length; i < j; i++) {
        WriteUnquotedString(tokens[i].AsJson());
      }
      // rewind the pointer to the begining of the JSON array.
      last_written_token_position_ -= tokens.Length;

      // Write end array advances the pointer, which causes the pointer to
      // points to the first token after the begin array token. This is ok,
      // since the begin array token should not be escaped.
      return WriteEndArray();
    }

    public JsonStringBuilder WriteTokenArray(IEnumerable<IJsonToken> tokens) {
      WriteBeginArray();
      foreach (IJsonToken token in tokens) {
        // Since we do not now the size of the tokens collections we need to
        // rewind the pointer at each iteration.
        --last_written_token_position_;
        WriteUnquotedString(token.AsJson());
      }

      // Write end array advances the pointer, which causes the pointer to
      // points to the first token after the begin array token. This is ok,
      // since the begin array token should not be escaped.
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
      ++last_written_token_position_;
      return
        WriteContentToken(new Token(value.ToString(format, numeric_format_),
          TokenType.Value));
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
      ++last_written_token_position_;
      return
        WriteContentToken(new Token(value.ToString(format, numeric_format_),
          TokenType.Value));
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
      ++last_written_token_position_;
      return
        WriteContentToken(new Token(value.ToString(format, numeric_format_),
          TokenType.Value));
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
      ++last_written_token_position_;
      return
        WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(name, TokenType.Value))
          .WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(kNameValueSeparator,
            TokenType.Structural))
          .WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(value, TokenType.Value))
          .WriteContentToken(new Token(kDoubleQuote, TokenType.Structural));
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
      ++last_written_token_position_;
      return
        WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(name, TokenType.Value))
          .WriteContentToken(new Token(kNameValueSeparator, TokenType.Structural));
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
      ++last_written_token_position_;
      return
        WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(name, TokenType.Value))
          .WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(kNameValueSeparator,
            TokenType.Structural))
          .WriteContentToken(new Token(value.ToString(format, numeric_format_),
            TokenType.Value));
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
      ++last_written_token_position_;
      return
        WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(name, TokenType.Value))
          .WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(kNameValueSeparator,
            TokenType.Structural))
          .WriteContentToken(new Token(value.ToString(format, numeric_format_),
            TokenType.Value));
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
      ++last_written_token_position_;
      return
        WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(name, TokenType.Value))
          .WriteReservedBeginToken(new Token(kDoubleQuote, TokenType.Structural))
          .WriteReservedBeginToken(new Token(kNameValueSeparator,
            TokenType.Structural))
          .WriteContentToken(new Token(value.ToString(format, numeric_format_),
            TokenType.Value));
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
          switch (c) {
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
      for (int i = last_written_token_position_ - 1, j = tokens_.Count;
           i < j;
           i++) {
        Token token = tokens_[i];

        // Structural tokens does should not be escaped.
        if (token.Type == TokenType.Structural) {
          continue;
        }

        string new_token = string.Empty;
        int last_replace_position = 0;
        for (int m = 0, n = token.Value.Length; m < n; m++) {
          char c = token.Value[m];
          if (c < kLargestEscapableChar) {
            string escape_string;
            switch (c) {
              case '\n':
                escape_string = "\\n";
                break;

              case '\r':
                escape_string = "\\r";
                break;

              case '\t':
                escape_string = "\\t";
                break;

              case '"':
                escape_string = "\\\"";
                break;

              case '\\':
                escape_string = "\\\\";
                break;

              case '\f':
                escape_string = "\\f";
                break;

              case '\b':
                escape_string = "\\b";
                break;

              default:
                continue;
            }

            // If we are here, we found some character that needs to be escaped.
            new_token +=
              string.Concat(
                token.Value.Substring(last_replace_position,
                  m - last_replace_position), escape_string);

            // set the last replace pointer to the location where the
            // replacement was performed plus 1 escaped character.
            last_replace_position += (m - last_replace_position + 1);
          }
        }

        // Replace the old token with the new token if the old one was
        // modified.
        if (last_replace_position != 0) {
          tokens_[i] = new Token(new_token, token.Type);
        }
      }
      return this;
    }

    JsonStringBuilder WriteReservedBeginToken(Token token) {
      switch (current_state_) {
        case State.None:
        case State.ReservedBeginToken:
        case State.ReservedEndToken:
          tokens_.Add(token);
          break;
        case State.ContentToken:
          tokens_.Add(new Token(kValueSeparator, TokenType.Structural));
          tokens_.Add(token);
          break;
      }
      current_state_ = State.ReservedBeginToken;
      return this;
    }

    JsonStringBuilder WriteReservedEndToken(Token token) {
      tokens_.Add(token);
      current_state_ = State.ReservedEndToken;
      return this;
    }

    JsonStringBuilder WriteContentToken(Token token) {
      switch (current_state_) {
        case State.None:
        case State.ReservedBeginToken:
          tokens_.Add(token);
          break;
        case State.ReservedEndToken:
        case State.ContentToken:
          tokens_.Add(new Token(kValueSeparator, TokenType.Structural));
          tokens_.Add(token);
          break;
      }
      current_state_ = State.ContentToken;
      return this;
    }

    /// <summary>
    /// Gets the string representing the current json string contained in
    /// this builder.
    /// </summary>
    public override string ToString() {
      if (memoized_json_string_ == null) {
        StringBuilder builder = new StringBuilder();
        for (int i = 0, j = tokens_.Count; i < j; i++) {
          builder.Append(tokens_[i].Value);
        }
        memoized_json_string_ = builder.ToString();
      }
      return memoized_json_string_;
    }
  }
}
