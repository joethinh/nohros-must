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
    public interface IDictionaryValue
    {
        /// <summary>
        /// Adds the specified key and value to the DictionaryValue.
        /// </summary>
        /// <param name="path">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null.</param>
        void Add(string path, IValue value);

        /// <summary>
        /// Determines whether the <typeparamref name="Nohros.Data.DictionaryValue"/> contains
        /// the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the<typeparamref name="Nohros.Data.DictionaryValue"/></param>
        /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an element with the
        /// specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">key is null</exception>
        bool HasKey(string key);

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
        IValue this[string path] { get; set; }

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
        bool Get(string path, out IValue out_value);

        /// <summary>
        /// Removes the <typeparamref name="T"/> object with the specified path
        /// from this dictionary(or one of its child dictionaries, if the path is more that just
        /// a local key).
        /// </summary>
        /// <param name="path">The path of the item to remove</param>
        /// <param name="out_value">When this method returns <paramref name="out_value"/> will contain a
        /// reference to the removed value or null if the specified path is not found.</param>
        /// <returns>true if the specified path is found and successfully removed; otherwise, false</returns>
        bool Remove(string path, out IValue out_value);

        /// <summary>
        /// Removes the <typeparamref name="T"/> object with the specified path
        /// from this dictionary(or one of its child dictionaries, if the path is more that just
        /// a local key).
        /// </summary>
        /// <param name="path">The path of the item to remove</param>
        /// <returns>A reference to the removed value or null if the specified path is not found.</returns>
        IValue Remove(string path);

        /// <summary>
        /// Gets the number of elements in this dictionary.
        /// </summary>
        int Size { get; }
    }
}