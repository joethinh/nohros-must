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
        char parameter_delimiter_char_;
        char[] parameter_parsing_stop_chars_;

        Dictionary<string, ParameterizedStringPart> parts_;

        #region .ctor
        /// <summary>
        ///  Used in deep copy operation
        /// </summary>
        /// <param name="capacity">
        /// The approximate number of elements that hte ParameterizedString object can initialy contain.
        /// </param>
        /// <param name="delimiter">
        ///  The parameters delimiter [(delimiter)paramname(delmiter)],
        ///  must be a printable ASCII character.
        /// </param>
        /// <param name="breaks">
        ///  Characters that invalidate a parameter parse sequence
        ///  [(delimiter)some text(break char)some text].
        /// </param>
        ///</param>
        ParameterizedString(int capacity, string str, char delimiter)
        {
            _parms = new Dictionary<string, Parameter>(capacity);
            _base = str;
            delimiter_ = ((byte)delimiter < 32 || (byte)delimiter > 126) ? '$' : delimiter;
        }
        public ParameterizedString(string str, char delimiter, char[] breaks): this(4, str, delimiter)
        {
            _breaks = (breaks == null) ? breaks : new char[] { ' ' };
            Parameterize();
        }
        /// <summary>
        /// Initializes a new instance of the ParameterizedString class by using
        /// the specified base string.
        /// </summary>
        /// <param name="str">The base string</param>
        public ParameterizedString(string str): this(str, (char)0, null)
        {
        }
        /// <summary>
        /// Initializes a new instance_ of the ParameterizedString class by using
        /// the specified base string, parameter delimiter, break characters and initial parameters
        /// </summary>
        /// <param name="str">The base string</param>
        /// <param name="parms">The initial parameters</param>
        /// <param name="delimiter">
        ///  The parameters delimiter [(delimiter)paramname(delmiter)],
        ///  must be a printable ASCII character.
        /// </param>
        /// <param name="breaks">
        ///  Characters that invalidate a parameter parse sequence
        ///  [(delimiter)some text(break char)some text].
        /// </param>
        public ParameterizedString(string str, string parms, char delimiter, char[] breaks): this(str, delimiter, breaks)
        {
            // If no parameter was supplied, nothing to do.
            if (parms == null)
                return;

            string[] m_parms = parms.Split('&');
            for (int i = 0, j = m_parms.Length; i < j; i++) {
                string keyValuePair = m_parms[i];
                int num2 = keyValuePair.IndexOf("=");
                string key = keyValuePair.Substring(0, num2);
                string value = keyValuePair.Substring(num2 + 1, keyValuePair.Length - num2 - 1);

                this[key] = value;
            }
        }
        /// <summary>
        /// Initializes a new instance_ of the ParameterizedString class by using the
        /// supplied base string and initial parameters.
        /// </summary>
        /// <param name="str">The base string</param>
        /// <param name="parms">The initial parameters</param>
        public ParameterizedString(string str, string parms): this(str, parms, (char)0, null)
        {
        }
        #endregion

        /// <summary>
        /// Preprocesses the base string
        /// </summary>
        /// <param name="str">The string to processes</param>
        private void Parameterize()
        {
            char c = ' ';

            // the base string will be modified by this function, so we
            // need to copy it to another temporary string.
            string str = _base;

            bool b = false; // set to true when the first [delimiter] character is found.
                            // set to false when true and a "space" character is found.
            int begin = 0;  // set to the position of the first [delimiter]
            int removed = 0;

            for (int i = 0, j = str.Length; i < j; i++)
            {
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
            }
        }

        public virtual ParameterizedString DeepCopy()
        {
            ParameterizedString str = new ParameterizedString(this._parms.Count, this._base, this.delimiter_);
            
            lock (_sync)
            {
                // deep copy the break characters
                int num = this._breaks.Length;
                str._breaks = new char[num];
                Buffer.BlockCopy(this._breaks, 0, str._breaks, 0, num);

                // deep copy the parameters
                foreach (KeyValuePair<string, Parameter> parm in _parms) {
                    string key = parm.Key;
                    Parameter p = parm.Value;

                    Parameter copy = new Parameter();
                    copy.Position = p.Position;
                    copy.Value = p.Value;

                    str._parms[key] = copy;
                }
            }
            return str;
        }

        public string this[string key] {
            get
            {
                Parameter parm = _parms[key];
                return (parm == null) ? null : parm.Value;
            }
            set
            {
                Parameter parm = _parms[key];
                if(parm != null)
                    parm.Value = value;
            }
        }

        public string[] Parameters
        {
            get
            {
                ICollection<string> keys = _parms.Keys;
                string[] s = new string[keys.Count];
                keys.CopyTo(s, 0);
                return s;
            }
        }

        /// <summary>
        /// Serializes this instance_ into a string object
        /// </summary>
        /// <returns>A string representation of this instance_</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(_base);

            lock (_sync)
            {
                int included = 0;
                string val;
                foreach (Parameter parm in _parms.Values)
                {
                    val = parm.Value;
                    str.Insert(parm.Position + included, val);
                    included += val.Length;
                }
            }

            return str.ToString();
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access
        /// to the ParameterizedString.
        /// </summary>
        public object SyncRoot
        {
            get { return _sync; }
        }
    }
}
