using System;
using System.Data;

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
    /// using the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The underlying value.
    /// </param>
    /// <param name="name">
    /// The name of the field.
    /// </param>
    /// <param name="position">
    /// The zero based ordinal position of the field within an
    /// <see cref="IDataReader"/>.
    /// </param>
    protected DataField(string name, int position) {
      this.name = name;
      this.position = position;
    }
    #endregion

    #region IDataField<T> Members
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
    #endregion
  }
}
