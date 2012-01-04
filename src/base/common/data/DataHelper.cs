using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.IO;

using Nohros.Resources;
using Nohros.Data.TransferObjects;

namespace Nohros.Data
{
  /// <summary>
  /// Collection of helper methods used by data manipulation.
  /// </summary>
  public class DataHelper
  {
    #region SQL helpers

    /// <summary>
    /// Return the indexes of the named fields.
    /// </summary>
    /// <param name="dr">A <see cref="IDataReader"/> containing the fileds to
    /// get ordinals.</param>
    /// <param name="names">A string array containing the names of the
    /// fields to find.</param>
    /// <returns>An array containing the indexes  of the specified column
    /// names within the <paramref name="IDataReader"/></returns>
    /// <remarks>Ordinal-based lookups are more efficient than name lookups, it
    /// is inefficient to call <see cref="IDataReader.GetOrdinal"/> within a
    /// loop. This method provides a convenient way to call the
    /// <see cref="IDataReader.GetOrdinal"/> method for a set of columns
    /// defined within a the <paramref name="IDataReader"/> and reduce the
    /// code len used for that purpose.
    /// <exception cref="ArgumentOutOfRangeException">The number of specified
    /// fields is less than the number of columns.</exception>
    public static int[] GetOrdinals(IDataReader dr, params object[] names) {
      int[] ordinals = new int[names.Length];

      int j = names.Length;
      if (dr.FieldCount < j)
        throw new ArgumentOutOfRangeException(StringResources.DataHelper_OrdArrInvalidOfLen);

      for (int i = 0; i < j; i++)
        ordinals[i] = dr.GetOrdinal((string)names[i]);
      return ordinals;
    }

    /// <summary>
    /// Return the indexes of the named fields.
    /// </summary>
    /// <param name="dr">The IDataReader that contains the fileds to get ordinals.</param>
    /// <param name="ordinals">The array that receives the ordinals.</param>
    /// <param name="reuse">true if the ordinals array can be reused;otherwise false.</param>
    /// <param name="names">A string array containing the names of the fields to find.</param>
    public static void GetOrdinals(IDataReader dr, ref int[] ordinals, bool reuse, params object[] names) {
      int[] ords = ordinals = (reuse) ? (ordinals == null) ? new int[names.Length] : ordinals : new int[names.Length];

      int j = names.Length;
      if (dr.FieldCount < j)
        throw new ArgumentOutOfRangeException("The number of specified fields is less than the number of columns", (Exception)(null));

      for (int i = 0; i < j; i++)
        ordinals[i] = dr.GetOrdinal((string)names[i]);
    }

    public static StringTransferObject PopulateStringTransferFromIDataReader(IDataReader dr, int[] ordinals, string[] names) {
      if (ordinals == null)
        throw new ArgumentNullException("ordinals cannot be null", (Exception)null);
      if (names == null)
        throw new ArgumentNullException("names cannot be null", (Exception)null);
      if (ordinals.Length != names.Length)
        throw new ArgumentOutOfRangeException("ordinals and names must have the same length", (Exception)null);

      string[] values = new string[ordinals.Length];
      for (int i = 0, j = names.Length; i < j; i++) {
        values[i] = dr.GetString(ordinals[i]);
      }
      StringTransferObject str = new StringTransferObject(names, values);
      return str;
    }

    public static DateTime ToSqlDateTime(DateTime input) {
      return (input == DateTime.MinValue) ? new DateTime(1753, 1, 1) : (input == DateTime.MaxValue) ? new DateTime(9999, 12, 31) : input;
    }
    #endregion

    #region Array helpers
    /// <summary>
    /// Searches through an string array removing any duplicate elements.
    /// </summary>
    /// <param name="columns"></param>
    /// <returns></returns>
    public static string[] Unique(string[] strs) {
      int size = 0;
      string[] unique;

      for (int i = 0, j = strs.Length; i < j; i++) {
        string str = strs[i];
        for (int k = 0; k < i; k++) {
          if (string.Compare(str, strs[k], StringComparison.OrdinalIgnoreCase) == 0) {
            strs[k] = null;
            --size;
            break;
          }
          ++size;
        }
      }

      unique = new string[size];
      while (--size > 0)
        if (strs[size] != null)
          unique[size] = strs[size];

      return unique;
    }

    /// <summary>
    /// Converts a string of values separated by a [delimiter] to a array of [type]
    /// </summary>
    /// <param name="values">String containing the values separated by a specific delimiter</param>
    /// <param name="type">The type of array to return</param>
    /// <param name="delimiter">The delimiter used to separate the values</param>
    /// <returns>a array of [type]</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     thrown when:
    ///         . The [type] is not a .NET value type or a string<see cref="http://msdn.microsoft.com/en-us/library/s1ax56ch%28VS.80%29.aspx"/>
    /// </exception>
    public static Array StringToArray(string values, string type, char delimiter) {
      string[] data = values.Split(delimiter);
      bool val = false;
      ArrayList array;
      int j;

      type = type.ToLower();

      if (!type.StartsWith("system"))
        type = "system." + type;

      if (type == "system.string")
        return data;

      // try to get the ValueType object that represents the specified type.
      Type m_type = Type.GetType(type, false, true);
      if (m_type == null || !m_type.IsValueType)
        throw new ArgumentOutOfRangeException(type);

      // all the .NET20 value types implements the TryParse method
      Type[] parms = new Type[] { typeof(string), m_type.MakeByRefType() };
      MethodInfo method = m_type.GetMethod("TryParse", parms);

      j = data.Length;
      array = new ArrayList(j);

      object[] args = { null, null };

      for (int i = 0; i < j; i++) {
        args[0] = data[i];
        args[1] = null;

        val = (bool)method.Invoke(null, args);
        if (val)
          array.Add(args[1]);
      }
      return array.ToArray(m_type);
    }

    /// <summary>
    /// Indicates whether any element of the specified String array is null or an Empty string.
    /// </summary>
    /// <param name="args">An array of String</param>
    /// <returns>true if any element of the array is null or an empty string(""); otherwise, false.</returns>
    /// <remarks>
    /// IsNullOrEmpty is a convenience method that enables you to simultaneously test
    /// each element of a string array for null reference or empty value.
    /// </remarks>
    public static bool IsNullOrEmpty(params string[] args) {
      for (int i = 0, j = args.Length; i < j; i++) {
        if (string.IsNullOrEmpty(args[i])) {
          return true;
        }
      }
      return false;
    }
    #endregion

    #region General helpers
    internal static T ParseStringEnum<T>(string in_value, T out_default) {
      T[] values;
      string[] names;

      names = Enum.GetNames(typeof(T));
      values = (T[])Enum.GetValues(typeof(T));

      for (int i = 0, j = names.Length; i < j; i++) {
        if (string.Compare(in_value, names[i], StringComparison.OrdinalIgnoreCase) == 0)
          return values[i];
      }
      return out_default;
    }

    /// <summary>
    /// Returns a ValueType with the specified <paramref name="ValueType"/> and whose
    /// value is equivalent to the specified <paramref name="s"/>
    /// </summary>
    /// <param name="type">A <paramref name="ValueType"/></param>
    /// <param name="s">A string containing a value to parse</param>
    /// <param name="result">When this method returns, a ValueType whose type is <paramref name="ValueType"/>
    /// an whose value is equivalent to <paramref name="s"/> or default(ValueType) if the conversion is not supported.
    /// </param>
    /// <returns></returns>
    public static bool TryParse(Type type, string s, out System.ValueType result) {
      result = default(System.ValueType);

      if (!type.IsValueType)
        return false;

      bool ret;

      if (type == typeof(int)) {
        int i;
        ret = int.TryParse(s, out i);
        result = i;
        return ret;
      }
      if (type == typeof(bool)) {
        bool boo;
        ret = bool.TryParse(s, out boo);
        result = boo;
        return ret;
      }
      if (type == typeof(decimal)) {
        decimal decima;
        ret = decimal.TryParse(s, out decima);
        result = decima;
        return ret;
      }
      if (type == typeof(DateTime)) {
        DateTime datetime;
        ret = DateTime.TryParse(s, out datetime);
        result = datetime;
        return ret;
      }
      if (type == typeof(long)) {
        long lon;
        ret = long.TryParse(s, out lon);
        result = lon;
        return ret;
      }
      if (type == typeof(float)) {
        float floa;
        ret = float.TryParse(s, out floa);
        result = floa;
        return ret;
      }
      if (type == typeof(double)) {
        double doubl;
        ret = double.TryParse(s, out doubl);
        result = doubl;
        return ret;
      }
      if (type == typeof(char)) {
        char cha;
        ret = char.TryParse(s, out cha);
        result = cha;
        return ret;
      }
      if (type == typeof(uint)) {
        uint uin;
        ret = uint.TryParse(s, out uin);
        result = uin;
        return ret;
      }
      if (type == typeof(byte)) {
        byte byt;
        ret = byte.TryParse(s, out byt);
        result = byt;
        return ret;
      }
      if (type == typeof(short)) {
        short shor;
        ret = short.TryParse(s, out shor);
        result = shor;
        return ret;
      }
      if (type == typeof(ulong)) {
        ulong ulon;
        ret = ulong.TryParse(s, out ulon);
        result = ulon;
        return ret;
      }
      if (type == typeof(ushort)) {
        ushort ushor;
        ret = ushort.TryParse(s, out ushor);
        result = ushor;
        return ret;
      }
      if (type == typeof(sbyte)) {
        sbyte sbyt;
        ret = sbyte.TryParse(s, out sbyt);
        result = sbyt;
        return ret;
      }
      return false;
    }
    #endregion

    #region IO helpers
    /// <summary>
    /// Merge the content of the speficied files into a string object.
    /// </summary>
    /// <param name="base_directory">The directory where the files that will be merged reside</param>
    /// <param name="files_to_merge">An array containing the files to be merged</param>
    /// <returns>The content of the specified files merged or a empty string</returns>
    /// <exception cref="DirectoryNotFoundException">the base directory does not exists.</exception>
    /// <remarks>
    /// The files will be merged in the same order as they apear into the <paramref name="files_to_merge"/>
    /// array. If one of the specified files does not exists we will ignore then and the merging process will
    /// continue to processes the remaining files.
    /// <para>
    /// If the base directory does not exists a DirectoryNotFoundException will be raised.
    /// </para>
    /// <para>
    /// The files_to_merge could be a null reference and, in that case, this method will return a empty string.
    /// </para>
    /// </remarks>
    public static string Merge(string base_directory, string[] files_to_merge) {
      StringBuilder merged_content;
      int merge_array_length;
      char ch;

      if (!Directory.Exists(base_directory))
        throw new DirectoryNotFoundException(base_directory);

      if (files_to_merge == null || (merge_array_length = files_to_merge.Length) == 0)
        return string.Empty;

      // If the directory does not have a separator at the end add it.
      ch = base_directory[base_directory.Length - 1];
      if (ch != Path.DirectorySeparatorChar && ch != Path.AltDirectorySeparatorChar && ch != Path.VolumeSeparatorChar)
        base_directory += Path.DirectorySeparatorChar;

      merged_content = new StringBuilder(merge_array_length * 100); // 100k file size average
      for (int i = 0; i < files_to_merge.Length; i++) {
        string file_to_merge_path = base_directory + files_to_merge[i];
        if (File.Exists(file_to_merge_path)) {
          StreamReader file_reader = new StreamReader(file_to_merge_path, Encoding.Default, false);
          try {
            merged_content.Append(file_reader.ReadToEnd());
          } catch (IOException) {
            // continue at the next file
            file_reader.Close();
            continue;
          } catch (OutOfMemoryException) {
            // we can't continue. there is no memory
            file_reader.Close();
            throw;
          }
          file_reader.Close();
        }
      }
      return merged_content.ToString();
    }
    #endregion
  }
}