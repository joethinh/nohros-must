using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Collections
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
  public class DictionaryValue: Value, IDictionaryValue, IEnumerable
  {
    Dictionary<string, IValue> dictionary_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance_ of the DictionaryValue class.
    /// </summary>
    public DictionaryValue()
      : base(ValueType.TYPE_DICTIONARY) {
      dictionary_ = new Dictionary<string, IValue>();
    }

    ~DictionaryValue() {
      Clear();
    }
    #endregion

    /// <summary>
    /// Adds the specified key and value to the DictionaryValue.
    /// </summary>
    /// <param name="path">The key of the element to add.</param>
    /// <param name="value">The value of the element to add. The value can be null.</param>
    public void Add(string path, IValue value) {
      this[path] = value;
    }

    /// <summary>
    /// Removes all keys and values from the <typeparamref name="Nohros.Data.DictionaryValue"/>
    /// </summary>
    public void Clear() {
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
    public bool HasKey(string key) {
      return dictionary_.ContainsKey(key);
    }

    /// <summary>
    /// Convenience form of <see cref="Nohros.Data.DictionaryValue.Set()"/>. This method will
    /// replace any existing value at that path, even if it has a diferrent type.
    /// </summary>
    /// <param name="path">The path to set</param>
    /// <param name="in_value">The boolean value to set</param>
    /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
    /// element with the specified key; otherwise, false</returns>
    public void SetBoolean(string path, bool in_value) {
      this[path] = CreateBooleanValue(in_value);
    }

    /// <summary>
    /// Convenience form of <see cref="Nohros.Data.DictionaryValue.Set()"/>. This method will
    /// replace any existing value at that path, even if it has a diferrent type.
    /// </summary>
    /// <param name="path">The path to set</param>
    /// <param name="in_value">The integer value to set</param>
    /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
    /// element with the specified key; otherwise, false</returns>
    public void SetInteger(string path, int in_value) {
      this[path] = CreateIntegerValue(in_value);
    }

    /// <summary>
    /// Convenience form of <see cref="Nohros.Data.DictionaryValue.Set()"/>. This method will
    /// replace any existing value at that path, even if it has a diferrent type.
    /// </summary>
    /// <param name="path">The path to set</param>
    /// <param name="in_value">The double value to set</param>
    /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
    /// element with the specified key; otherwise, false</returns>
    public void SetReal(string path, double in_value) {
      this[path] = CreateRealValue(in_value);
    }

    /// <summary>
    /// Convenience form of <see cref="Nohros.Data.DictionaryValue.Set()"/>. This method will
    /// replace any existing value at that path, even if it has a diferrent type.
    /// </summary>
    /// <param name="path">The path to set</param>
    /// <param name="in_value">The string value to set</param>
    /// <returns>true if the <typeparamref name="Nohros.Data.DictionaryValue"/> contains an
    /// element with the specified key; otherwise, false</returns>
    public void SetString(string path, string in_value) {
      this[path] = CreateStringValue(in_value);
    }

    /// <summary>
    /// Gets or sets the <typeparamref name="Nohros.Data.Value"/> associated with the given path starting
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
    /// a <typeparamref name="Nohros.Data.DictionaryValue"/>, a new <typeparamref name="Nohros.Data.DictionaryValue"/>
    /// instance will be created and attached to the path in that location.
    /// </para>
    /// </remarks>
    public IValue this[string path] {
      get {
        string key = path;
        int delimiter_position = path.IndexOf('.', 0);
        if (delimiter_position != -1)
          key = path.Substring(0, delimiter_position);

        IValue entry;
        if (!dictionary_.TryGetValue(key, out entry))
          return null;

        if (delimiter_position == -1)
          return entry;

        if (entry.ValueType == ValueType.TYPE_DICTIONARY) {
          DictionaryValue dictionary = entry as DictionaryValue;
          return dictionary[path.Substring(delimiter_position + 1)];
        }

        return null;
      }
      set {
        string key = path;
        int delimiter_position = path.IndexOf('.', 0);

        // If there isn't a dictionary delimiter in the path, we're done.
        if (delimiter_position == -1) {
          dictionary_[key] = value;
          return;
        } else {
          key = path.Substring(0, delimiter_position);
        }

        // Assume we're are indexing into a dictionary.
        IValue dict;
        DictionaryValue entry = null;
        if (dictionary_.TryGetValue(key, out dict) && dict.ValueType == ValueType.TYPE_DICTIONARY) {
          entry = dict as DictionaryValue;
        } else {
          entry = new DictionaryValue();
          dictionary_[key] = entry;
        }

        entry[path.Substring(delimiter_position + 1)] = value;
      }
    }

    #region Get...(..., out ...) overloads
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
    public bool Get(string path, out IValue out_value) {
      out_value = this[path];
      if (out_value == null)
        return false;
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
    /// and the value at the end of the path represents a boolean value.
    /// </remarks>
    public bool GetBoolean(string path, out bool out_value) {
      IValue value;

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
    public bool GetInteger(string path, out int out_value) {
      IValue value;

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
    public bool GetReal(string path, out double out_value) {
      IValue value;

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
    public bool GetString(string path, out string out_value) {
      IValue value;

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
    public bool GetDictionary(string path, out DictionaryValue out_value) {
      IValue value;

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
    public bool GetList(string path, out ListValue out_value) {
      IValue value;

      out_value = null;
      if (!Get(path, out value) || !value.IsType(ValueType.TYPE_LIST))
        return false;

      out_value = value as ListValue;

      return true;
    }
    #endregion

    #region [ValueType] Get...(string) overloads
    /// <summary>
    /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
    /// </summary>
    /// <param name="path">The path to get</param>
    /// <returns>A reference to an <typeparamref name="System.String"/> object associated with the
    /// specified path or null if the specified path is not found.</returns>
    public string GetString(string path) {
      IValue entry;
      if (!Get(path, out entry))
        return null;
      return entry.GetAsString();
    }

    /// <summary>
    /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
    /// </summary>
    /// <param name="path">The path to get</param>
    /// <returns>A reference to an <typeparamref name="Nohros.Data.DictionaryValue"/> object associated with the
    /// specified path or null if the specified path is not found.</returns>
    public DictionaryValue GetDictionary(string path) {
      IValue value;
      if (!Get(path, out value) || !value.IsType(ValueType.TYPE_DICTIONARY))
        return null;

      return value as DictionaryValue;
    }

    /// <summary>
    /// This is a convenience form of <see cref="Nohros.Data.DictionaryValue.Get()"/>
    /// </summary>
    /// <param name="path">The path to get</param>
    /// <param name="out_value">When this method returns <paramref name="out_value"/> will hold
    /// a reference to an <typeparamref name="Nohros.Data.ListValue"/> object associated with the
    /// specified path or null if the specified path is not found.</param>
    /// <returns>A reference to an <typeparamref name="Nohros.Data.ListValue"/> object associated with the
    /// specified path or null if the specified path is not found.</returns>
    public ListValue GetList(string path) {
      IValue value;
      if (!Get(path, out value) || !value.IsType(ValueType.TYPE_LIST))
        return null;

      return value as ListValue;
    }
    #endregion

    /// <summary>
    /// Removes the <typeparamref name="Nohros.Data.Value"/> object with the specified path
    /// from this dictionary(or one of its child dictionaries, if the path is more that just
    /// a local key).
    /// </summary>
    /// <param name="path">The path of the item to remove</param>
    /// <param name="out_value">When this method returns <paramref name="out_value"/> will contain a
    /// reference to the removed value or null if the specified path is not found.</param>
    /// <returns>true if the specified path is found and successfully removed; otherwise, false</returns>
    public bool Remove(string path, out IValue out_value) {
      out_value = Remove(path);
      if (out_value == null)
        return false;
      return true;
    }

    /// <summary>
    /// Removes the <typeparamref name="Nohros.Data.Value"/> object with the specified path
    /// from this dictionary(or one of its child dictionaries, if the path is more that just
    /// a local key).
    /// </summary>
    /// <param name="path">The path of the item to remove</param>
    /// <returns>A reference to the removed value or null if the specified path is not found.</returns>
    public IValue Remove(string path) {
      string key = path;

      int delimiter_position = path.IndexOf('.', 0);
      if (delimiter_position != -1) {
        key = path.Substring(0, delimiter_position);
      }

      IValue entry = null;
      if (!Get(path, out entry))
        return null;

      if (delimiter_position == -1) {
        dictionary_.Remove(path);
        return entry;
      }

      if (entry.IsType(ValueType.TYPE_DICTIONARY)) {
        return ((DictionaryValue)entry).Remove(path.Substring(delimiter_position + 1));
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

      DictionaryValue other_dict = other as DictionaryValue;
      Dictionary<string, IValue>.KeyCollection keys = dictionary_.Keys;

      if (keys.Count != other_dict.dictionary_.Keys.Count)
        return false;

      IValue lhs, rhs;
      foreach (string path in keys) {
        if (!Get(path, out lhs) || !other_dict.Get(path, out rhs) || !lhs.Equals(rhs)) {
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
    /// This method is an O(n2) operation, where n is <see cref="Size"/>
    /// </para>
    /// <para>
    /// ToArray&lt;T&gt; will never return a null reference; however, the returned array
    /// will contain zero elements if the dictionary contains no elements.
    /// </para>
    /// </remarks>
    public T[] ToArray<T>() where T: class {
      if (Size == 0)
        return new T[0];

      int pos = 0;
      T[] destination_array = new T[Size];
      T element;
      Dictionary<string, IValue>.ValueCollection values = dictionary_.Values;

      // filtering the elements of the type T.
      foreach (IValue value in values) {
        element = value as T;
        if (element != null)
          destination_array[pos++] = element;
      }

      // if all the elements is of the specified type, just return.
      if (pos == Size)
        return destination_array;

      // we need to redimension the array, eliminating the unused slots.
      T[] end_array = new T[pos];
      Array.Copy(destination_array, 0, end_array, 0, pos);

      return end_array;
    }

    #region IEnumerable
    public IEnumerator GetEnumerator() {
      Dictionary<string, IValue>.ValueCollection values = dictionary_.Values;

      // filtering the elements of the type T.
      foreach (IValue value in values) {
        // all the elements must be an instance of T.
        if (value.ValueType == ValueType.TYPE_GENERIC_DICTIONARY)
          continue;
        yield return value;
      }
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