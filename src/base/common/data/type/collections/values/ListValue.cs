using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    public class ListValue : Value
    {
        List<IValue> list_;

        #region .ctor
        public ListValue():base(ValueType.TYPE_LIST)
        {
            list_ = new List<IValue>();
        }

        ~ListValue()
        {
            Clear();
        }
        #endregion

        /// <summary>
        /// Clears the contents of this ListValue.
        /// </summary>
        public void Clear()
        {
            list_.Clear();
        }

        /// <summary>
        /// Sets the list item at the given index to be the Value specified by
        /// the value given. If the index beyond the current end of list, null
        /// Values will be used to pad out the list.
        /// </summary>
        /// <param name="index">The index within the list where the Value will be set.</param>
        /// <param name="in_value">The Value to set at the given index.</param>
        /// <returns>true if successful, or false if the index was negative or the value
        /// is a null reference.</returns>
        public bool Set(int index, Value in_value)
        {
            if (in_value == null)
                return false;

            if (index >= list_.Count) {
                // Pad out any intermediate indexes with null settings
                while (index > list_.Count)
                    Append(CreateNullValue());
                Append(in_value);
            } else {
                list_[index] = in_value;
            }
            return true;
        }

        /// <summary>
        /// Gets the <see cref="Value"/> object at the given index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <param name="out_value"></param>
        /// <returns>true if the index falls within the current list range; otherwise false.</returns>
        public bool Get(int index, out IValue out_value)
        {
            out_value = this[index];
            return (out_value != null);
        }

        /// <summary>
        /// Convenience form of Get().
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <param name="out_value">When this method returns, contains the <see cref="Value"/> associated
        /// with the specified index if the index falls within the current list range; otherwise null</param>
        /// <returns>true if the index falls within the current list range; otherwise false.</returns>
        public bool GetBoolean(int index, out bool out_value)
        {
            IValue value;

            out_value = default(bool);
            if (!Get(index, out value))
                return false;

            return value.GetAsBoolean(out out_value);
        }

        /// <summary>
        /// Convenience form of Get().
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <param name="out_value">When this method returns, contains the <see cref="Value"/> associated
        /// with the specified index if the index falls within the current list range; otherwise null</param>
        /// <returns>true if the index falls within the current list range; otherwise false.</returns>
        public bool GetInteger(int index, out int out_value)
        {
            IValue value;

            out_value = default(int);
            if (!Get(index, out value))
                return false;

            return value.GetAsInteger(out out_value);
        }

        /// <summary>
        /// Convenience form of Get().
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <param name="out_value">When this method returns, contains the <see cref="Value"/> associated
        /// with the specified index if the index falls within the current list range; otherwise null</param>
        /// <returns>true if the index falls within the current list range; otherwise false.</returns>
        public bool GetReal(int index, out double out_value)
        {
            IValue value;

            out_value = default(double);
            if (!Get(index, out value))
                return false;

            return value.GetAsReal(out out_value);
        }

        /// <summary>
        /// Convenience form of Get().
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <param name="out_value">When this method returns, contains the <see cref="Value"/> associated
        /// with the specified index if the index falls within the current list range; otherwise null</param>
        /// <returns>true if the index falls within the current list range; otherwise false.</returns>
        public bool GetString(int index, out string out_value)
        {
            IValue value;

            out_value = null;
            if (!Get(index, out value))
                return false;

            return value.GetAsString(out out_value);
        }

        /// <summary>
        /// Convenience form of Get().
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <param name="out_value">When this method returns, contains the <see cref="Value"/> associated
        /// with the specified index if the index falls within the current list range; otherwise null</param>
        /// <returns>true if the index falls within the current list range; otherwise false.</returns>
        public bool GetDictionary(int index, out DictionaryValue out_value)
        {
            IValue value;

            out_value = null;
            if (!Get(index, out value) || value.IsType(ValueType.TYPE_DICTIONARY))
                return false;

            out_value = value as DictionaryValue;
            return true;
        }

        /// <summary>
        /// Convenience form of Get().
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <param name="out_value">When this method returns, contains the <see cref="Value"/> associated
        /// with the specified index if the index falls within the current list range; otherwise null</param>
        /// <returns>true if the index falls within the current list range; otherwise false.</returns>
        public bool GetList(int index, out ListValue out_value)
        {
            IValue value;

            out_value = null;
            if (!Get(index, out value) || value.IsType(ValueType.TYPE_LIST))
                return false;

            out_value = value as ListValue;
            return true;
        }

        /// <summary>
        /// Removes the <see cref="Value"/> with the specified index from this list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <param name="value">When this method returns, contains the removed value if the <paramref name="index"/>
        /// is valid; otherwise, null.</param>
        /// <returns>true if <paramref name="index"/> is valid; otherwise false.</returns>
        public bool Remove(int index, out IValue out_value)
        {
            out_value = null;
            if (index < 0 || index >= list_.Count)
                return false;

            out_value = list_[index];
            list_.RemoveAt(index);

            return true;
        }

        /// <summary>
        /// Removes the first instance_ of <paramref name="value"/> found in the list, if any.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The zero-based index of the first occurrence of <paramref name="value"/> within the entire list,
        /// if found; otherwise -1.</returns>
        public int Remove(IValue value)
        {
            int index = list_.IndexOf(value);
            if (index >= 0)
                list_.RemoveAt(index);
            return index;
        }

        /// <summary>
        /// Appends a <see cref="Value"/> to the end of the list.
        /// </summary>
        public void Append(IValue in_value)
        {
            list_.Add(in_value);
        }

        /// <summary>
        /// Insert a <see cref="Value"/> at index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="in_value">The <see cref="Value"/> to insert.</param>
        /// <returns>true if successful, or false if the index was out of range.</returns>
        public bool Insert(int index, IValue in_value)
        {
            if (index < 0 || index > list_.Count)
                return false;

            list_.Insert(index, in_value);

            return true;
        }

        public override IValue DeepCopy()
        {
            ListValue result = new ListValue();
            foreach(IValue value in list_) {
                result.Append(value.DeepCopy());
            }
            return result;
        }

        public override bool Equals(IValue other)
        {
            if (other.Type != Type)
                return false;

            int count;
            ListValue other_list = other as ListValue;

            if ((count = Size) != other_list.Size)
                return false;

            for (int i = 0; i < count; i++) {
                if (!(list_[i].Equals(other_list.list_[i])))
                    return false;
            }

            return true;
        }

        public IValue this[int index] {
            get
            {
                if (index >= list_.Count || index < 0)
                    return null;
                return list_[index];
            }
        }

        /// <summary>
        /// Get the number of elements actually contained in this List.
        /// </summary>
        public int Size
        {
            get { return list_.Count; }
        }
    }
}
