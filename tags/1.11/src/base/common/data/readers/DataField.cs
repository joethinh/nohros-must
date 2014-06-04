using System;
using System.Data;
using Nohros.Resources;

namespace Nohros.Data
{
  /// <summary>
  /// Provides a skeletal implementation of the <see cref="IDataField{T}"/>
  /// interface to minimize the effort required to implement that interface.
  /// </summary>
  /// <typeparam name="T">
  /// The type of the underlying value.
  /// </typeparam>
  public abstract class DataField<T> : IDataField<T>
  {
    /// <summary>
    /// The field's name.
    /// </summary>
    protected readonly string name;

    /// <summary>
    /// The zero-based ordinal column of the field.
    /// </summary>
    protected readonly int position;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="DataField{T}"/> class by
    /// using the specified field <paramref name="name"/> and
    /// ordinal <paramref name="position"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the field.
    /// </param>
    /// <param name="position">
    /// The zero based ordinal position of the field within an
    /// <see cref="IDataReader"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="position"/> is negative.
    /// </exception>
    protected DataField(string name, int position) {
      if (name == null) {
        throw new ArgumentNullException("name");
      }

      if (position < 0) {
        throw new IndexOutOfRangeException(
          string.Format(StringResources.ArgumentOutOfRange_NeedNonNegNum));
      }

      this.name = name;
      this.position = position;
    }
    #endregion

    /// <inheritdoc/>
    public int Position {
      get { return position; }
    }

    /// <inheritdoc/>
    public string Name {
      get { return name; }
    }

    /// <inheritdoc/>
    public abstract T GetValue(IDataReader reader);
  }
}
