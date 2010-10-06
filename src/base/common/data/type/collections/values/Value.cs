using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    public class Value : IValue
    {
        ValueType type_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        Value() { }

        /// <summary>
        /// This isn't safe for end-users (they should use the Create*Value()
        /// static method above), but it's useful for subclasses.
        /// </summary>
        /// <param name="type">The type of the Value</param>
        protected Value(ValueType type)
        {
            type_ = type;
        }
        #endregion

        /// <summary>
        /// Convenience method for creating Value of type TYPE_NULL without thinking
        /// about which class implements it.
        /// </summary>
        /// <returns>A Value object which ValueType is equals to TYPE_NULL</returns>
        public static Value CreateNullValue() {
            return new Value(ValueType.TYPE_NULL);
        }

        /// <summary>
        /// Convenience method for creating Value of type TYPE_BOLEAN without thinking
        /// about which class implements it.
        /// </summary>
        /// <param name="in_value">The underlying bool value</param>
        /// <returns>A Value object which ValueType is equals to TYPE_BOLEAN</returns>
        public static Value CreateBooleanValue(bool in_value) {
            return new FundamentalValue(in_value);
        }

        /// <summary>
        /// Convenience method for creating Value of type TYPE_INTEGER without thinking
        /// about which class implements it.
        /// </summary>
        /// <param name="in_value">The underlying integer value</param>
        /// <returns>A Value object which ValueType is equals to TYPE_INTEGER</returns>
        public static Value CreateIntegerValue(int in_value) {
            return new FundamentalValue(in_value);
        }
        /// <summary>
        /// Convenience method for creating Value of type TYPE_REAL without thinking
        /// about which class implements it.
        /// </summary>
        /// <param name="in_value">The underlying double value</param>
        /// <returns>A Value object which ValueType is equals to TYPE_REAL</returns>
        public static Value CreateRealValue(double in_value) {
            return new FundamentalValue(in_value);
        }

        /// <summary>
        /// Convenience method for creating Value of type TYPE_STRING without thinking
        /// about which class implements it.
        /// </summary>
        /// <param name="in_value">The underlying string value</param>
        /// <returns>A Value object which ValueType is equals to TYPE_STRING</returns>
        public static Value CreateStringValue(string in_value) {
            return new StringValue(in_value);
        }

        /// <summary>
        /// Convenience method for creating Valut of type TYPE_GENERIC without thinking
        /// about which class implements it.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="in_value"/> parameter</typeparam>
        /// <param name="in_value">The underlying <typeparamref name="T"/>value<</param>
        /// <returns></returns>
        public static Value CreateGenericValue<T>(T in_value) where T : class {
            return new GenericValue<T>(in_value);
        }

        #region bool GetAs[...](out ...) methods
        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a boolean type, the value is returned
        /// through the <paramref="out_value"> parameter; otherwise a default bool value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a boolean type;
        /// otherwise, false.</returns>
        public virtual bool GetAsBoolean(out bool out_value) {
            out_value = default(bool);
            return false;
        }

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a integer type, the value is returned
        /// through the <paramref="out_value"> parameter; otherwise a default int value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a integer type;
        /// otherwise, false.</returns>
        public virtual bool GetAsInteger(out int out_value) {
            out_value = default(int);
            return false;
        }

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a double type, the value is returned
        /// through the <paramref="out_value">parameter; otherwise a default double value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a double type;
        /// otherwise, false.</returns>
        public virtual bool GetAsReal(out double out_value) {
            out_value = default(double);
            return false;
        }

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a string type, the value is returned
        /// through the <paramref="out_value">parameter; otherwise a null is returned
        /// through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a string type;
        /// otherwise, false.</returns>
        public virtual bool GetAsString(out string out_value) {
            out_value = default(string);
            return false;
        }
        #endregion

        #region [ValueType] GetAs[...]() methods
        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a boolean type, the value is returned
        /// through the <paramref="out_value"> parameter; otherwise a default bool value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a boolean type;
        /// otherwise, false.</returns>
        public virtual bool GetAsBoolean()
        {
            return default(bool);
        }

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a integer type, the value is returned
        /// through the <paramref="out_value"> parameter; otherwise a default int value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a integer type;
        /// otherwise, false.</returns>
        public virtual int GetAsInteger()
        {
            return default(int);
        }

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a double type, the value is returned
        /// through the <paramref="out_value">parameter; otherwise a default double value
        /// is returned through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a double type;
        /// otherwise, false.</returns>
        public virtual double GetAsReal()
        {
            return default(double);
        }

        /// <summary>
        /// This method allow the convenient retrieval of settings. If the current
        /// setting object can be converted into a string type, the value is returned
        /// through the <paramref="out_value">parameter; otherwise a null is returned
        /// through the <paramref="out_value">.
        /// </summary>
        /// <returns>true if the current setting object can be converted into a string type;
        /// otherwise, false.</returns>
        public virtual string GetAsString()
        {
            return default(string);
        }
        #endregion

        /// <summary>
        /// Creates a deep copy of the entire Value tree.
        /// </summary>
        /// <returns>A deep copy of the entire value tree.</returns>
        /// <remarks>This method should only be getting called for null Values-- all
        /// subclasses need to provide their own implementation.</remarks>
        public virtual IValue DeepCopy() {
            if (IsType(ValueType.TYPE_NULL))
                return CreateNullValue();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compares if two Value objects have equal contents.
        /// </summary>
        /// <returns>true if this instance_ have equals contents of other.</returns>
        /// <remarks>This method should only be getting called for null values-- all
        /// subclasses need to provide their own implementation.</remarks>
        public virtual bool Equals(IValue other) {
            if (IsType(ValueType.TYPE_NULL))
                return other.IsType(ValueType.TYPE_NULL);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether the current object represents a given type or not.
        /// </summary>
        /// <returns>true if the current object represents a given type</returns>
        public bool IsType(ValueType type) {
            return type_ == type;
        }

        /// <summary>
        /// Gets the type of the value stored by the current Value object.
        /// Each type will be implemented by only one subclass of Value, so it's
        /// safe to use the ValueType to determine whether you can cast from
        /// Value to (Implementating Class)[*]. Also, A Value object never changes
        /// its type after construction.
        /// </summary>
        public ValueType ValueType {
            get { return type_; }
        }
    }
}
