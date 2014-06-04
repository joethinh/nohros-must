
namespace Nohros
{
  /// <summary>
  /// Speficies a recursive data storage intended to storing settings and
  /// other persistentable data. The concet of <see cref="IValue"/> was ported
  /// from the Google Chromium library.
  /// </summary>
  public interface IValue
  {
    /// <summary>
    /// This method allow the convenient retrieval of configurations. If the
    /// current configuration object can be converted into a boolean type, the
    /// value is returned through the <paramref="out_value"> parameter;
    /// otherwise a default bool value is returned through the
    /// <paramref="out_value"> variable.
    /// </summary>
    /// <returns><c>true</c> if the current configuration object can be
    /// converted into a boolean type; otherwise, <c>false</c>.</returns>
    bool GetAsBoolean(out bool out_value);

    /// <summary>
    /// This method allow the convenient retrieval of configurations. If the
    /// current configuration object can be converted into a integer type, the
    /// value is returned through the <paramref="out_value"> parameter;
    /// otherwise a default <c>int</c> value is returned through the
    /// <paramref="out_value"> variable.
    /// </summary>
    /// <returns><c>true</c> if the current configuration object can be
    /// converted into a integer type; otherwise, false.</returns>
    bool GetAsInteger(out int out_value);

    /// <summary>
    /// This method allow the convenient retrieval of configurations. If the
    /// current configuration object can be converted into a <c>double</c> type,
    /// the value is returned through the <paramref="out_value"> parameter;
    /// otherwise a default <c>double</c> value is returned through the
    /// <paramref="out_value"> variable.
    /// </summary>
    /// <returns><c>true</c> if the current configuration object can be
    /// converted into a <c>double</c> type; otherwise, false.</returns>
    bool GetAsReal(out double out_value);

    /// <summary>
    /// This method allow the convenient retrieval of configurations. If the
    /// current configuration object can be converted into a <c>string</c> type,
    /// the value is returned through the <paramref="out_value">parameter;
    /// otherwise a <c>null</c> is returned through the <paramref="out_value">
    /// variable.
    /// </summary>
    /// <returns><c>true</c> if the current configuration object can be
    /// converted into a <c>string</c> type; otherwise, <c>false</c>.</returns>
    bool GetAsString(out string out_value);

    /// <summary>
    /// This method allow the convenient retrieval of configurations. If the
    /// current configuration object can be converted into a boolean type, the
    /// value is returned as a boolean; otherwise this method throws an
    /// <see cref=" InvalidCastException"/>.
    /// </summary>
    /// <returns>The current <see cref="Value"/> instance as a boolean.
    /// </returns>
    bool GetAsBoolean();

    /// <summary>
    /// This method allow the convenient retrieval of configurations. If the
    /// current configuration object can be converted into a integer type, the
    /// value is returned as a integer; otherwise this method throws an
    /// <see cref=" InvalidCastException"/>.
    /// </summary>
    /// <returns>The current <see cref="Value"/> instance as a integer.
    /// </returns>
    int GetAsInteger();

    /// <summary>
    /// This method allow the convenient retrieval of configurations. If the
    /// current configuration object can be converted into a real/decimal type,
    /// the value is returned as a real/decimal; otherwise this method throws
    /// an <see cref=" InvalidCastException"/>.
    /// </summary>
    /// <returns>The current <see cref="Value"/> instance as a real.
    /// </returns>
    double GetAsReal();

    /// <summary>
    /// This method allow the convenient retrieval of configurations. If the
    /// current configuration object can be converted into a string type, the
    /// value is returned as a string; otherwise this method throws an
    /// <see cref=" InvalidCastException"/>.
    /// </summary>
    /// <returns>The current <see cref="Value"/> instance as a string.
    /// </returns>
    string GetAsString();

    /// <summary>
    /// Creates a deep copy of the entire Value tree.
    /// </summary>
    /// <returns>A deep copy of the entire value tree.</returns>
    /// <remarks>This method should only be getting called for null Values; all
    /// subclasses need to provide their own implementation.</remarks>
    IValue DeepCopy();

    /// <summary>
    /// Compares if two Value objects have equal contents.
    /// </summary>
    /// <returns>true if this instance_ have equals contents of other.</returns>
    /// <remarks>This method should only be getting called for null values; all
    /// subclasses need to provide their own implementation.</remarks>
    bool Equals(IValue other);

    /// <summary>
    /// Gets a value indicating whether the current object represents a given
    /// type or not.
    /// </summary>
    /// <returns><c>true</c> if the current object represents a given type.</returns>
    bool IsType(ValueType type);

    /// <summary>
    /// Gets the type of the value stored by the current Value object.
    /// Each type will be implemented by only one subclass of Value, so it's
    /// safe to use the ValueType to determine whether you can cast from
    /// Value to (Implementating Class)[*]. Also, A Value object never changes
    /// its type after construction.
    /// </summary>
    ValueType ValueType { get; }
  }
}