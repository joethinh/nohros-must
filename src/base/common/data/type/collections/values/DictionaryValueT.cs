using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// A recursive data storage class optimized for the namely storing a hierarchical
    /// tree of simple <see cref="Values"/>.
    /// <remarks>
    /// This class specifies a recursive data storage class. So it is fairly expressive.
    /// However, the API is optimized for the common case, namely storing a hierarchical
    /// tree of simple values. Given a DictionaryValue root, you can easily do things like:
    /// <para>
    /// <code>
    /// root.SetString("global.pages.homepage", "http://sys.nohros.com");
    /// string homepage = "http://nohros.com";  // default/fallback value
    /// homepage = root.GetString("global.pages.homepage", out homepage);
    /// </code>
    /// </para>
    /// where "global" and "pages" are also DictionaryValues, and "homepage"
    /// is a string setting.  If some elements of the path didn't exist yet,
    /// the SetString() method would create the missing elements and attach them
    /// to root before attaching the homepage value.
    /// </summary>
    public class DictionaryValue<T> : Value, IDictionaryValue where T: class, IValue
    {
        Dictionary<string, IValue> dictionary_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the DictionaryValue class.
        /// </summary>
        public DictionaryValue(): base(ValueType.TYPE_GENERIC_DICTIONARY) {
            dictionary_ = new Dictionary<string, IValue>();
        }
        #endregion

        /// <summary>
        /// Adds the specified key and value to the DictionaryValue.
        /// </summary>
        /// <param name="path">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null.</param>
        public void Add(string path, T value) {
            this[path] = value;
        }

        /// <summary>
        /// Determines whether the <typeparamref name="Nohros.Data.DictionaryValue"/> contains
        /// the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the<typeparamref name="Nohros.Data.DictionaryValue"/></param>
        /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an element with the
        /// specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">key is null</exception>
        public bool HasKey(string key) {
            return dictionary_.ContainsKey(key);
        }

        /// <summary>
        /// Gets a <see cref="IValue"/> object from dictionary, using the specified path.
        /// </summary>
        /// <param name="path">The path of the value to get.</param>
        /// <returns>An IValue instance with path <paramref name="path"/> or null if <paramref name="path"/> is
        /// not found.</returns>
        /// <remarks>
        /// This method is used internally to traverse the internal dictionary tree. The elements of the internal
        /// dictionary could be a <see cref="DictionaryValue&lt;T&gt;"/> or a <typeparamref name="T"/>. So, to traverse it
        /// recursivelly we need a method that returns a object of the type <see cref="IValue"/> instead of an object of the
        /// type <paramref name="T"/>.
        /// </remarks>
        IValue GetValue(string path) {
            string key = path;
            int delimiter_position = path.IndexOf('.', 0);
            if (delimiter_position != -1)
                key = path.Substring(0, delimiter_position);

            IValue entry;
            if (!dictionary_.TryGetValue(key, out entry))
                return null;

            if (delimiter_position == -1)
                return entry;

            if (entry.ValueType == ValueType.TYPE_GENERIC_DICTIONARY) {
                DictionaryValue<T> dictionary = entry as DictionaryValue<T>;
                return dictionary.GetValue(path.Substring(delimiter_position + 1));
            }

            return null;
        }

        /// <summary>
        /// Gets a <see cref="IValue"/> object from dictionary, using the specified path.
        /// </summary>
        /// <param name="path">The path of the value to get.</param>
        /// <param name="out_value">When this method returns contains an IValue object associated with the
        /// <paramref name="path"/> or null if <paramref name="path"/> is not found.</param>
        /// <returns>true if <paramref name="path"/> is foound; otherwise null.</returns>
        /// <remarks>
        /// This method is used internally to traverse the internal dictionary tree. The elements of the internal
        /// dictionary could be a <see cref="DictionaryValue&lt;T&gt;"/> or a <typeparamref name="T"/>. So, to traverse it
        /// recursivelly we need a method that returns a object of the type <see cref="IValue"/> instead of an object of the
        /// type <paramref name="T"/>.
        /// </remarks>
        bool GetValue(string path, out IValue out_value) {
            out_value = GetValue(path);
            return (out_value != null);
        }

        /// <summary>
        /// Sets the <see cref="IValue"/> object associated with the given path string.
        /// </summary>
        /// <param name="path">The path of the value to get.</param>
        /// <returns>An IValue instance with path <paramref name="path"/> or null if <paramref name="path"/> is
        /// not found.</returns>
        /// <remarks>
        /// This method is used internally to traverse the internal dictionary tree. The elements of the internal
        /// dictionary could be a <see cref="DictionaryValue&lt;T&gt;"/> or a <typeparamref name="T"/>. So, to traverse it
        /// recursivelly we need a method that returns a object of the type <see cref="IValue"/> instead of an object of the
        /// type <paramref name="T"/>.
        /// </remarks>
        void SetValue(string path, IValue value) {
            string key = path;
            int delimiter_position = path.IndexOf('.', 0);

            // If there isn't a dictionary delimiter in the path, we're done.
            if (delimiter_position == -1) {
                dictionary_[key] = value;
                return;
            }
            else {
                key = path.Substring(0, delimiter_position);
            }

            // Assume we're are indexing into a dictionary.
            IValue dict;
            DictionaryValue<T> entry = null;
            if (dictionary_.TryGetValue(key, out dict) && dict.ValueType == ValueType.TYPE_GENERIC_DICTIONARY) {
                entry = dict as DictionaryValue<T>;
            }
            else {
                entry = new DictionaryValue<T>();
                dictionary_[key] = entry;
            }

            entry.SetValue(path.Substring(delimiter_position + 1), value);
        }

        /// <summary>
        /// Gets or sets the <typeparamref name="T"/> associated with the given path starting
        /// from this object.
        /// </summary>
        /// <param name="path">The path of the value to get or set</param>
        /// <returns>The value of the last key in the path if it can be resolved successfully; otherwise,
        /// it will return a null reference.
        /// </returns>
        /// <remarks>A path has the form "&lt;key&gt;" or "&lt;key&gt;.&lt;key&gt;.[...]", where "." indexes
        /// into the next <typeparamref name="Nohros.Data.DictionaryValue"/> down. Obviously,
        /// "." can't be used within a key, but there are no other restrictions on keys.
        /// <para>
        /// If the key at any step of the way doesn't exist, or exists but isn't
        /// a <typeparamref name="DictionaryValue&lt;T&gt;"/>, a new <typeparamref name="DictionaryValue&lt;T&gt;"/>
        /// instance will be created and attached to the path in that location.
        /// </para>
        /// </remarks>
        public T this[string path] {
            get {
                T t = GetValue(path) as T;
                return t;
            }
            set {
                SetValue(path, value);
            }
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> associated with the given path starting
        /// from this object.
        /// </summary>
        /// <param name="path">The path to get</param>
        /// <param name="out_value"></param>
        /// <returns>A path has the form "&alt;key&gt" or "&alt;key&gt.&alt;key&gt.[...]", where
        /// "." indexes into the next <typeparamref name="DictionaryValue&lt;T&gt;"/> down.
        /// <para>
        /// If the path can be resolved successfully, the value for the last key in the path will
        /// be returned through the "value" parameter, and the function will return true.
        /// Otherwise, it will return false and "value" will contains null.
        /// </para>
        /// </returns>
        public bool Get(string path, out T out_value) {
            out_value = this[path];
            if (out_value == null)
                return false;
            return true;
        }
        
        /// <summary>
        /// Removes the <typeparamref name="T"/> object with the specified path
        /// from this dictionary(or one of its child dictionaries, if the path is more that just
        /// a local key).
        /// </summary>
        /// <param name="path">The path of the item to remove</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will contain a
        /// reference to the removed value or null if the specified path is not found.</param>
        /// <returns>true if the specified path is found and successfully removed; otherwise, false</returns>
        public bool Remove(string path, out T out_value) {
            out_value = Remove(path);
            if (out_value == null)
                return false;
            return true;
        }

        /// <summary>
        /// Removes the <typeparamref name="T"/> object with the specified path
        /// from this dictionary(or one of its child dictionaries, if the path is more that just
        /// a local key).
        /// </summary>
        /// <param name="path">The path of the item to remove</param>
        /// <returns>A reference to the removed value or null if the specified path is not found.</returns>
        public T Remove(string path) {
            string key = path;

            int delimiter_position = path.IndexOf('.', 0);
            if (delimiter_position != -1) {
                key = path.Substring(0, delimiter_position);
            }

            IValue entry = GetValue(path);
            if (entry == null)
                return null;

            if (delimiter_position == -1) {
                if (entry is T) {
                    dictionary_.Remove(path);
                    return (T)entry;
                }
                return null;
            }

            if (entry.IsType(ValueType.TYPE_GENERIC_DICTIONARY)) {
                return ((DictionaryValue<T>)entry).Remove(path.Substring(delimiter_position + 1));
            }

            return null;
        }

        public override IValue DeepCopy() {
            DictionaryValue result = new DictionaryValue();
            foreach (KeyValuePair<string, IValue> pair in dictionary_) {
                result[pair.Key] = pair.Value;
            }
            return result;
        }

        public override bool Equals(IValue other) {
            if (other.ValueType != ValueType)
                return false;

            DictionaryValue<T> other_dict = other as DictionaryValue<T>;
            Dictionary<string, IValue>.KeyCollection keys = dictionary_.Keys;

            if (keys.Count != other_dict.dictionary_.Keys.Count)
                return false;

            IValue lhs, rhs;
            foreach (string path in keys) {
                if (!GetValue(path, out lhs) || !other_dict.GetValue(path, out rhs) || !lhs.Equals(rhs)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Copies the elements of the <see cref="DictionaryValue"/> to a new array.
        /// </summary>
        /// <typeparam name="T">The type of elements to copy.</typeparam>
        /// <returns>An array containing copies of the elements of the <see cref="DictionaryValue"/></returns>
        /// <remarks>Only the elements of the type <typeparamref name="T"/> are copied.
        /// <para>
        /// This method is an O(n) operation, where n is <see cref="Size"/>
        /// </para>
        /// <para>
        /// ToArray&lt;T&gt; will never return a null reference; however, the returned array
        /// will contain zero elements if the dictionary contains no elements.
        /// </para>
        /// </remarks>
        public T[] ToArray() {
            if (Size == 0)
                return new T[0];

            int pos = 0;
            T[] destination_array = new T[Size];
            Dictionary<string, IValue>.ValueCollection values = dictionary_.Values;

            // filtering the elements of the type T.
            foreach (T value in values) {
                // all the elements must be an instance of T.
                if(value.ValueType == ValueType.TYPE_GENERIC_DICTIONARY)
                    return new T[0];

                destination_array[pos++] = value;
            }

            return destination_array;
        }

        #region IDictionaryValue
        bool IDictionaryValue.Get(string path, out IValue out_value) {
            out_value = null;
            return false;
        }

        void IDictionaryValue.Add(string path, IValue value) {
        }

        bool IDictionaryValue.Remove(string path, out IValue out_value) {
            out_value = null;
            return false;
        }

        IValue IDictionaryValue.Remove(string path) {
            return null;
        }

        IValue IDictionaryValue.this[string path] {
            get { return null; }
            set { }
        }
        #endregion

        /// <summary>
        /// Gets the number of elements in this dictionary.
        /// </summary>
        public int Size {
            get { return dictionary_.Count; }
        }
    }
}