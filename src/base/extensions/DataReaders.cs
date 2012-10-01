using System;
using System.Data;
using Nohros.Data;
using Nohros.Resources;

namespace Nohros.Extensions
{
  public static class DataReaders
  {
    /// <summary>
    /// Gets the columns indexes(ordinals), given the name of the columns.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="column_names">
    /// An array containing the column names to get the ordinals.
    /// </param>
    /// <returns>
    /// An array containing the indexes of the specified column
    /// names within the <paramref name="reader"/>. The indexes is
    /// returned in the same order they appear on column names array.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The number of field of the <paramref name="reader"/> is less than
    /// the number of columns specified in <paramref name="columns"/>.
    /// </exception>
    /// <remarks>
    /// Ordinal-based lookups are more efficient than name lookups, it
    /// is inefficient to call <see cref="IDataReader.GetOrdinal"/> within a
    /// loop. This method provides a convenient way to call the
    /// <see cref="IDataReader.GetOrdinal"/> method for a set of columns
    /// defined within a <paramref name="reader"/> and reduce the
    /// code len used for that purpose.
    /// </remarks>
    public static int[] GetOrdinals(this IDataReader reader,
      params string[] columns) {
      var ordinals = new int[columns.Length];
      int j = columns.Length;
      if (reader.FieldCount < j) {
        throw new ArgumentException(
          StringResources.Argument_ArrayLengthMismatch);
      }
      for (int i = 0; i < j; i++) {
        ordinals[i] = reader.GetOrdinal(columns[i]);
      }
      return ordinals;
    }

    public static bool GetBoolean(this IDataReader reader, string name) {
      return reader.GetBoolean(reader.GetOrdinal(name));
    }

    public static byte GetByte(this IDataReader reader, string name) {
      return reader.GetByte(reader.GetOrdinal(name));
    }

    public static char GetChar(this IDataReader reader, string name) {
      return reader.GetChar(reader.GetOrdinal(name));
    }

    public static DateTime GetDateTime(this IDataReader reader, string name) {
      return reader.GetDateTime(reader.GetOrdinal(name));
    }

    public static double GetDouble(this IDataReader reader, string name) {
      return reader.GetDouble(reader.GetOrdinal(name));
    }

    public static float GetFloat(this IDataReader reader, string name) {
      return reader.GetFloat(reader.GetOrdinal(name));
    }

    public static Guid GetGuid(this IDataReader reader, string name) {
      return reader.GetGuid(reader.GetOrdinal(name));
    }

    public static short GetInt16(this IDataReader reader, string name) {
      return reader.GetInt16(reader.GetOrdinal(name));
    }

    public static int GetInt32(this IDataReader reader, string name) {
      return reader.GetInt32(reader.GetOrdinal(name));
    }

    public static long GetInt64(this IDataReader reader, string name) {
      return reader.GetInt64(reader.GetOrdinal(name));
    }

    public static string GetString(this IDataReader reader, string name) {
      return reader.GetString(reader.GetOrdinal(name));
    }
  }
}
