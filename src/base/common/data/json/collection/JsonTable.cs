using System;
using System.Collections.Generic;

using Nohros.Resources;

namespace Nohros.Data.Json
{
  /// <summary>
  /// Used to transfer a collection of strings between application subsystems.
  /// Recommended for collections that typically contains 10 columns or less.
  /// </summary>
  /// <remarks>
  /// This class is recommendend for collections that contains 10 columns or
  /// less and should not be used for large numbers of columns if performance
  /// is important factor.
  /// </remarks>
  public class JsonTable : IJsonToken, IJsonCollection
  {
    const int kDefaultCapacity = 4;

    readonly string[] columns_;
    readonly List<JsonArray> rows_;
    int size_;

    #region .ctor
    /// <summary>
    /// Creates a new instance of the <see cref="JsonTable"/> class
    /// that has no values and contains the specified columns.
    /// </summary>
    /// <param name="columns">
    /// An array of strings containing the name of the columns.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="columns"/> is a <c>null</c> reference
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="columns"/> or contains a elements that is <c>null</c>.
    /// </exception>
    public JsonTable(string[] columns) {
      if (columns == null) {
        throw new ArgumentNullException("columns");
      }

      // sanity check the column names for null.
      for (int i = 0, j = columns.Length; i < j; i++) {
        if (columns[i] == null) {
          throw new ArgumentOutOfRangeException("columns");
        }
      }
      size_ = 0;
      columns_ = columns;
      rows_ = new List<JsonArray>();
    }
    #endregion

    #region IJsonCollection Members
    /// <inheritdoc/>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="token"/> is not an instance of the class
    /// <see cref="JsonArray"/>.
    /// </exception>
    void IJsonCollection.Add(IJsonToken token) {
      JsonArray array = token as JsonArray;
      if (array == null) {
        throw new ArgumentOutOfRangeException(string.Format(
          StringResources.Arg_WrongType, "token", "JsonArray"));
      }
      Add(array);
    }

    /// <inheritdoc/>
    public int Count {
      get { return rows_.Count; }
    }
    #endregion

    #region IJsonToken Members
    /// <summary>
    /// Gets a string of characters representing the underlying
    /// class and formatted like a json array.
    /// </summary>
    /// <returns>
    /// A string representation of the <see cref="JsonTable"/> class
    /// that represents a json array.
    /// </returns>
    public string AsJson() {
      int length = columns_.Length;
      if (length == 0) {
        return "{}";
      }

      #region Member names
      const string kColumnNamesMemberName = "columns";
      const string kDataMemberName = "data";
      #endregion

      JsonStringBuilder builder = new JsonStringBuilder()
        .WriteBeginObject()
        .WriteMemberName(kColumnNamesMemberName)
        .WriteStringArray(columns_)
        .WriteMemberName(kDataMemberName);
      for (int i = 0, j = rows_.Count; i < j; i++) {
        builder.WriteUnquotedString(rows_[i].AsJson());
      }
      return builder.WriteEndObject().ToString();
    }
    #endregion

    /// <summary>
    /// Adds an <see cref="IJsonToken"/> to the
    /// <seealso cref="IJsonCollection"/>.
    /// </summary>
    /// <param name="array">
    /// The <seealso cref="IJsonToken"/> object to be added to the
    /// <see cref="IJsonToken"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The length of the array <paramref name="array"/> is not equals to
    /// the number of columns.
    /// </exception>
    public void Add(JsonArray array) {
      if (array == null) {
        throw new ArgumentNullException("array");
      }

      if (array.Value.Length != columns_.Length) {
        throw new ArgumentException(string.Format(
          StringResources.Arg_ArrayLengthDifferFrom, "array.Value", "columns"));
      }
      rows_.Add(array);
    }
  }
}
