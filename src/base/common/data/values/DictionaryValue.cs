using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    public class DictionaryValue : Value
    {
        Dictionary<string, Value> dictionary_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the DictionaryValue class.
        /// </summary>
        public DictionaryValue()
            : base(ValueType.TYPE_DICTIONARY)
        {
            dictionary_ = new Dictionary<string, Value>();
        }

        ~DictionaryValue()
        {
            Clear();
        }
        #endregion

        /// <summary>
        /// Removes all keys and values from the <typeparamref name="Nohros.Data.DictionaryValue"/>
        /// </summary>
        public void Clear()
        {
            dictionary_.Clear();
        }

        /// <summary>
        /// Determines whether the <typeparamref name="Nohros.Data.DictionaryValue"/> contains
        /// the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the<typeparamref name="Nohros.Data.DictionaryValue"/></param>
        /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an element with the
        /// specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">key is null</exception>
        public bool HasKey(string key)
        {
            return dictionary_.ContainsKey(key);
        }

        /// <summary>
        /// Sets the <typeparamref name="Nohros.Data.Value"/> associated with the given path starting
        /// from this object.
        /// </summary>
        /// <param name="path">The path of the value to set.</param>
        /// <param name="in_value">The value to set.</param>
        /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
        /// element with the specified key; otherwise, false</returns>
        /// <remarks>
        /// A path has the form "&alt;key&gt" or "&alt;key&gt.&alt;key&gt.[...]", where
        /// "." indexes into the next <typeparamref name="Nohros.Data.DictionaryValue"/> down. Obviously,
        /// "." can't be used within a key, but there are no other restrictions on keys.
        /// <para>
        /// If the key at any step of the way doesn't exist, or exists but isn't
        /// a <typeparamref name="Nohros.Data.DictionaryValue"/>, a new <typeparamref name="Nohros.Data.DictionaryValue"/>
        /// will be created and attached to the path in that location.
        /// </para>
        /// </remarks>
        public bool Set(string path, Value in_value)
        {
            string key = path;
            int delimiter_position = path.IndexOf('.', 0);

            // If there isn't a dictionary delimiter in the path, we're done.
            if (delimiter_position == -1) {
                dictionary_[key] = in_value;
                return true;
            } else {
                key = path.Substring(0, delimiter_position);
            }

            // Assume we're are indexing into a dictionary.
            Value value;
            DictionaryValue entry = null;
            if (dictionary_.TryGetValue(key, out value) && value.Type == ValueType.TYPE_DICTIONARY) {
                entry = value as DictionaryValue;
            }
            else {
                entry = new DictionaryValue();
                dictionary_[key] = entry;
            }

            return entry.Set(path.Substring(delimiter_position + 1), in_value);
        }

        /// <summary>
        /// Convenience form of <see cref="Nohros.Data.DictionaryValue.Set()"/>. This method will
        /// replace any existing value at that path, even if it has a diferrent type.
        /// </summary>
        /// <param name="path">The path to set</param>
        /// <param name="in_value">The boolean value to set</param>
        /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
        /// element with the specified key; otherwise, false</returns>
        public bool SetBoolean(string path, bool in_value)
        {
            return Set(path, CreateBooleanValue(in_value));
        }

        /// <summary>
        /// Convenience form of <see cref="Nohros.Data.DictionaryValue.Set()"/>. This method will
        /// replace any existing value at that path, even if it has a diferrent type.
        /// </summary>
        /// <param name="path">The path to set</param>
        /// <param name="in_value">The integer value to set</param>
        /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
        /// element with the specified key; otherwise, false</returns>
        public bool SetInteger(string path, int in_value)
        {
            return Set(path, CreateIntegerValue(in_value));
        }

        /// <summary>
        /// Convenience form of <see cref="Nohros.Data.DictionaryValue.Set()"/>. This method will
        /// replace any existing value at that path, even if it has a diferrent type.
        /// </summary>
        /// <param name="path">The path to set</param>
        /// <param name="in_value">The double value to set</param>
        /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
        /// element with the specified key; otherwise, false</returns>
        public bool SetReal(string path, double in_value)
        {
            return Set(path, CreateRealValue(in_value));
        }

        /// <summary>
        /// Convenience form of <see cref="Nohros.Data.DictionaryValue.Set()"/>. This method will
        /// replace any existing value at that path, even if it has a diferrent type.
        /// </summary>
        /// <param name="path">The path to set</param>
        /// <param name="in_value">The string value to set</param>
        /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
        /// element with the specified key; otherwise, false</returns>
        public bool SetString(string path, string in_value)
        {
            return Set(path, CreateStringValue(in_value));
        }

        /// <summary>
        /// Gets the <typeparamref name="Nohros.Data.Value"/> associated with the given path starting
        /// from this object.
        /// </summary>
        /// <param name="path">The path to get</param>
        /// <param name="out_value"></param>
        /// <returns>A path has the form "&alt;key&gt" or "&alt;key&gt.&alt;key&gt.[...]", where
        /// "." indexes into the next <typeparamref name="Nohros.Data.DictionaryValue"/> down.
        /// <para>
        /// If the path can be resolved successfully, the value for the last key in the path will
        /// be returned through the "value" parameter, and the function will return true.
        /// Otherwise, it will return false and "value" will contains null.
        /// </para>
        /// </returns>
        public bool Get(string path, out Value out_value)
        {
            string key = path;
            int delimiter_position = path.IndexOf('.', 0);
            if (delimiter_position != -1) {
                key = path.Substring(0, delimiter_position);
            }

            out_value = null;

            Value entry;
            if (!dictionary_.TryGetValue(key, out entry))
                return false;

            if (delimiter_position == -1) {
                out_value = entry;
                return true;
            }

            if(entry.Type == ValueType.TYPE_DICTIONARY) {
                DictionaryValue dictionary = entry as DictionaryValue;
                return dictionary.Get(path.Substring(delimiter_position + 1), out out_value);
            }

            return false;
        }

        /// <summary>
        /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
        /// </summary>
        /// <param name="path">The path to get</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will hold
        /// a reference to an <typeparamref name="Nohros.Data.Value"/> object associated with the
        /// specified path or null if the specified path is not found.</param>
        /// <remarks>
        /// The value will be retrieved and the return value will be true if the path is valid
        /// and the value at the end of the path represents a boolean value.
        /// </remarks>
        public bool GetBoolean(string path, out bool out_value)
        {
            Value value;

            out_value = default(bool);
            if (!Get(path, out value))
                return false;
            return value.GetAsBoolean(out out_value);
        }

        /// <summary>
        /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
        /// </summary>
        /// <param name="path">The path to get</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will hold
        /// a reference to an <typeparamref name="Nohros.Data.Value"/> object associated with the
        /// specified path or null if the specified path is not found.</param>
        /// <remarks>
        /// The value will be retrieved and the return value will be true if the path is valid
        /// and the value at the end of the path represents a integer value.
        /// </remarks>
        public bool GetInteger(string path, out int out_value)
        {
            Value value;

            out_value = default(int);
            if (!Get(path, out value))
                return false;
            return value.GetAsInteger(out out_value);
        }

        /// <summary>
        /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
        /// </summary>
        /// <param name="path">The path to get</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will hold
        /// a reference to an <typeparamref name="Nohros.Data.Value"/> object associated with the
        /// specified path or null if the specified path is not found.</param>
        /// <remarks>
        /// The value will be retrieved and the return value will be true if the path is valid
        /// and the value at the end of the path represents a double value.
        /// </remarks>
        public bool GetReal(string path, out double out_value)
        {
            Value value;

            out_value = default(double);
            if (!Get(path, out value))
                return false;
            return value.GetAsReal(out out_value);
        }

        /// <summary>
        /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
        /// </summary>
        /// <param name="path">The path to get</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will hold
        /// a reference to an <typeparamref name="Nohros.Data.Value"/> object associated with the
        /// specified path or null if the specified path is not found.</param>
        /// <remarks>
        /// The value will be retrieved and the return value will be true if the path is valid
        /// and the value at the end of the path represents a boolean value.
        /// </remarks>
        public bool GetString(string path, out string out_value)
        {
            Value value;

            out_value = null;
            if (!Get(path, out value))
                return false;
            return value.GetAsString(out out_value);
        }

        /// <summary>
        /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
        /// </summary>
        /// <param name="path">The path to get</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will hold
        /// a reference to an <typeparamref name="Nohros.Data.Value"/> object associated with the
        /// specified path or null if the specified path is not found.</param>
        /// <remarks>
        /// The value will be retrieved and the return value will be true if the path is valid
        /// and the value at the end of the path represents a <typeparamref name="Nohros.Data.DictionaryValue"/>.
        /// </remarks>
        public bool GetDictionary(string path, out DictionaryValue out_value)
        {
            Value value;

            out_value = null;
            if (!Get(path, out value) || !value.IsType(ValueType.TYPE_DICTIONARY))
                return false;

            out_value = value as DictionaryValue;

            return true;
        }

        /// <summary>
        /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
        /// </summary>
        /// <param name="path">The path to get</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will hold
        /// a reference to an <typeparamref name="Nohros.Data.Value"/> object associated with the
        /// specified path or null if the specified path is not found.</param>
        /// <remarks>
        /// The value will be retrieved and the return value will be true if the path is valid
        /// and the value at the end of the path represents a <typeparamref name="Nohros.Data.ListValue"/>.
        /// </remarks>
        public bool GetList(string path, out ListValue out_value)
        {
            Value value;

            out_value = null;
            if (!Get(path, out value) || !value.IsType(ValueType.TYPE_LIST))
                return false;

            out_value = value as ListValue;

            return true;
        }

        /// <summary>
        /// Removes the <typeparamref name="Nohros.Data.Value"/> object with the specified path
        /// from this dictionary(or one of its child dictionaries, if the path is more that just
        /// a local key).
        /// </summary>
        /// <param name="path">The path of the item to remove</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will contain a
        /// reference to the removed value or null if the specified path is not found.</param>
        /// <returns>true if the specified path is found and successfully removed; otherwise, false</returns>
        public bool Remove(string path, out Value out_value)
        {
            string key = path;

            int delimiter_position = path.IndexOf('.', 0);
            if (delimiter_position != -1) {
                key = path.Substring(0, delimiter_position);
            }

            out_value = null;

            Value entry = null;
            if (!Get(path, out entry))
                return false;

            if (delimiter_position == -1) {
                out_value = entry;
                dictionary_.Remove(path);
                return true;
            }

            if (entry.IsType(ValueType.TYPE_DICTIONARY)) {
                return ((DictionaryValue)entry).Remove(path.Substring(delimiter_position + 1), out out_value);
            }

            return false;
        }

        public override Value DeepCopy()
        {
            DictionaryValue result = new DictionaryValue();
            foreach(KeyValuePair<string, Value> pair in dictionary_) {
                result.Set(pair.Key, pair.Value);
            }
            return result;
        }

        public override bool Equals(Value other)
        {
            if (other.Type != Type)
                return false;

            DictionaryValue other_dict = other as DictionaryValue;
            Dictionary<string, Value>.KeyCollection keys = dictionary_.Keys;

            if (keys.Count != other_dict.dictionary_.Keys.Count)
                return false;

            Value lhs, rhs;
            foreach (string path in keys) {
                if (!Get(path, out lhs) || !other_dict.Get(path, out rhs) || !lhs.Equals(rhs)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the number of elements in this dictionary.
        /// </summary>
        public int Size
        {
            get { return dictionary_.Count; }
        }
    }
}
