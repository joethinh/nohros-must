using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// A node that contains the common providers options.
  /// </summary>
  public partial class ProviderOptionsNode : AbstractConfigurationNode,
                                             IProviderOptions
  {
    readonly IDictionary<string, string> options_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderOptionsNode"/> by
    /// using the given name.
    /// </summary>
    public ProviderOptionsNode(string name)
      : base(name) {
      options_ = new Dictionary<string, string>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderOptionsNode"/> by
    /// using the given node name and options.
    /// </summary>
    /// <param name="name">
    /// The name of the node
    /// </param>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> object containing the options.
    /// </param>
    public ProviderOptionsNode(string name, IDictionary<string, string> options)
      : base(name) {
      if (options == null) {
        throw new ArgumentNullException("options");
      }
      options_ = options;
    }
    #endregion

    public IEnumerator GetEnumerator() {
      return options_.GetEnumerator();
    }

    IEnumerator<KeyValuePair<string, string>>
      IEnumerable<KeyValuePair<string, string>>.GetEnumerator() {
      return options_.GetEnumerator();
    }

    public bool TryGetString(string key, out string str) {
      return options_.TryGetValue(key, out str);
    }

    /// <inheritdoc/>
    public void Add(string key, string value) {
      options_.Add(key, value);
    }

    /// <inheritdoc/>
    public bool ContainsKeys(params string[] keys) {
      if (keys == null || keys.Length == 0) {
        return true;
      }

      for (int i = 0, j = keys.Length; i < j; i++) {
        if (!options_.ContainsKey(keys[i])) {
          return false;
        }
      }
      return true;
    }

    /// <inheritdoc/>
    public string GetString(string key) {
      return options_[key];
    }

    /// <inheritdoc/>
    public int GetInt(string key) {
      return int.Parse(options_[key]);
    }

    /// <inheritdoc/>
    public long GetLong(string key) {
      return long.Parse(options_[key]);
    }

    /// <inheritdoc/>
    public short GetShort(string key) {
      return short.Parse(options_[key]);
    }

    /// <inheritdoc/>
    public float GetFloat(string key) {
      return float.Parse(options_[key]);
    }

    /// <inheritdoc/>
    public double GetDouble(string key) {
      return double.Parse(options_[key]);
    }

    /// <inheritdoc/>
    public decimal GetDecimal(string key) {
      return decimal.Parse(options_[key]);
    }

    /// <inheritdoc/>
    public bool GetBoolean(string key) {
      return bool.Parse(options_[key]);
    }

    /// <inheritdoc/>
    public string TryGetString(string key, string default_value) {
      string value;
      if (!options_.TryGetValue(key, out value)) {
        return default_value;
      }
      return value;
    }

    /// <inheritdoc/>
    public int TryGetInteger(string key, int default_value) {
      string option;
      if (options_.TryGetValue(key, out option)) {
        int i;
        if (int.TryParse(option, out i)) {
          return i;
        }
      }
      return default_value;
    }

    /// <inheritdoc/>
    public long TryGetLong(string key, long default_value) {
      string option;
      if (options_.TryGetValue(key, out option)) {
        int i;
        if (int.TryParse(option, out i)) {
          return i;
        }
      }
      return default_value;
    }

    /// <inheritdoc/>
    public IDictionary<string, string> ToDictionary() {
      return new Dictionary<string, string>(options_);
    }

    /// <inheritdoc/>
    public string this[string key] {
      get { return options_[key]; }
      set { options_[key] = value; }
    }

    /// <inheritdoc/>
    public bool Remove(string key, string value) {
      return options_.Remove(key);
    }
  }
}
