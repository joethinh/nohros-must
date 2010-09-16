using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// Implements a <see cref="IDictionary&lt;TKey,TValue&gt;"/> with the key and the value strongly typed
    /// to be strings rather than object.
    /// </summary>
    public class StringMap
    {
        Dictionary<string, string> strings_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the StringMap class that is empty, has the default initial capacity, and uses
        /// the default string equality comparer.
        /// </summary>
        public StringMap():this(0, null) { }

        /// <summary>
        /// Initializes a new instance of the StringMap class that contains elements copied from the specified <see cref="IDictionary&gt;,&lt;"/>
        /// and uses the default string equality comparer.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary&lt;,&gt;"/> whose elements are copied to the new StringMap object<./param>
        public StringMap(IDictionary<string, string> dictionary) : this(dictionary, null) { }

        /// <summary>
        /// Initializes a new instance of the StringMap class that is empty, has the default initial capacity, and use the
        /// specified <see cref="IEqualityComparer&lt;string&gt;"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer&lt;string&gt;"/>. implementation to use when comparing
        /// keys, or null to use the default string <see cref="EqualityComparer&lt;string&gt;"/>.</param>
        public StringMap(IEqualityComparer<string> comparer):this(0, comparer) { }

        /// <summary>
        /// Initializes a new instance of the StringMap class that is empty, has the specified
        /// initial capacity, and uses the default string comparer.
        /// </summary>
        /// <param name="capacity">The initial number of strings that the <see cref="IDictionary&lt;string,string&gt;"/> can contain.</param>
        public StringMap(int capacity):this(capacity, null) { }

        /// <summary>
        /// Initializes a new instance of the StringMap class that contains elements copied from the specified <see cref="IDictionary&gt;,&lt;"/>
        /// and uses the specified <see cref="IEqualityComparer&gt;string&lt;"/>
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary&gt;string,string&lt;"/> whose elements are copied to the new StringMap class.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer&gt;string&lt;"/> implementation to use when comparing
        /// keys, or null to use the default <see cref="EqualityComparer&gt;string&lt;"/></param>
        public StringMap(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) {
            strings_ = new Dictionary<string, string>(dictionary, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IDictionary&lt;string,string&gt;"/> class that is empty, has specified
        /// initial capacity, and uses the specified <see cref="IEqualityComparer&lt;string&gt;"/>.
        /// </summary>
        /// <param name="capacity">The initial number of strings that the StringMap can contain.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer&gt;string&lt;"/> implementation to use when comparing
        /// keys, or null to use the default <see cref="EqualityComparer&lt;string&gt;"/></param>
        public StringMap(int capacity, IEqualityComparer<string> comparer) {
            strings_ = new Dictionary<string, string>(capacity, comparer);
        }
        #endregion

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key,
        /// if the key is found; otherwise, a null reference. This parameter is passed uninitialized.</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out string value) {
            return strings_.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets or sets a string associated with the specified key.
        /// </summary>
        /// <param name="key">The key value to get or set.</param>
        /// <returns>The value associated with the specified key.If the specified key is not found attempt to get
        /// the value of this property returns null and attempt to set the value of this property creates a new element
        /// with the specified key.</returns>
        public string this[string key] {
            get {
                string str = null;
                strings_.TryGetValue(key, out str);
                return str;
            }
            set { strings_[key] = value; }
        }
    }
}