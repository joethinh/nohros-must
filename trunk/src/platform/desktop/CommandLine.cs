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
        // Since we use a lazy match, make sure that longer versions (like "--")
        // are listed before shorter versions (like "-") of similar prefixes.
        const char kSwitchValueSeparator = '=';

        static CommandLine current_process_commandline_;

        string command_line_string_;
        Dictionary<string ,string> switches_;
        List<string> loose_values_;

        #region .ctor
        /// <summary>
        /// Singleton constructor.
        /// </summary>
        static CommandLine() {
            current_process_commandline_ = new CommandLine();
            current_process_commandline_.ParseFromString(current_process_commandline_.command_line_string_);
        }

        /// <summary>
        /// Constructs a new, empty command line.
        /// </summary>
        internal CommandLine() {
            switches_ = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            loose_values_ = new List<string>();
            command_line_string_ = Environment.CommandLine;
        }
        #endregion

        /// <summary>
        /// Creates a parsed version of the given command-line string.
        /// </summary>
        /// <param name="command_line">The command line to parse.</param>
        void ParseFromString(string command_line) {
            if (command_line == null)
                return;

            command_line_string_ = command_line.Trim();
            if (command_line_string_ == string.Empty)
                return;

            int j = command_line_string_.Length, i = 0;

            while(i < j) {
                switch(command_line_string_[i]) {
                    case '-':
                        if (command_line_string_[++i] == '-')
                        {
                            if (command_line_string_[++i] == '-')
                                continue;
                        }
                        i += ParseSwitch(i);
                        break;

                    case '/':
                        i += ParseSwitch(++i);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses a switch that begins at <paramref name="begin"/> position within the current command line string.
        /// </summary>
        /// <param name="begin">The positon wihtin the current command line string where the switch begins.</param>
        /// <returns>The number of characters advanced from <paramref name="begin"/> while parsing the swicth.</returns>
        int ParseSwitch(int begin) {
            string switch_string, switch_value;
            int equals_positon, end_positon;

            equals_positon = command_line_string_.IndexOf('=', begin);
            if (equals_positon == -1) {
                end_positon = command_line_string_.IndexOf(' ', begin);
                if (end_positon == -1)
                    end_positon = command_line_string_.Length - begin;

                switch_string = command_line_string_.Substring(begin, end_positon);
                switch_value = string.Empty;
            } else {
                end_positon = command_line_string_.IndexOf(' ', equals_positon);
                if (end_positon == -1)
                    end_positon = command_line_string_.Length - equals_positon;

                switch_string = command_line_string_.Substring(begin, equals_positon - begin);
                switch_value = command_line_string_.Substring(equals_positon, end_positon - equals_positon);
            }
            switches_[switch_string] = switch_value;

            return (end_positon - begin);
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
        /// Gets the singleton CommandLine representing the current process's command line.
        /// </summary>
        static CommandLine ForCurrentProcess {
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
        IList<string> LooseValues {
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
