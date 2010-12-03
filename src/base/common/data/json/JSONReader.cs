using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Nohros.Resources;

using Nohros.Data.Collections;

namespace Nohros.Data
{
    /// <summary>
    /// A JSON parser. Converts strings of JSON into a
    /// http://www.w3.org/TR/2001/CR-css3-selectors-20011113/
    /// 
    /// Known limitations/derivations from the RFC:
    /// - Only knows how to parse ints within the range of signed 32 bits int and
    ///   decimal numbers within double.
    /// </summary>
    public class JSONReader
    {
        /// <summary>
        /// The input string.
        /// </summary>
        string json_;

        /// <summary>
        /// Thecurrent position in the input string.
        /// </summary>
        int json_pos_;

        /// <summary>
        /// The length of the input string.
        /// </summary>
        int json_length_;

        /// <summary>
        /// A parser flag that allows trailing comma in objects and arrays.
        /// </summary>
        bool allow_trailing_comma_;

        /// <summary>
        /// Used to keep track of how many trailing commas in objects and arrays.
        /// </summary>
        int stack_depth_;

        /// <summary>
        /// A structure to hold a JSON token.
        /// </summary>
        #region Token
        class Token
        {
            #region Token types
            public enum TokenType
            {
                OBJECT_BEGIN,           // {
                OBJECT_END,             // }
                ARRAY_BEGIN,            // [
                ARRAY_END,              // ]
                STRING,
                NUMBER,
                BOOL_TRUE,              // true
                BOOL_FALSE,             // false
                NULL_TOKEN,             // null
                LIST_SEPARATOR,         // ,
                OBJECT_PAIR_SEPARATOR,  // :
                INVALID_TOKEN,
                END_OF_INPUT
            }
            #endregion

            /// <summary>
            /// The token type.
            /// </summary>
            public TokenType type;

            /// <summary>
            /// A index into JSONReader::json_pos_ that's the beginning of this token.
            /// </summary>
            public int begin;

            /// <summary>
            /// End should be one char past the end of the token.
            /// </summary>
            public int length;

            /// <summary>
            /// Initializes a new instance_ of the Token class by using the specified token
            /// type and length.
            /// </summary>
            /// <param name="t">The type of the token</param>
            /// <param name="b">The beginning position of the the token.</param>
            /// <param name="len">The length of the token</param>
            public Token(TokenType t, int b, int len)
            {
                type = t;
                begin = b;
                length = len;
            }
        }
        #endregion

        #region .ctor
        public JSONReader()
        {
        }
        #endregion

        static Token kInvalidToken = new Token(Token.TokenType.INVALID_TOKEN, 0, 0);
        static int kStackLimit = 100;

        /// <summary>
        /// A helper method for ParseNumberToken. It reads an int from the end of the
        /// token. The method returns false if there is no valid integer at the end of
        /// the token.
        /// </summary>
        /// <returns></returns>
        bool ReadInt(Token token, bool can_have_leading_zeros)
        {
            int len = 0;
            int pos = token.begin + token.length;
            char c, first;

            first = json_[pos];

            // Read in more digits
            c = first;
            while (pos < json_length_ && c >= '0' && c <= '9') {
                ++token.length;
                ++len;
                c = json_[++pos];
            }

            // We need at least one digit
            if (len == 0)
                return false;

            if (!can_have_leading_zeros && len > 1 && '0' == first)
                return false;

            return true;
        }

        /// <summary>
        /// Reads and parses <paramref name="json"/>, returning a <see cref="IDictionary&alt;string, string&gt;"/>. 
        /// </summary>
        /// <param name="json">The JSON-compliant string to parse.</param>
        /// <param name="allow_trailing_comma">true to allow trailing commas in objects and arrays; otherwise , false.</param>
        /// <param name="check_root">true to require  the root object to be an object or an array; otherwise, false.</param>
        /// <returns>A IDictionary&lt;string, string&gt; containing the parsed JSON string.</returns>
        /// <exception cref="ArgumentException">The <paramref name="json"/> is not a properly formed JSON string.</exception>
        /// <remarks>
        /// If <paramref name="allow_trailing_comma"/> is true, we will ignore trailing commas in objects and
        /// arrays even through this goes against RFC.
        /// </remarks>
        public Value JsonToValue(string json, bool check_root, bool allow_trailing_comma)
        {
            json_ = json;
            allow_trailing_comma_ = allow_trailing_comma;
            json_pos_ = 0;
            json_length_ = json_.Length;
            stack_depth_ = 0;

            Value root = BuildValue(check_root);
            if (root != null) {
                if (ParseToken().type == Token.TokenType.END_OF_INPUT) {
                    return root;
                }
                else {
                    throw new ArgumentException(string.Format(StringResources.JSON_UnexpectedDataAfterRoot, json_pos_));
                }
            }

            throw new ArgumentException(StringResources.Generic_SyntaxError);
        }

        /// <summary>
        /// Recursively build <see cref="Value"/>.
        /// </summary>
        /// <param name="is_root">true to verify if the root is either an object or an array; otherwise, false.</param>
        /// <returns>An IDictionary&alt;string, string&gt; containing the parsed JSON string or a null reference
        /// if we don't have a valid JSON string.</returns>
        /// <exception cref="ArgumentException">Too much nesting.</exception>
        Value BuildValue(bool is_root)
        {
            ++stack_depth_;
            if (stack_depth_ > kStackLimit)
                throw new ArgumentException(StringResources.JSON_TooMuchNesting);

            Token token = ParseToken();
            // The root token must be an array or an object.
            if (is_root && token.type != Token.TokenType.OBJECT_BEGIN && token.type != Token.TokenType.ARRAY_BEGIN)
                return null;

            Value node;

            switch(token.type) {
                case Token.TokenType.END_OF_INPUT:
                case Token.TokenType.INVALID_TOKEN:
                    return null;

                case Token.TokenType.NULL_TOKEN:
                    node = Value.CreateNullValue();
                    break;

                case Token.TokenType.BOOL_TRUE:
                    node = Value.CreateBooleanValue(true);
                    break;

                case Token.TokenType.BOOL_FALSE:
                    node = Value.CreateBooleanValue(false);
                    break;

                case Token.TokenType.NUMBER:
                    node = DecodeNumber(token);
                    break;

                case Token.TokenType.STRING:
                    node = DecodeString(token);
                    break;

                case Token.TokenType.ARRAY_BEGIN:
                    {
                        json_pos_ += token.length;
                        token = ParseToken();

                        node = new ListValue();
                        while (token.type != Token.TokenType.ARRAY_END) {
                            Value array_node = BuildValue(false);
                            if (array_node == null)
                                return null;
                            ((ListValue)node).Append(array_node);

                            // After a list value, we expect a comma ot the end of the list.
                            token = ParseToken();
                            if (token.type == Token.TokenType.LIST_SEPARATOR) {
                                json_pos_ += token.length;
                                token = ParseToken();

                                // Trailing commas are invalid according to the JSON RFC, but some
                                // consumers need the parsing leniency, so handle accordingly.
                                if (token.type == Token.TokenType.ARRAY_END) {
                                    if (!allow_trailing_comma_) {
                                        throw new ArgumentException(string.Format(StringResources.JSON_TrailingComma, json_pos_));
                                    }
                                    // Trailing comma OK, stop parsing the Array.
                                    break;
                                }
                            } else if (token.type != Token.TokenType.ARRAY_END) {
                                // Unexpected value after list value. Bail out.
                                return null;
                            }
                        }
                        if (token.type != Token.TokenType.ARRAY_END) {
                            return null;
                        }
                        break;
                    }

                case Token.TokenType.OBJECT_BEGIN:
                    {
                        json_pos_ += token.length;
                        token = ParseToken();

                        node = new DictionaryValue();
                        while(token.type != Token.TokenType.OBJECT_END) {
                            if (token.type != Token.TokenType.STRING) {
                                throw new ArgumentException(string.Format(StringResources.JSON_UnquotedDictionaryKey, json_pos_));
                            }
                            Value dict_key_value = DecodeString(token);
                            if (dict_key_value == null)
                                return null;

                            // Converts the key into a string.
                            string dict_key;
                            bool success = dict_key_value.GetAsString(out dict_key);

                            json_pos_ += token.length;
                            token = ParseToken();
                            if (token.type != Token.TokenType.OBJECT_PAIR_SEPARATOR)
                                return null;

                            json_pos_ += token.length;
                            //token = ParseToken();

                            Value dict_value = BuildValue(false);
                            if (dict_value == null)
                                return null;

                            (node as DictionaryValue)[dict_key] = dict_value;

                            // After a key value pair, we expect a comma or the end of the object
                            token = ParseToken();
                            if (token.type == Token.TokenType.LIST_SEPARATOR) {
                                json_pos_ += token.length;
                                token = ParseToken();
                                // Trailing commas are invalid according to the JSON RFC, but some
                                // consumers need the parsing leniency, so handle accordingly.
                                if (token.type == Token.TokenType.OBJECT_END) {
                                    if (!allow_trailing_comma_) {
                                        throw new ArgumentException(string.Format(StringResources.JSON_TrailingComma, json_pos_));
                                    }
                                    // Trailing comma OK, stop parsing the object.
                                    break;
                                }
                            } else if(token.type != Token.TokenType.OBJECT_END) {
                                // Unexpected value after last object value. Bail out.
                                return null;
                            }
                        }
                        if (token.type != Token.TokenType.OBJECT_END)
                            return null;

                        break;
                    }

                default:
                    // We got a token that's not a value.
                    return null;
            }
            json_pos_ += token.length;

            --stack_depth_;

            return node;
        }

        /// <summary>
        /// Parses a sequence of characters into a Token.TokenType.NUMBER. If the sequence of
        /// characters is not a valid number, returns a Token.TokenType.INVALID_TOKEN. Note
        /// that the string will not be converted to a number.
        /// </summary>
        Token ParseNumberToken()
        {
            // We just grab the number here. We validate the size in DecodeNumber.
            // According to RFC4627, a valid number is: [minus] int [frac] [exp]
            Token token = new Token(Token.TokenType.NUMBER, json_pos_, 0);
            //int len = 0;
            int pos = json_pos_;
            char c = json_[pos];

            // Read in more digits
            if ('-' == c) {
                ++token.length;
            }

            if (!ReadInt(token, false))
                return kInvalidToken;

            pos += token.length;
            if (pos >= json_length_)
                return token;

            // Optional fraction part
            c = json_[pos];
            if ('.' == c) {
                ++token.length;
                int len = token.length;

                if (!ReadInt(token, true))
                    return kInvalidToken;

                pos += token.length - len + 1; // one character ahead
                if (pos >= json_length_)
                    return token;

                c = json_[pos];
            }

            // Optional expoent part
            if ('e' == c || 'E' == c) {
                ++token.length;

                // expoent without expoent [E|e]?.
                if (++pos >= json_length_)
                    return kInvalidToken;

                c = json_[pos];
                if ('-' == c || '+' == c) {
                    ++token.length;
                    if (++pos >= json_length_)
                        return kInvalidToken;
                }
                if (!ReadInt(token, true))
                    return kInvalidToken;
            }

            return token;
        }

        /// <summary>
        /// Try and convert a substring that token holds into their an int or a double.
        /// </summary>
        /// <param name="token">A token that holds the substring to be converted.</param>
        /// <returns>A Value object containing the converted number if the substring that token holds was
        /// converted successfully.</returns>
        Value DecodeNumber(Token token)
        {
            string num_string = json_.Substring(token.begin, token.length);

            if (num_string.IndexOf('.') != -1) {
                double num_double;
                if (double.TryParse(num_string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out num_double))
                    return Value.CreateRealValue(num_double);
            } else {
                int num_int;
                if (int.TryParse(num_string, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out num_int))
                    return Value.CreateIntegerValue(num_int);
            }

            return null;
        }

        /// <summary>
        /// Parses a sequence of characters into a <see cref="Token.TokenType.STRING"/>. If the
        /// sequence of characters is not a valid string, returns a <see cref="Token.TokenType::INVALID_TOKEN"/>.
        /// </summary>
        /// <returns>A Token of the type <see cref="Token.TokenType.STRING"/> containing the parsed
        /// seuence of characters.</returns>
        Token ParseStringToken()
        {
            Token token = new Token(Token.TokenType.STRING, json_pos_, 1);
            int pos = json_pos_ + 1;
            char c = json_[pos];

            while (pos < json_length_)
            {
                if (c == '\\')
                {
                    ++token.length;
                    c = json_[++pos];

                    // Make sure the escaped char is valid.
                    switch (c)
                    {
                        case '\\':
                        case '/':
                        case 'b':
                        case 'f':
                        case 'n':
                        case 'r':
                        case 't':
                        case 'v':
                        case '"':
                            break;
                        default:
                            return kInvalidToken;
                    }
                }
                else if (c == '"')
                {
                    ++token.length;
                    return token;
                }
                ++token.length;
                c = json_[++pos];
            }
            return kInvalidToken;
        }

        /// <summary>
        /// Converts the substring that token holds into a <see cref="Value"/> string.
        /// This should always succeed( otherwise ParseStringToken() would have failed ).
        /// </summary>
        /// <param name="token">The token that holds the substring to convert.</param>
        /// <returns>The converted <see cref="Value"/> string.</returns>
        Value DecodeString(Token token)
        {
            return Value.CreateStringValue(json_.Substring(token.begin + 1, token.length - 2));
        }


        /// <summary>
        /// Grabs the next token in the jsin stream. This does not increment the
        /// stream so it can be used to look ahead at the next token.
        /// </summary>
        /// <returns>A token class containing information about the next token in
        /// the json stream</returns>
        Token ParseToken()
        {
            const string kNullString = "null";
            const string kTrueString = "true";
            const string kFalseString = "false";

            EatWhitespacesAndComments();

            Token token = new Token(Token.TokenType.INVALID_TOKEN, 0, 0);
            if (json_pos_ >= json_length_) {
                token.type = Token.TokenType.END_OF_INPUT;
                return token;
            }

            switch (json_[json_pos_])
            {
                case 'n':
                    if (NextStringMatch(kNullString))
                        token = new Token(Token.TokenType.NULL_TOKEN, json_pos_, 4);
                    break;

                case 't':
                    if (NextStringMatch(kTrueString))
                        token = new Token(Token.TokenType.BOOL_TRUE, json_pos_, 4);
                    break;

                case 'f':
                    if (NextStringMatch(kFalseString))
                        token = new Token(Token.TokenType.BOOL_FALSE, json_pos_, 1);
                    break;

                case '[':
                    token = new Token(Token.TokenType.ARRAY_BEGIN, json_pos_, 1);
                    break;

                case ']':
                    token = new Token(Token.TokenType.ARRAY_END, json_pos_, 1);
                    break;

                case ',':
                    token = new Token(Token.TokenType.LIST_SEPARATOR, json_pos_, 1);
                    break;

                case '{':
                    token = new Token(Token.TokenType.OBJECT_BEGIN, json_pos_, 1);
                    break;

                case '}':
                    token = new Token(Token.TokenType.OBJECT_END, json_pos_, 1);
                    break;

                case ':':
                    token = new Token(Token.TokenType.OBJECT_PAIR_SEPARATOR, json_pos_, 1);
                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                    token = ParseNumberToken();
                    break;

                case '"':
                    token = ParseStringToken();
                    break;
            }
            return token;
        }

        /// <summary>
        /// Checks if a given string matches the string beginning at the |json_pos_| position.
        /// </summary>
        /// <param name="str">The string to match</param>
        /// <returns>true if the given string matches the string beginning at the |json_pos_|
        /// position; otherwise false.</returns>
        bool NextStringMatch(string str)
        {
            for (int i = 0, j = str.Length; i < j; ++i) {
                if (json_[json_pos_ + i] != str[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Increments |json_pos_| past leading whitespaces and comments.
        /// </summary>
        void EatWhitespacesAndComments()
        {
            while (json_pos_ < json_length_)
            {
                switch (json_[json_pos_])
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                        ++json_pos_;
                        break;
                    case '/':
                        if (!EatComment())
                            return;
                        break;
                    default:
                        // Not a whitespace char, just exit.
                        return;
                }
            }
        }

        /// <summary>
        /// Eat comments.
        /// </summary>
        /// <returns>true if |json_pos_| is at the start of a commet; otherwise false</returns>
        bool EatComment()
        {
            if ('/' != json_[json_pos_])
                return false;

            char next_char = json_[json_pos_ + 1];
            if (next_char == '/')
            {
                // Line comment read until \n or \r
                json_pos_ += 2;
                while (json_pos_ < json_length_)
                {
                    switch (json_[json_pos_])
                    {
                        case '\n':
                        case '\r':
                            ++json_pos_;
                            return true;
                        default:
                            ++json_pos_;
                            break;
                    }
                }
            }
            else if (next_char == '*')
            {
                // Block comment, read until */
                json_pos_ += 2;
                while (json_pos_ < json_length_)
                {
                    if (json_[json_pos_] == '*' && json_[json_pos_ + 1] == '/')
                    {
                        json_pos_ += 2;
                        return true;
                    }
                    ++json_pos_;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
