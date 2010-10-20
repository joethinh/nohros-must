using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// Represents a string that has embedded parameters.
    /// </summary>
    public class ParameterizedString
    {
        string flat_string_;
        string parameter_delimiter_;

        static object mutex_;

        Dictionary<string, ParameterizedStringPart> parts_;

        #region .ctor
        /// <summary>
        /// Static initializer.
        /// </summary>
        static ParameterizedString() {
            mutex_ = new object();
        }

        /// <summary>
        /// Initializes a new instance of the ParameterizedString class.
        /// </summary>
        public ParameterizedString():this(string.Empty, string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the ParameterizedString class by using the given
        /// </summary>
        /// <param name="str">A string that can be made into a parameterized string.</param>
        /// <param name="delimiter">A string that delimits a parameter parameters in the
        /// parameterized string.</param>
        public ParameterizedString(string str, string delimiter) {
            if (str == null || delimiter == null)
                throw new ArgumentNullException((str == null) ? "str" : "delimiter");

            flat_string_ = str;
            parameter_delimiter_ = delimiter;
            parts_ = new Dictionary<string, ParameterizedStringPart>();

            Parse(str, delimiter);
        }

        /// <summary>
        /// Initializes a new instance of the ParameterizedString class by using the provided list
        /// of parameter parts.
        /// </summary>
        /// <param name="parts">A list of <see cref="ParameterizedStringPart"/>object.</param>
        /// <param name="delimiter">A string that delimits a parameter parameters in the
        /// parameterized string.</param>
        public ParameterizedString(IEnumerable<ParameterizedStringPart> parts, string delimiter) {
            StringBuilder builder = new StringBuilder();
            foreach (ParameterizedStringPart part in parts) {
                parts_.Add(part.ParameterName, part);
                if (part.IsParameter) {
                    builder.Append(delimiter).Append(part.LiteralValue).Append(delimiter);
                    continue;
                }
                builder.Append(part.LiteralValue);
            }
            flat_string_ = builder.ToString();
        }
        #endregion


        void Parse(string str, string delimiter) {
            //TODO: implements this
            /*for (int i = 0, j = str.Length; i < j; i++) {
                c = str[i];
                if(c == delimiter_)
                {
                    if(b)
                    {
                        string parmname = str.Substring(begin, i - begin);
                        _base = _base.Remove(begin - 1 - removed, parmname.Length + 2);

                        Parameter parm = new Parameter();
                        parm.Position = begin - 1- removed;
                        parm.Value = string.Empty;
                        
                        // store the paramter into the paramters list
                        _parms[parmname] = parm;

                        removed += parmname.Length + 2;

                        b = false;
                    }
                    else
                    {
                        b = true;
                        begin = i+1;
                    }
                }
                else
                {
                    for(int k=0, l=_breaks.Length; k < l; k++)
                    {
                        if (_breaks[k] == c)
                            b = false;
                    }
                }
            }*/
        }

        public string this[string key] {
            // TODO: implement this.
            get { return null; }
        }

        /// <summary>
        /// Serializes this instance into a string object
        /// </summary>
        /// <returns>A string representation of this instance_</returns>
        public override string ToString()
        {
            //TODO: implements
            return null;
        }
    }
}
