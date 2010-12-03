using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    public interface IValue
    {
        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a boolean type, the value is returned
        /// through the <paramref="out_value"> parameter; otherwise a default bool value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a boolean type;
        /// otherwise, false.</returns>
        bool GetAsBoolean(out bool out_value);

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a integer type, the value is returned
        /// through the <paramref="out_value"> parameter; otherwise a default int value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a integer type;
        /// otherwise, false.</returns>
        bool GetAsInteger(out int out_value);

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a double type, the value is returned
        /// through the <paramref="out_value">parameter; otherwise a default double value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a double type;
        /// otherwise, false.</returns>
        bool GetAsReal(out double out_value);

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a string type, the value is returned
        /// through the <paramref="out_value">parameter; otherwise a null is returned
        /// through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a string type;
        /// otherwise, false.</returns>
        bool GetAsString(out string out_value);

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a boolean type, the value is returned
        /// through the <paramref="out_value"> parameter; otherwise a default bool value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a boolean type;
        /// otherwise, false.</returns>
        bool GetAsBoolean();

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a integer type, the value is returned
        /// through the <paramref="out_value"> parameter; otherwise a default int value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a integer type;
        /// otherwise, false.</returns>
        int GetAsInteger();

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a double type, the value is returned
        /// through the <paramref="out_value">parameter; otherwise a default double value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a double type;
        /// otherwise, false.</returns>
        double GetAsReal();

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a string type, the value is returned
        /// through the <paramref="out_value">parameter; otherwise a null is returned
        /// through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a string type;
        /// otherwise, false.</returns>
        string GetAsString();

        /// <summary>
        /// Creates a deep copy of the entire Value tree.
        /// </summary>
        /// <returns>A deep copy of the entire value tree.</returns>
        /// <remarks>This method should only be getting called for null Values-- all
        /// subclasses need to provide their own implementation.</remarks>
        IValue DeepCopy();

        /// <summary>
        /// Compares if two Value objects have equal contents.
        /// </summary>
        /// <returns>true if this instance_ have equals contents of other.</returns>
        /// <remarks>This method should only be getting called for null values-- all
        /// subclasses need to provide their own implementation.</remarks>
        bool Equals(IValue other);

        /// <summary>
        /// Gets a value indicating whether the current object represents a given type or not.
        /// </summary>
        /// <returns>true if the current object represents a given type</returns>
        bool IsType(Nohros.Data.ValueType type);

        /// <summary>
        /// Gets the type of the value stored by the current Value object.
        /// Each type will be implemented by only one subclass of Value, so it's
        /// safe to use the ValueType to determine whether you can cast from
        /// Value to (Implementating Class)[*]. Also, A Value object never changes
        /// its type after construction.
        /// </summary>
        Nohros.Data.ValueType ValueType { get; }
    }
}
