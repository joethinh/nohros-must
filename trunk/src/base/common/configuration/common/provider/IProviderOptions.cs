using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  public interface IProviderOptions : IEnumerable<KeyValuePair<string, string>>
  {
    /// <summary>
    /// Adds an element with the provided key and value to the
    /// <see cref="IProviderOptions"/> collection.
    /// </summary>
    /// <param name="key">
    /// The string to use an the key of the value to add.
    /// </param>
    /// <param name="value">
    /// The string to use as the value of the element to add.
    /// </param>
    void Add(string key, string value);

    /// <summary>
    /// Gets the number of options in the collection.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Checks if the specified options keys exists in the collection.
    /// </summary>
    /// <param name="keys">
    /// The keys to check for existence.
    /// </param>
    /// <returns>
    /// <c>true</c> if all the specified keys exists in the collection;
    /// otherwise, <c>false</c>. If no keys is specified this method returns
    /// <c>true</c>.
    /// </returns>
    /// <remarks>
    /// This method checks for existence for each specified key and if one
    /// of them does not exists returns <c>false</c>.
    /// </remarks>
    bool ContainsKeys(params string[] keys);

    /// <summary>
    /// Gets the value associated with the specified key as a string
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <returns>
    /// The value associated with the specified key as a string.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// The key <paramref name="key"/> does not exists in the collection.
    /// </exception>
    string GetString(string key);

    /// <summary>
    /// Gets the value associated with the specified key as a 32-bit
    /// signed integer.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <returns>
    /// The value associated with the specified key as a 32-bit
    /// signed integer.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The value associated with the key <paramref name="key"/> could not be
    /// casted to a 32-bit signed integer.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The key <paramref name="key"/> does not exists in the collection.
    /// </exception>
    int GetInt(string key);

    /// <summary>
    /// Gets the value associated with the specified key as a 64-bit
    /// signed integer.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <returns>
    /// The value associated with the specified key as a 64-bit
    /// signed integer.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The value associated with the key <paramref name="key"/> could not be
    /// casted to a 64-bit signed integer.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The key <paramref name="key"/> does not exists in the collection.
    /// </exception>
    long GetLong(string key);

    /// <summary>
    /// Gets the value associated with the specified key as a 16-bit
    /// signed integer.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <returns>
    /// The value associated with the specified key as a 16-bit
    /// signed integer.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The value associated with the key <paramref name="key"/> could not be
    /// casted to a 16-bit signed integer.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The key <paramref name="key"/> does not exists in the collection.
    /// </exception>
    short GetShort(string key);

    /// <summary>
    /// Gets the value associated with the specified key as a single-precision
    /// floating point number.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <returns>
    /// The value associated with the specified key as a single-precision
    /// floating point number.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The value associated with the key <paramref name="key"/> could not be
    /// casted to a float precision number.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The key <paramref name="key"/> does not exists in the collection.
    /// </exception>
    float GetFloat(string key);

    /// <summary>
    /// Gets the value associated with the specified key as a double-precision
    /// floating point number.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <returns>
    /// The value associated with the specified key as a double-precision
    /// floating point number.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The value associated with the key <paramref name="key"/> could not be
    /// casted to a double precision number.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The key <paramref name="key"/> does not exists in the collection.
    /// </exception>
    double GetDouble(string key);

    /// <summary>
    /// Gets the value associated with the specified key as a
    /// <see cref="Decimal"/> object.
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <returns>
    /// The value associated with the specified key as a <see cref="Decimal"/>
    /// object.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The value associated with the key <paramref name="key"/> could not be
    /// casted to a decimal
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The key <paramref name="key"/> does not exists in the collection.
    /// </exception>
    decimal GetDecimal(string key);

    /// <summary>
    /// Gets the value associated with the specified key as a
    /// <see cref="bool"/>
    /// </summary>
    /// <param name="key">
    /// The key associated with the value to get.
    /// </param>
    /// <returns>
    /// The value associated with the specified key as a <see cref="bool"/>
    /// object.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// The value associated with the key <paramref name="key"/> could not be
    /// casted to a <see cref="bool"/>
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The key <paramref name="key"/> does not exists in the collection.
    /// </exception>
    bool GetBoolean(string key);

    /// <summary>
    /// Gets the string value associated with the specified key.
    /// </summary>
    /// <param name="key">
    /// The key of the value to get.
    /// </param>
    /// <param name="default_value">
    /// The value to be returned if the <paramref name="key"/> is not found.
    /// </param>
    /// <returns>
    /// A string containing the value for the specified option key, or the
    /// value of <paramref name="default_value"/> if <paramref name="key"/>
    /// is not found.
    /// </returns>
    string TryGetString(string key, string default_value);

    bool TryGetString(string key, out string str);

    /// <summary>
    /// Gets the string value associated with the key
    /// <paramref name="key"/> and try to convert it to its 32-bit signed
    /// integer equivalent.
    /// </summary>
    /// <param name="key">
    /// The key that is associated with the value to get.
    /// </param>
    /// <param name="default_value">
    /// A 32-bit integer that will be returned when the key
    /// <paramref name="key"/> is not found.
    /// </param>
    /// <returns>
    /// The value associated with the key <paramref name="key"/> or
    /// <paramref name="default_value"/> if the key was not found.
    /// </returns>
    int TryGetInteger(string key, int default_value);

    /// <summary>
    /// Gets the string value associated with the key
    /// <paramref name="key"/> and try to convert it to its 64-bit signed
    /// integer equivalent.
    /// </summary>
    /// <param name="key">
    /// The key that is associated with the value to get.
    /// </param>
    /// <param name="default_value">
    /// A 64-bit integer that will be returned when the key
    /// <paramref name="key"/> is not found.
    /// </param>
    /// <returns>
    /// The value associated with the key <paramref name="key"/> or
    /// <paramref name="default_value"/> if the key was not found.
    /// </returns>
    long TryGetLong(string key, long default_value);

    /// <summary>
    /// Copies the elements of the <see cref="IProviderOptions"/> to a
    /// <see cref="IDictionary{TKey,TValue}"/> instance.
    /// </summary>
    /// <returns>
    /// A <see cref="IDictionary{TKey,TValue}"/> containing copies of the
    /// options of the <see cref="IProviderOptions"/>.
    /// </returns>
    IDictionary<string, string> ToDictionary();

    string this[string key] { get; set; }
  }
}
