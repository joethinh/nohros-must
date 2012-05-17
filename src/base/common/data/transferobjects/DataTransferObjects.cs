using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
  public class DataTransferObjectSet<T>: ICollection<T> where T: IDataTransferObject
  {
    private List<T> _dtos;
    private int _sizeOf = 1;

    #region .ctor
    public DataTransferObjectSet() {
      _dtos = new List<T>();
    }
    public DataTransferObjectSet(int capacity) {
      _dtos = new List<T>(capacity);
    }
    #endregion

    public void Add(T item) {
      _dtos.Add(item);
    }

    public void Clear() {
      _dtos.Clear();
    }
    public bool Contains(T item) {
      return _dtos.Contains(item);
    }

    #region public void CopyTo(...) overloads
    public void CopyTo(T[] array) {
      _dtos.CopyTo(array);
    }
    public void CopyTo(T[] array, int arrayIndex) {
      _dtos.CopyTo(array, arrayIndex);
    }
    public void CopyTo(int index, T[] array, int arrayIndex, int count) {
      _dtos.CopyTo(index, array, arrayIndex, count);
    }
    #endregion

    #region public IEnumerator<T> GetEnumerator(...) overloads
    public IEnumerator<T> GetEnumerator() {
      return _dtos.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return _dtos.GetEnumerator();
    }
    #endregion

    public bool Remove(T item) {
      return _dtos.Remove(item);
    }
    public void RemoveAt(int index) {
      _dtos.RemoveAt(index);
    }
    public void RemoveRange(int index, int count) {
      _dtos.RemoveRange(index, count);
    }

    public T this[int index] {
      get { return _dtos[index]; }
    }

    public int Count {
      get { return _dtos.Count; }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    /// <summary>
    /// Gets or set the size of the type T when it's serialized
    /// </summary>
    public int SizeOf {
      get { return _sizeOf; }
      set { _sizeOf = value; }
    }

    public string AsJsonArray() {
      int j = j = _dtos.Count;

      StringBuilder sb = new StringBuilder(_sizeOf * j);

      sb.Append("[");
      for (int i = 0; i < j; i++)
        sb.Append(_dtos[i].AsJsonObject()).Append(",");

      // remove the last comma and closes the json array
      return sb.Remove(sb.Length - 1, 1).Append("]").ToString();
    }

    public override string ToString() {
      return AsJsonArray();
    }
  }
}