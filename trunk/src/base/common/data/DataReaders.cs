using System;
using System.Data;

using Nohros.Resources;

namespace Nohros.Data
{
  /// <summary>
  /// Utility methods for <see cref="IDataReader"/>.
  /// </summary>
  public sealed class DataReaders
  {
    /// <summary>
    /// Return the indexes of the named fields.
    /// </summary>
    /// <param name="data_reader">
    /// A <see cref="IDataReader"/> containing the fileds to get the ordinals.
    /// </param>
    /// <param name="column_names">
    /// A string array containing the names of the fields to find.
    /// </param>
    /// <returns>
    /// An array containing the indexes of the specified column
    /// names within the <paramref name="IDataReader"/>. The indexes is
    /// returned in the same order they appear on column names array.
    /// </returns>
    /// <remarks>
    /// Ordinal-based lookups are more efficient than name lookups, it
    /// is inefficient to call <see cref="IDataReader.GetOrdinal"/> within a
    /// loop. This method provides a convenient way to call the
    /// <see cref="IDataReader.GetOrdinal"/> method for a set of columns
    /// defined within a <paramref name="IDataReader"/> and reduce the
    /// code len used for that purpose.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The number of specified fields is less than the number of columns.
    /// </exception>
    public static int[] GetOrdinals(IDataReader data_reader,
      params string[] column_names) {
      int[] ordinals = new int[column_names.Length];

      int j = column_names.Length;
      if (data_reader.FieldCount < j) {
        throw new ArgumentOutOfRangeException(
          StringResources.ArgumentOutOfRange_ArrayLengthMismatch);
      }

      for (int i = 0; i < j; i++) {
        ordinals[i] = data_reader.GetOrdinal(column_names[i]);
      }

      return ordinals;
    }
  }
}
