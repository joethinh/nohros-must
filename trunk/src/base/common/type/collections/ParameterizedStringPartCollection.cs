using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Nohros.Resources;

namespace Nohros.Collections
{
  /// <summary>
  /// Represents a collections of parameters associated with a <see cref="ParameterizedString"/>. The elements added
  /// to this collection must be a parameter, literal value are not allowed.
  /// </summary>
  public class ParameterizedStringPartCollection: ICollection<ParameterizedStringPart>
  {
    List<ParameterizedStringPart> parameters_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the ParameterizedStringPartCollection.
    /// </summary>
    internal ParameterizedStringPartCollection() {
      parameters_ = new List<ParameterizedStringPart>();
    }
    #endregion

    /// <summary>
    /// Gets the location of the specified <see cref="ParameterizedStringPart"/> with the specified name.
    /// </summary>
    /// <param name="parameter_name">The case-insensitive name of the <see cref="ParameterizedStringPart"/> to find.</param>
    /// <returns>The zero-based location of the specified <see cref="ParameterizedStringPart"/> with the specified
    /// case-insensitive name. Returns -1 when the object does not exists in the collection.</returns>
    public int IndexOf(string parameter_name) {
      if (parameter_name == null)
        return -1;

      for (int i = 0, j = parameters_.Count; i < j; i++) {
        if (string.Compare(parameters_[i].ParameterName, parameter_name, true) == 0) {
          return i;
        }
      }
      return -1;
    }

    #region ICollection<T>
    /// <summary>
    /// Adds the specified <see cref="ParameterizedStringPart"/> object to the <see cref="ParameterizedStringPartCollection"/>.
    /// </summary>
    /// <param name="part">The <see cref="ParameterizedStringPart"/> to add to the collection.</param>
    /// <exception cref="ArgumentException">The <see cref="ParameterizedStringPart"/> specified is already added to
    /// this ParameterizedStringPartCollection.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="part"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="part"/> is a literal value.</exception>
    public void Add(ParameterizedStringPart part) {
      if (part == null)
        throw new ArgumentNullException("part");

      if (part.ParameterName.Length == 0)
        throw new ArgumentOutOfRangeException("part");

      if (Contains(part))
        throw new ArgumentException(string.Format(StringResources.Collection_elm_AddingDuplicate, part.ParameterName));

      parameters_.Add(part);
    }

    /// <summary>
    /// Adds a new <see cref="ParameterizedStringPart"/> object to the collection by using the specified literal text.
    /// </summary>
    /// <param name="literal_text">A string that contains the parameterized string part.</param>
    /// <exception cref="ArgumentNullException"><paramref name="literal_text"/> is null</exception>
    /// <remarks>Thie method overload constructs a new <see cref="ParameterizedStringPart"/> using
    /// the specified parameters and add it to the collection.</remarks>
    /// <seealso cref="ParameterizedStringPart(string, string)"/>
    public void Add(string literal_text) {
      Add(new ParameterizedStringPart(literal_text));
    }

    /// <summary>
    /// Adds a new <see cref="ParameterizedStringPart"/> object to the by using the specified parameter name
    /// and value.
    /// </summary>
    /// <param name="parameter_name">The name of the parameter.</param>
    /// <param name="parameter_value">The value of the parameter. This could be null.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parameter_name"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="parameter_name"/>is a empty string.</exception>
    /// <remarks>Thie method overload constructs a new <see cref="ParameterizedStringPart"/> using
    /// the specified parameters and add it to the collection.</remarks>
    /// <seealso cref="ParameterizedStringPart(string, string)"/>
    public void Add(string parameter_name, string parameter_value) {
      Add(new ParameterizedStringPart(parameter_name, parameter_value));
    }

    /// <summary>
    /// Removes all the <see cref="ParameterizedStringPart"/> objects from the <see cref="ParameterizedStringPartCollection"/>.
    /// </summary>
    public void Clear() {
      parameters_.Clear();
    }

    /// <summary>
    /// Determines whether the specified <see cref="ParameterizedStringPart"/> is in the <see cref="ParameterizedStringPartCollection"/>.
    /// </summary>
    /// <param name="part">The <see cref="ParameterizedStringPart"/>value to verify.</param>
    /// <returns>true if the <see cref="ParameterizedStringPartCollection"/> contains the <paramref name="part"/>; otherwise false.</returns>
    public bool Contains(ParameterizedStringPart part) {
      return (part != null && -1 != IndexOf(part.ParameterName));
    }

    /// <summary>
    /// Copies all elements of the current <see cref="ParameterizedStringPartCollection"/> to the specified
    /// <see cref="ParameterizedStringPartCollection"/> array starting at the specified destination index.
    /// </summary>
    /// <param name="array">The <see cref="ParameterizedStringPartCollection"/> array that is the destination of
    /// the elements copied from the current <see cref="ParameterizedStringPartCollection"/></param>
    /// <param name="index">A 32-bit integer that represents the index in the <see cref="ParameterizedStringPartCollection"/>
    /// at which copying starts.</param>
    /// <exception cref="ArgumentNullException">array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.</exception>
    /// <exception cref="ArgumentException">The number of elements in the source <see cref="ParameterizedStringPartCollection"/>
    /// is greater than the available space from <paramref name="index"/> to the end of the destination array.</exception>
    public void CopyTo(ParameterizedStringPart[] array, int index) {
      parameters_.CopyTo(array, index);
    }

    /// <summary>
    /// Removes the specified <see cref="ParameterizedStringPart"/> from the collection.
    /// </summary>
    /// <param name="part">A <see cref="ParameterizedStringPart"/> object to remove from the colletion.</param>
    /// <returns>true if the parameter is successfully removed; othewise, false.</returns>
    /// <remarks>If part is not in the list, the Remove method will do nothing. In particular, it does not throws
    /// an exception.</remarks>
    public bool Remove(ParameterizedStringPart part) {
      if (part == null)
        return false;

      return Remove(part.ParameterName);
    }

    /// <summary>
    /// Removes a <see cref="ParameterizedStringPart"/> with name <paramref name="name"/> from the collection.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>true if the parameter is successfully removed; othewise, false.</returns>
    /// <remarks>If a paramtere with name <paramref name="name"/> could not be found, the Remove method will
    /// do nothing. In particular, it does not throws an exception, only returns false.</remarks>
    public bool Remove(string name) {
      int index = IndexOf(name);
      if (index != -1) {
        parameters_.RemoveAt(index);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns an integer that contains the number of elements in the <see cref="ParameterizedStringPartCollection"/>.
    /// </summary>
    public int Count {
      get { return parameters_.Count; }
    }

    /// <summary>
    /// Gets a value that indicates whether the <see cref="ParameterizedStringPartCollection"/> is read-only.
    /// </summary>
    /// <value>Returns true if the <see cref="ParameterizedStringPartCollection"/>is read only; otherwise, false.</value>
    bool ICollection<ParameterizedStringPart>.IsReadOnly {
      get { return false; }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="ParameterizedStringPartCollection"/>
    /// </summary>
    /// <returns>An IEnumerator of <see cref="ParameterizedStringPartCollection"/></returns>
    IEnumerator<ParameterizedStringPart> GetEnumerator() {
      for (int i = 0, j = parameters_.Count; i < j; i++) {
        yield return parameters_[i];
      }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="ParameterizedStringPartCollection"/>
    /// </summary>
    /// <returns>An IEnumerator of <see cref="ParameterizedStringPartCollection"/></returns>
    IEnumerator<ParameterizedStringPart> IEnumerable<ParameterizedStringPart>.GetEnumerator() {
      return GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="ParameterizedStringPartCollection"/>
    /// </summary>
    /// <returns>An IEnumerator of <see cref="ParameterizedStringPartCollection"/></returns>
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
    #endregion

    /// <summary>
    /// Gets the <see cref="ParameterizedStringPart"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the parameter to retrieve.</param>
    /// <returns>The <see cref="ParameterizedStringPart"/> at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">The specified index does not exists.</exception>
    public ParameterizedStringPart this[int index] {
      get { return parameters_[index]; }
    }

    /// <summary>
    /// Gets the <see cref="ParameterizedStringPart"/> with the specified name.
    /// </summary>
    /// <param name="parameter_name">The name of the parameter to retrieve.</param>
    /// <returns>The <see cref="ParameterizedStringPart"/> with the specified name.</returns>
    /// <exception cref="IndexOutOfRangeException"><paramref name="parameter_name"/> is not found or is invalid.</exception>
    /// <remarks>The <paramref name="parameter_name"/> is used to look up the index value in the
    /// underlying <see cref="ParameterizedStringPartCollection"/>. If the <paramref name="parameter_name"/>
    /// is not found or if it is not valid, an <see cref="IndexOutOfRangeException"/> will be throw.</remarks>
    public ParameterizedStringPart this[string parameter_name] {
      get {
        int index = IndexOf(parameter_name);
        if (index == -1)
          throw new IndexOutOfRangeException("parameter_name");
        return parameters_[index];
      }
    }
  }
}
