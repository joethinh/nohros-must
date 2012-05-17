using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// A utility class for manage provider options.
  /// </summary>
  public sealed class ProviderOptions
  {
    /// <summary>
    /// Checks if the specified options keys exists in the options dictionary.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> to check for key existence.
    /// </param>
    /// <param name="keys">
    /// THe options keys to check for existence.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> is a null reference.
    /// </exception>
    /// <returns>
    /// <c>true</c> if all the specified keys exists in the given 
    /// <paramref name="options"/> dictionary; otherwise, <c>false</c>. If no
    /// keys is specified this method returns <c>true</c>.
    /// </returns>
    /// <remarks>
    /// This method checks for existence for each specified key and if one
    /// of them does not exists returns <c>false</c>.
    /// </remarks>
    public static bool CheckIfExists(IDictionary<string, string> options,
      params string[] keys) {
      if (options == null) {
        throw new ArgumentNullException("options");
      }

      if (keys == null) {
        return true;
      }

      for (int i = 0, j = keys.Length; i < j; i++) {
        if (!options.ContainsKey(keys[i])) {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Checks if the specified options keys exists in the options dictionary
    /// and return its values.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> to search for keys.
    /// </param>
    /// <param name="keys">
    /// The options keys to check for existence.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> or <paramref name="keys"/> is a null
    /// reference.
    /// </exception>
    /// <returns>
    /// An array of string containing the values for the specified option keys.
    /// If no keys is specified this method returns an empty array.
    /// </returns>
    /// <remarks>
    /// This method checks the existence of each specified key and if one
    /// of them does not exists returns an empty string array.
    /// </remarks>
    public static string[] GetIfExists(IDictionary<string, string> options,
      params string[] keys) {
      if (options == null || keys == null) {
        throw new ArgumentNullException(options == null ? "options" : "keys");
      }

      int j = keys.Length;
      string[] values = new string[j];
      for (int i = 0; i < j; i++) {
        if (!options.TryGetValue(keys[i], out values[i])) {
          return new string[0];
        }
      }
      return values;
    }

    /// <summary>
    /// Checks if the specified option key exists in the options dictionary
    /// and return its values.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> to search for
    /// <paramref name="key"/>.
    /// </param>
    /// <param name="key">
    /// The option key to check for existence.
    /// </param>
    /// <param name="def_value">
    /// The value to be returned if the <paramref name="key"/> is not found.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> or <paramref name="key"/> is a null
    /// reference.
    /// </exception>
    /// <returns>
    /// A string containing the value for the specified option key.
    /// </returns>
    /// <remarks>
    /// This method checks the existence of each specified key and if it
    /// is not found returns <paramref name="def_value"/>.
    /// </remarks>
    public static string GetIfExists(IDictionary<string, string> options,
      string key, string def_value) {
      if (options == null || key == null) {
        throw new ArgumentNullException(options == null ? "options" : "key");
      }

      string option;
      bool ok = options.TryGetValue(key, out option);
      return ok ? option : def_value;
    }

    /// <summary>
    /// Checks if the specified options keys exists in the options dictionary
    /// and return its values.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> to check for key existence.
    /// </param>
    /// <param name="keys">
    /// The options keys to check for existence.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> or <paramref name="keys"/> are <c>null</c>.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// A given key does not exist in the specified <paramref name="options"/>
    /// dictionaty.
    /// </exception>
    /// <returns>
    /// An array of string containing the values for the specified option keys.
    /// </returns>
    /// <remarks>
    /// This method checks the existence of each specified key and if one
    /// of them does not exists throws an <see cref="KeyNotFoundException"/>.
    /// <para>
    /// If no keys is specified this method returns an empty array.
    /// </para>
    /// </remarks>
    public static string[] ThrowIfNotExists(IDictionary<string, string> options,
      params string[] keys) {
      if (options == null || keys == null) {
        throw new ArgumentNullException(options == null ? "options" : "keys");
      }

      int j = keys.Length;
      string[] values = new string[j];
      for (int i = 0; i < j; i++) {
        if (keys[i] == null || !options.TryGetValue(keys[i], out values[i])) {
          throw new KeyNotFoundException(keys[i]);
        }
      }
      return values;
    }

    /// <summary>
    /// Gets the value associated with the key <paramref name="key"/> and
    /// try to convert the value to its 32-bit signed integer equivalent.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> to get the value from.
    /// </param>
    /// <param name="key">
    /// The key that is associated with the value to get.
    /// </param>
    /// <param name="default_value">
    /// A 32-bit integer that will be returned when the key
    /// <paramref name="key"/> was not found.
    /// </param>
    /// <returns>
    /// The value associated with the key <paramref name="key"/> or
    /// <paramref name="default_value"/> if the key was not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> or <paramref name="key"/> is null.
    /// </exception>
    public static int TryGetInteger(IDictionary<string, string> options,
      string key, int default_value) {
      if (options == null || key == null) {
        throw new ArgumentNullException(options == null ? "options" : "key");
      }
      string option_string;
      if (options.TryGetValue(key, out option_string)) {
        int option_int;
        if (int.TryParse(option_string, out option_int)) {
          return option_int;
        }
      }
      return default_value;
    }
  }
}
