using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Desktop
{
    /// <summary>
    /// This class works with command lines: building and parsing.
    /// Switches can optionally have a value attached using an equals sign,
    /// as in "-switch=value, /switch=value or --switch=value". Arguments that aren't prefixed with a
    /// switch prefix are considered "loose parameters". Switch names are
    /// case-insensitive.
    /// <para>
    /// There is a singleton read-only CommandLine that represents the command
    /// line that the current process was started with.
    /// </para>
    /// </summary>
    public class CommandLine
    {
        #region TokenType
        class Token {
            public enum TokenType
            {
                PROGRAM,
                SPACE,
                SWITCH_VALUE_PAIR_SEPARATOR,
                SWITCH_BEGIN,
                STRING,
                END_OF_INPUT
            }

            /// <summary>
            /// A index into CommandLine.command_line_string_ that's the beginning of this token.
            /// </summary>
            public int begin;

            /// <summary>
            /// The length of this token.
            /// </summary>
            public int length;

            /// <summary>
            /// The token type.
            /// </summary>
            public TokenType type;

            /// <summary>
            /// Initializes a new TOken by using the specified token begin, type and end.
            /// </summary>
            /// <param name="begin">The begin of the token within the current command line</param>
            /// <param name="end">The end of the token within the current command line.</param>
            /// <param name="type">The type of teh token.</param>
            public Token(TokenType t, int b, int len) {
                type = t;
                begin = b;
                length = len;
            }
        }
        #endregion

        static CommandLine current_process_commandline_;

        string command_line_string_, program_;
        Dictionary<string ,string> switches_;
        List<string> loose_values_;

        int command_line_pos_, command_line_length_;

        #region .ctor
        /// <summary>
        /// Singleton constructor.
        /// </summary>
        static CommandLine() {
            current_process_commandline_ = new CommandLine();
            current_process_commandline_.ParseFromString(current_process_commandline_.command_line_string_);
        }

        /// <summary>
        /// Constructs a new, empty command line and initializes it. Used internally.
        /// </summary>
        CommandLine() {
            Reset();
            command_line_string_ = Environment.CommandLine;
        }

        /// <summary>
        /// Constructs a new empty CommandLine object.
        /// <param name="program">The name of the program to run.(aka argv[0])</param>
        /// </summary>
        public CommandLine(string program) {
            Reset();
            program_ = program;
            command_line_string_ = QuoteIfNeed(program_);
        }
        #endregion

        /// <summary>
        /// Encloses a string in quotes if has spaces within it.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string QuoteIfNeed(string value) {
            if ((value != null) &&
                (value.Trim().Length != 0) &&
                (value.IndexOf(' ') != -1) &&
                (value[0] != '"') &&
                (value[value.Length - 1] != '"')) {
                value = string.Concat('"', value, '"');
            }
            return value;
        }


        /// <summary>
        /// Gets a valid switch prefix based on the specified switch.
        /// </summary>
        /// <param name="switch_prefix">An switch to validate.</param>
        /// <returns>The specified switch prefix or "-" if the specified switch prefix is not valid.</returns>
        /// <remarks>
        /// Valid switch prefixes are: "-", "--", "/".
        /// </remarks>
        string GetSwitchPrefix(string switch_prefix) {
            switch(switch_prefix) {
                case "--":
                case "-":
                case "/":
                    return switch_prefix;
            }
            return "-";
        }

        /// <summary>
        /// Appends the given switch string (preceded by a space and a switch prefix)
        /// to the given string.
        /// </summary>
        /// <param name="switch_string">The switch string to append.</param>
        /// <param name="switch_prefix">The switch prefix to append.</param>
        public void AppendSwitch(string switch_string, string switch_prefix) {
            if (switch_string == null || switch_string.Trim().Length == 0)
                return;

            switch_prefix = GetSwitchPrefix(switch_prefix);

            command_line_string_ += string.Concat(' ', switch_prefix, switch_string);
            switches_[switch_string] = string.Empty;
        }

        /// <summary>
        /// Appends the given switch string(preceded by a space and a switch prefix)
        /// to the given string, with the given value attached.
        /// </summary>
        public void AppendSwitchWithValue(string switch_string, string switch_prefix, string switch_value, char switch_value_separator) {
            if (switch_string == null || switch_string.Trim().Length == 0)
                return;

            command_line_string_ += string.Concat(' ',
                GetSwitchPrefix(switch_prefix),
                switch_string,
                switch_value_separator,
                QuoteIfNeed(switch_value));

            switches_[switch_string] = switch_value;
        }

        /// <summary>
        /// Append a loose value to the command line.
        /// </summary>
        /// <param name="value"></param>
        public void AppendLooseValue(string value) {
            command_line_string_ += string.Concat(' ', QuoteIfNeed(value));
            loose_values_.Add(value);
        }

        /// <summary>
        /// Resets the command line.
        /// </summary>
        public void Reset() {
            command_line_string_ = string.Empty;
            program_ = string.Empty;
            command_line_pos_ = 0;
            command_line_length_ = 0;
            switches_ = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            loose_values_ = new List<string>();
        }

        /// <summary>
        /// Creates a parsed version of the given command-line string. The program name is
        /// assumed to be the first item in the string.
        /// </summary>
        /// <param name="command_line">The command line to parse.</param>
        public void ParseFromString(string command_line) {
            if (command_line == null)
                return;

            command_line_string_ = command_line.Trim();
            if (command_line_string_ == string.Empty)
                return;

            int j = command_line_string_.Length, i;

            // populate program_ with the trimmed version of the first arg.
            i = command_line_string_.IndexOf((command_line_string_[0] == '"') ? '"' : ' ', 1);
            if (i == -1) {
                program_ = command_line_string_;
                return; // the command line has no arguments/switches(no spaces).
            }

            program_ = command_line_string_.Substring(0, i);

            command_line_pos_ = ++i;
            command_line_length_ = command_line_string_.Length;

            // tokenizing the command line
            List<Token> tokens = new List<Token>();

            // removing the white spaces after the program token
            while (command_line_pos_ < command_line_length_)
                if (command_line_string_[command_line_pos_++] != ' ') {
                    --command_line_pos_;
                    break;
                }

            // no arguments was supplied, nothing to do.
            if (command_line_pos_ == command_line_length_)
                return;

            Token token;
            do {
                token = ParseToken();
                tokens.Add(token);
                command_line_pos_ += token.length;
            } while (token.type != Token.TokenType.END_OF_INPUT);

            // process the command line tokens
            string switch_string = null, switch_value = null;
            int begin = 0;

            i = 0;
            j = tokens.Count;
            while (i < j) {
                token = tokens[i];
                switch (token.type) {
                    case Token.TokenType.SWITCH_BEGIN:
                        begin = token.begin + 1;
                        while (token.type != Token.TokenType.END_OF_INPUT) {
                            token = tokens[++i];
                            if (token.type == Token.TokenType.STRING || token.type == Token.TokenType.END_OF_INPUT) {
                                switch_string = command_line_string_.Substring(begin, token.begin - begin + token.length);
                                break;
                            } else if (token.type == Token.TokenType.SWITCH_BEGIN) {
                                begin = token.begin + 1;
                            }
                        }
                        break;

                    case Token.TokenType.SWITCH_VALUE_PAIR_SEPARATOR:
                        begin = token.begin + 1;
                        while (token.type != Token.TokenType.END_OF_INPUT) {
                            token = tokens[++i];
                            if (token.type == Token.TokenType.SPACE || token.type == Token.TokenType.END_OF_INPUT) {
                                switch_value = command_line_string_.Substring(begin, token.begin - begin);
                                if (switch_string == null)
                                    loose_values_.Add(switch_value);
                                else {
                                    switches_[switch_string] = switch_value;
                                    switch_string = null;
                                }
                                break;
                            }
                        }
                        break;

                    case Token.TokenType.STRING:
                        begin = token.begin;
                        while (token.type != Token.TokenType.END_OF_INPUT) {
                            token = tokens[++i];
                            if (token.type == Token.TokenType.SPACE || token.type == Token.TokenType.END_OF_INPUT) {
                                loose_values_.Add(command_line_string_.Substring(begin, token.begin - begin));
                                break;
                            }
                        }
                        break;

                    case Token.TokenType.SPACE:
                    case Token.TokenType.END_OF_INPUT:
                        if (switch_string != null)
                            switches_[switch_string] = string.Empty;
                        break;
                }
                ++i;
            }
        }

        /// <summary>
        /// Parses a sequence of characters into a <see cref="Token.TokenType.STRING"/>.
        /// </summary>
        /// <remarks>A string token parsing ends when a space character is found.</remarks>
        /// <returns>A Token of the type <see cref="Token.TokenType.STRING"/> containing the parsed
        /// sequence of characters.</returns>
        Token ParseStringToken() {
            Token token = new Token(Token.TokenType.STRING, command_line_pos_, 1);
            int pos = command_line_pos_;
            char c = command_line_string_[pos];

            if(c == '"') {
                while (++pos < command_line_length_) {
                    ++token.length;
                    c = command_line_string_[pos];
                    if( c == '"') break;
                }
            } else {
                while (++pos < command_line_length_) {
                    c = command_line_string_[pos];
                    switch (c) {
                        case ':':
                        case '=':
                        case '-':
                        case '/':
                        case ' ':
                            return token;
                        default:
                            ++token.length;
                            break;
                    }
                }
            }
            return token;
        }

        /// <summary>
        /// Grabs the next token in the command line stream. This does not decrements the
        /// stream so it can be used to look ahead at the next token.
        /// </summary>
        /// <returns>A token class containing information about the next token in
        /// the command line stream</returns>
        Token ParseToken() {
            Token token = new Token(Token.TokenType.STRING, 0, 0);

            if (command_line_pos_ >= command_line_length_)
                return new Token(Token.TokenType.END_OF_INPUT, command_line_length_, 0);

            switch (command_line_string_[command_line_pos_]) {
                case '=':
                case ':':
                    token = new Token(Token.TokenType.SWITCH_VALUE_PAIR_SEPARATOR, command_line_pos_, 1);
                    break;

                case '/':
                    token = new Token(Token.TokenType.SWITCH_BEGIN, command_line_pos_, 1);
                    break;

                case '-':
                    token = new Token(Token.TokenType.SWITCH_BEGIN, command_line_pos_, 1);
                    break;

                case ' ':
                    token = new Token(Token.TokenType.SPACE, command_line_pos_, 1);
                    break;

                case '"':
                    token = ParseStringToken();
                    break;

                default:
                    token = ParseStringToken();
                    break;
            }
            return token;
        }

        /// <summary>
        /// Gets a value indicating if this command line has the specified switch.
        /// </summary>
        /// <param name="switch_string">The switch to verify.</param>
        /// <returns>true if this command line contains the given switch.</returns>
        /// <remarks>Switch names are case-insensitive.</remarks>
        public bool HasSwitch(string switch_string) {
            string switch_value;
            return switches_.TryGetValue(switch_string, out switch_value);
        }

        /// <summary>
        /// Gets the value associated with the specified switch.
        /// </summary>
        /// <param name="switch_string">The switch to get the value from.</param>
        /// <returns>The value associated with the specified switch or an empty string if
        /// the switch has no value or isn't present.</returns>
        public string GetSwitchValue(string switch_string) {
            string switch_value;
            if (switches_.TryGetValue(switch_string, out switch_value)) {
                return switch_value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the program part of the command line string (the first item).
        /// </summary>
        public string Program {
            get { return program_; }
        }

        /// <summary>
        /// Gets the singleton CommandLine representing the current process's command line.
        /// </summary>
        public static CommandLine ForCurrentProcess {
            get { return current_process_commandline_; }
        }

        /// <summary>
        /// Gets the number of switches in this process.
        /// </summary>
        public int SwitchCount {
            get { return switches_.Count; }
        }

        /// <summary>
        /// Gets the "loose parameters" that is the command line arguments that aren't
        /// prefixed with a switch prefix.
        /// </summary>
        /// <returns>An IList&lt;string&gt; containing all the "loose parameters".</returns>
        public IList<string> LooseValues {
            get { return loose_values_; }
        }

        /// <summary>
        /// Gets the original command line string.
        /// </summary>
        public string CommandLineString {
            get { return command_line_string_; }
        }
    }
}
