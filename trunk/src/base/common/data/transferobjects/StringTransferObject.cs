using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data.TransferObjects
{
  [Serializable]
  class StringTransferNode
  {
    public string column;
    public string value;
  }

  /// <summary>
  /// Used to transfer a collection of strings between application subsystems.
  /// Recommended for collections that typically contain 10 items or less.
  /// </summary>
  public class StringTransferObject: DataTransferObject
  {
    private const int _defaultCapacity = 4;
    private StringTransferNode[] _nodes;
    private int _size;
    private object _syncRoot;
    static StringTransferNode nil;

    #region .ctor
    static StringTransferObject() {
      nil = new StringTransferNode();
      nil.column = "nil";
      nil.value = null;
    }

    /// <summary>
    /// Creates an empty StringTransferObject.
    /// </summary>
    public StringTransferObject() {
      _syncRoot = new object();
      _nodes = new StringTransferNode[_defaultCapacity];
      _size = 0;
    }

    /// <summary>
    /// Creates an empty StringTransferObject using the specified
    /// columns names.
    /// </summary>
    /// <param name="names">the array of column names to add to the object</param>
    /// <remarks>
    /// This class is recommendend for collections that typically contain 10 columns or less.
    /// This should not be used if performance is important for large numbers of elements.
    /// <para>
    /// A column cannot be a null reference, but a value can.
    /// </para>
    /// </remarks>
    public StringTransferObject(params string[] names)
      : this(names, null) {
    }

    /// <summary>
    /// Creates a StringTransferObject object by using the specified
    /// column names and values.
    /// </summary>
    /// <param name="names">an array that contain the names of the columns</param>
    /// <param name="values">an array that contain the value of each column</param>
    /// <remarks>
    /// The <paramref name="values"/> array length must be equals or
    /// greater than the <paramref name="values"/> array length.
    /// <para>
    /// The values array can be null.
    /// </para>
    /// <para>
    /// This method assumes that the value at the position <c>i</c> of
    /// the <paramref name="names"/> array belongs to the defined by the value
    /// of the position <c>i</c> of the <paramref name="values"/> array.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">the names array or any element within
    /// it is a null reference</exception>
    /// <exception cref="ArgumentOutOfRangeException">the lengtg of the values array
    /// is less than the names array</exception>
    public StringTransferObject(string[] names, string[] values) {
      if (names == null)
        throw new ArgumentNullException("names cannot be null");
      if (values != null && values.Length < names.Length)
        throw new ArgumentOutOfRangeException("values", "values length cannot be less than names length");

      _syncRoot = new object();
      _size = 0;
      _nodes = new StringTransferNode[names.Length];

      if (values == null)
        values = new string[names.Length];

      // fill the _nodes with supplied the column names
      // and null values
      for (int i = 0, j = names.Length; i < j; i++) {
        StringTransferNode node = new StringTransferNode();

        string column = names[i];
        if (column == null)
          throw new ArgumentNullException("column name cannot be null");

        node.column = column;
        node.value = values[i];

        _nodes[_size++] = node;
      }
    }
    #endregion

    #region IDataTransferObject

    /// <summary>
    /// Gets a JSON-compliant string of characters representing the underlying class and
    /// formatted like a JSON array element.
    /// </summary>
    /// <returns>A string that can be used like an element of a JSON array.</returns>
    /// <remarks>
    /// The resulting string is a JSON array and can be used inside another JSON array to simulate a
    /// table data structure.
    /// </remarks>
    public override string AsJsonArray() {
      string elm = "[";
      string value;

      // StringBuilder.Append is faster than string concatenation
      // if the number of concatenations is less than 10. This
      // class is intend to be used with small number of columns,
      // so this is the reason to use string concatenations here.
      for (int i = 0; i < _size; i++) {
        StringTransferNode node = _nodes[i];
        value = value = (node == null) ? "nil" : node.value;
        elm += string.Concat("'", node.column, "':'", (value == null) ? "nil" : value, "',");
      }

      // close the brackets and remove the trailing comma
      elm = (elm.Length > 1) ? string.Concat(elm.Remove(elm.Length - 1, 1), "]") : string.Concat(elm, "]");

      return elm;
    }

    public override string AsJsonObject() {
      throw new NotImplementedException();
    }
    #endregion

    /// <summary>
    /// Gets the column ordinal, given the name of the column.
    /// </summary>
    /// <param name="column">the name of the column</param>
    /// <returns>the zero-based column ordinal</returns>
    /// <remarks>
    /// <see cref="GetOrdinal(String)"/> performs a case-insensitive lookup to find
    /// the column. The method throws an <see cref="IndexOutOfRangeException"/> exception if the
    /// zero-based column ordinal is not found.
    /// <para>
    /// <see cref="GetOrdianl(String)"/> is culture neutral.
    /// </para>
    /// <para>
    /// Because ordinal-based lookups ara more efficient than lookups, it is inneficient
    /// to call GetOrdinal within a loop. Save time by calling GetOrdinal once and
    /// assigning the results to an integer variable for use within the loop.
    /// </para>
    /// </remarks>
    /// <exception cref="IndexOutOfRangeException">the name specified is not a valid column name</exception>
    public int GetOrdinal(string column) {
      int index = IndexOf(column);
      if (index == -1)
        throw new IndexOutOfRangeException("the name specified is not a valid column name");
      return index;
    }

    /// <summary>
    /// Searches for the specified name and returns the zero-based index
    /// of the column.
    /// </summary>
    /// <param name="column">the name of the column</param>
    /// <returns>the zero-based column index, if found;otherwise, -1</returns>
    /// <remarks>
    /// This method performs a linear search; therefore, this method
    /// is an <c>O(n)</c> operation, where <c>n</c> is <see cref="Count"/>
    /// </remarks>
    private int IndexOf(string column) {
      StringTransferNode node;
      for (int i = 0, j = _nodes.Length; i < j; i++) {
        node = _nodes[i];
        if (string.Compare(node.column, column, StringComparison.InvariantCultureIgnoreCase) == 0) {
          return i;
        }
      }
      return -1;
    }
    /// <summary>
    /// Gets a value associated with the specified column name.
    /// </summary>
    /// <returns>The StringTransferNode object associated with
    /// the specified column name or null if the specified column name
    /// is not found</returns>
    private StringTransferNode Find(string column) {
      StringTransferNode node;
      for (int i = 0, j = _nodes.Length; i < j; i++) {
        node = _nodes[i];
        if (node != null && string.Compare(node.column, column, StringComparison.OrdinalIgnoreCase) == 0) {
          return node;
        }
      }
      return null;
    }

    /// <summary>
    /// Ensures that the capacity of this instance_ of <see cref="StringTransferObject"/> is
    /// at least the specified value.
    /// </summary>
    /// <param name="min">the minimun capacity to ensure</param>
    /// <remarks>If the current capacity is less than the capacity, memory for
    /// this instance_ is reallocated to hold at least capacity number of items;
    /// otherwise no memory is changed</remarks>
    private void EnsureCapacity(int min) {
      int length = _nodes.Length;
      if (length < min) {
        int num = (length == 0) ? 4 : (length * 2);
        if (num < min) {
          num = min;
        }
        Resize(num);
      }
    }

    /// <summary>
    /// Reallocate the internal array to accommodate the specified length.
    /// </summary>
    /// <param name="newLength">the new number of columns that this instance_
    /// can contain</param>
    private void Resize(int newLength) {
      int length = _nodes.Length;
      if (newLength != length) {
        // the value cannot be less than Count.
        if (length < _size)
          return;

        if (newLength > 0) {
          StringTransferNode[] destinationArray = new StringTransferNode[newLength];
          if (_size > 0) {
            Array.Copy(_nodes, 0, destinationArray, 0, _size);
          }
          _nodes = destinationArray;
        }
      }
    }

    /// <summary>
    /// Gets the number of columns actually contained int the <see cref="StringTransferObject"/>
    /// </summary>
    /// <remarks>
    /// Retrieving the value of this property is an O(1) operation.
    /// </remarks>
    public int Count {
      get { return _size; }
    }

    /// <summary>
    /// Gets or sets the value associated with the specified column.
    /// </summary>
    /// <param name="column">The column whose value to get or set</param>
    /// <returns>The value associated with the specified column. If the specified column
    /// is not found, attempting to get it returns null, and attempting to set it creates
    /// a new entry using the specified column.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="column"/> is null</exception>
    public string this[string column] {
      get {
        if (column == null)
          throw new ArgumentNullException("column", "Column cannot be null");

        StringTransferNode node = Find(column);
        return (node == null) ? null : node.value;
      }
      set {
        if (column == null)
          throw new ArgumentNullException("column", "Column cannot be null");

        StringTransferNode node = Find(column);
        if (node == null) {
          // resize the internal array if it is full
          if (_size == _nodes.Length)
            EnsureCapacity(_size + 1);

          node = new StringTransferNode();
          node.column = column;
          node.value = value;

          _nodes[_size++] = node;
        } else {
          node.value = value;
        }
      }
    }
  }
}
