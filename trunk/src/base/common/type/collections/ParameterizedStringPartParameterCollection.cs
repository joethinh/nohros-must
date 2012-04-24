using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nohros.Resources;

namespace Nohros.Collections
{
  /// <summary>
  /// Represents a collections of parameters associated with a
  /// <see cref="ParameterizedString"/>.
  /// </summary>
  /// <remarks>
  /// A <see cref="ParameterizedStringPartParameterCollection"/> class was designed to
  /// handle a small number of parameters and it uses classes that does not
  /// performs well when the number of elements is greater than 10 elements.
  /// </remarks>
  public class ParameterizedStringPartParameterCollection :
    ICollection<ParameterizedStringPartParameter>
  {
    readonly List<ParameterizedStringPartParameter> parameters_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the ParameterizedStringPartParameterCollection.
    /// </summary>
    internal ParameterizedStringPartParameterCollection() {
      parameters_ = new List<ParameterizedStringPartParameter>();
    }
    #endregion

    #region ICollection<ParameterizedStringPartParameter> Members
    /// <summary>
    /// Adds the specified <see cref="ParameterizedStringPartParameter"/>
    /// object to this <see cref="ParameterizedStringPartParameterCollection"/>.
    /// </summary>
    /// <param name="part">
    /// The <see cref="ParameterizedStringPartParameter"/> to add to the
    /// collection.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The <see cref="ParameterizedStringPart"/> specified is already added to
    /// this <see cref="ParameterizedStringPartParameterCollection"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="part"/> is <c>null</c>.
    /// </exception>
    public void Add(ParameterizedStringPartParameter part) {
      if (part == null) {
        throw new ArgumentNullException("part");
      }

      if (Contains(part))
        throw new ArgumentException(
          string.Format(
            StringResources.Collection_elm_AddingDuplicate, part.Name));

      parameters_.Add(part);
    }

    /// <summary>
    /// Removes all the <see cref="ParameterizedStringPart"/> objects from
    /// the <see cref="ParameterizedStringPartParameterCollection"/>.
    /// </summary>
    public void Clear() {
      parameters_.Clear();
    }

    /// <summary>
    /// Determines whether the specified <see cref="ParameterizedStringPart"/>
    /// is in the <see cref="ParameterizedStringPartParameterCollection"/>.
    /// </summary>
    /// <param name="part">
    /// The <see cref="ParameterizedStringPart"/>value to verify.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <see cref="ParameterizedStringPartParameterCollection"/>
    /// contains the <paramref name="part"/>; otherwise <c>false</c>.
    /// </returns>
    public bool Contains(ParameterizedStringPartParameter part) {
      return (part != null && -1 != IndexOf(part.Name));
    }

    /// <summary>
    /// Copies all elements of the current
    /// <see cref="ParameterizedStringPartParameterCollection"/> to the specified
    /// <see cref="ParameterizedStringPartParameterCollection"/> array starting at the
    /// specified destination index.
    /// </summary>
    /// <param name="array">
    /// The <see cref="ParameterizedStringPartParameterCollection"/> array that is the
    /// destination of the elements copied from the current
    /// <see cref="ParameterizedStringPartParameterCollection"/>
    /// </param>
    /// <param name="index">
    /// A 32-bit integer that represents the index in the
    /// <see cref="ParameterizedStringPartParameterCollection"/> at which copying
    /// starts.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> is  <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The number of elements in the source
    /// <see cref="ParameterizedStringPartParameterCollection"/> is greater than the
    /// available space from <paramref name="index"/> to the end of the
    /// destination array.
    /// </exception>
    public void CopyTo(ParameterizedStringPartParameter[] array, int index) {
      parameters_.CopyTo(array, index);
    }

    /// <summary>
    /// Removes the specified <see cref="ParameterizedStringPart"/> from the
    /// collection.
    /// </summary>
    /// <param name="part">
    /// A <see cref="ParameterizedStringPart"/> object to remove from the
    /// colletion.
    /// </param>
    /// <returns>
    /// <c>true</c> if the parameter is successfully removed; othewise,
    /// <c>false</c>.
    /// </returns>
    /// <remarks>
    /// If part is not in the list, the
    /// <see cref="Remove(ParameterizedStringPart)"/> method will do nothing.
    /// In particular, it does not throws an exception.
    /// </remarks>
    public bool Remove(ParameterizedStringPartParameter part) {
      if (part == null)
        return false;

      return Remove(part.Name);
    }

    /// <summary>
    /// Returns an integer that contains the number of elements in the
    /// <see cref="ParameterizedStringPartParameterCollection"/>.
    /// </summary>
    public int Count {
      get { return parameters_.Count; }
    }

    /// <summary>
    /// Gets a value that indicates whether the
    /// <see cref="ParameterizedStringPartParameterCollection"/> is read-only.
    /// </summary>
    /// <value>
    /// Returns true if the <see cref="ParameterizedStringPartParameterCollection"/>is
    /// read only; otherwise, false.
    /// </value>
    bool ICollection<ParameterizedStringPartParameter>.IsReadOnly {
      get { return false; }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the
    /// <see cref="ParameterizedStringPartParameterCollection"/>
    /// </summary>
    /// <returns>
    /// An IEnumerator of <see cref="ParameterizedStringPartParameterCollection"/>
    /// </returns>
    IEnumerator<ParameterizedStringPartParameter>
      IEnumerable<ParameterizedStringPartParameter>.GetEnumerator() {
      return GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the
    /// <see cref="ParameterizedStringPartParameterCollection"/>
    /// </summary>
    /// <returns>
    /// An IEnumerator of <see cref="ParameterizedStringPartParameterCollection"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
    #endregion

    /// <summary>
    /// Adds the specified <see cref="ParameterizedStringPartParameter"/>
    /// object to this <see cref="ParameterizedStringPartParameterCollection"/> if it
    /// is not added yet.
    /// </summary>
    /// <param name="part">
    /// The <see cref="ParameterizedStringPartParameter"/> to add to the
    /// collection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="part"/> is <c>null</c>.
    /// </exception>
    public void AddIfAbsent(ParameterizedStringPartParameter part) {
      if (part == null) {
        throw new ArgumentNullException("part");
      }

      if (!Contains(part)) {
        parameters_.Add(part);
      }
    }

    /// <summary>
    /// Gets the location of the specified
    /// <see cref="ParameterizedStringPart"/> with the specified name.
    /// </summary>
    /// <param name="name">
    /// The case-insensitive name of the <see cref="ParameterizedStringPart"/>
    /// to find.
    /// </param>
    /// <returns>
    /// The zero-based location of the specified
    /// <see cref="ParameterizedStringPart"/> with the specified
    /// case-insensitive name. Returns -1 when the object does not exists in
    /// the collection.
    /// </returns>
    public int IndexOf(string name) {
      if (name == null)
        return -1;

      for (int i = 0, j = parameters_.Count; i < j; i++) {
        if (string.Compare(
          parameters_[i].Name, name,
          StringComparison.OrdinalIgnoreCase) == 0) {
          return i;
        }
      }
      return -1;
    }

    /// <summary>
    /// Removes a <see cref="ParameterizedStringPart"/> with name
    /// <paramref name="name"/> from the collection.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter.
    /// </param>
    /// <returns><c>true</c> if the parameter is successfully removed;
    /// othewise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// If a parameter with name <paramref name="name"/> could not be found,
    /// the <see cref="Remove(string)"/> method will
    /// do nothing. In particular, it does not throws an exception, only
    /// returns <c>false</c>.
    /// </remarks>
    public bool Remove(string name) {
      int index = IndexOf(name);
      if (index != -1) {
        parameters_.RemoveAt(index);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the
    /// <see cref="ParameterizedStringPartParameterCollection"/>
    /// </summary>
    /// <returns>
    /// An IEnumerator of <see cref="ParameterizedStringPartParameterCollection"/>.
    /// </returns>
    IEnumerator<ParameterizedStringPartParameter> GetEnumerator() {
      for (int i = 0, j = parameters_.Count; i < j; i++) {
        yield return parameters_[i];
      }
    }

    /// <summary>
    /// Gets the <see cref="ParameterizedStringPart"/> at the specified index.
    /// </summary>
    /// <param name="index">
    /// The zero-based index of the parameter to retrieve.
    /// </param>
    /// <returns>
    /// The <see cref="ParameterizedStringPart"/> at the specified index.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// The specified index does not exists.
    /// </exception>
    public ParameterizedStringPartParameter this[int index] {
      get { return parameters_[index]; }
    }

    /// <summary>
    /// Gets the <see cref="ParameterizedStringPart"/> with the specified name.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter to retrieve.
    /// </param>
    /// <returns>
    /// The <see cref="ParameterizedStringPart"/> with the specified name.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="name"/> is not found or is invalid.
    /// </exception>
    /// <remarks>
    /// The <paramref name="name"/> is used to look up the index
    /// value in the underlying
    /// <see cref="ParameterizedStringPartParameterCollection"/>. If the
    /// <paramref name="name"/> is not found or if it is not valid,
    /// an <see cref="IndexOutOfRangeException"/> will be throw.
    /// </remarks>
    public ParameterizedStringPartParameter this[string name] {
      get {
        int index = IndexOf(name);
        if (index == -1)
          throw new IndexOutOfRangeException("name");
        return parameters_[index];
      }
    }
  }
}
