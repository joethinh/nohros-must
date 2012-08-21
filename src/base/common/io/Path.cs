using System;
using System.Reflection;

namespace Nohros.IO
{
  /// <summary>
  /// A collection of methods that extends the .NET <see cref="Path"/> built-in
  /// class.
  /// </summary>
  public static class Path
  {
    /// <summary>
    /// Gets the directory where the calling assembly is stored.
    /// </summary>
    /// <returns>
    /// The directory where the calling assembly is stored.
    /// </returns>
    public static string GetCallingAssemblyDirectory() {
      return GetDirectoryName(Assembly.GetCallingAssembly().Location);
    }

    /// <summary>
    /// Convert provided relative <paramref name="path"/> into an absolute
    /// path using the calling assembly location as base directory.
    /// </summary>
    /// <param name="path">
    /// The relative path to be converted.
    /// </param>
    /// <returns>
    /// Teh absolute path for <paramref name="path"/>.
    /// </returns>
    /// <remarks>
    /// If <paramref name="path"/> is not relative this method simple returns
    /// the provided path.
    /// </remarks>
    public static string AbsoluteForCallingAssembly(string path) {
      if (IsPathRooted(path)) {
        return path;
      }
      string calling_assembly_directory =
        GetDirectoryName(Assembly.GetCallingAssembly().Location);
      return GetFullPath(Combine(calling_assembly_directory, path));
    }

    /// <summary>
    /// Convert provided relative <paramref name="path"/> into an absolute path
    /// using the <see cref="AppDomain.BaseDirectory"/> as base directory.
    /// </summary>
    /// <param name="path">
    /// The relative path to be converted.
    /// </param>
    /// <returns>
    /// Teh absolute path for <paramref name="path"/>.
    /// </returns>
    /// <remarks>
    /// If <paramref name="path"/> is not relative this method simple returns
    /// the provided path.
    /// </remarks>
    public static string AbsoluteForApplication(string path) {
      if (IsPathRooted(path)) {
        return path;
      }
      return GetFullPath(Combine(AppDomain.CurrentDomain.BaseDirectory, path));
    }

    /// <summary>
    /// This method has the same functionality of the method
    /// <see cref="System.IO.Path.IsPathRooted"/> and exists to allow users
    /// to use the <see cref="System.IO.Path.IsPathRooted"/> without including
    /// the <see cref="System.IO"/> namespace.
    /// </summary>
    public static bool IsPathRooted(string path) {
      return System.IO.Path.IsPathRooted(path);
    }

    /// <summary>
    /// This method has the same functionality of the method
    /// <see cref="System.IO.Path.Combine"/> and exists to allow users
    /// to use the <see cref="System.IO.Path.Combine"/> without including
    /// the <see cref="System.IO"/> namespace.
    /// </summary>
    public static string Combine(string path1, string path2) {
      return System.IO.Path.Combine(path1, path2);
    }

    /// <summary>
    /// This method has the same functionality of the method
    /// <see cref="System.IO.Path.GetDirectoryName"/> and exists to allow users
    /// to use the <see cref="System.IO.Path.GetDirectoryName"/> without including
    /// the <see cref="System.IO"/> namespace.
    /// </summary>
    public static string GetDirectoryName(string path) {
      return System.IO.Path.GetDirectoryName(path);
    }

    /// <summary>
    /// This method has the same functionality of the method
    /// <see cref="System.IO.Path.GetFullPath"/> and exists to allow users
    /// to use the <see cref="System.IO.Path.GetFullPath"/> without including
    /// the <see cref="System.IO"/> namespace.
    /// </summary>
    public static string GetFullPath(string path) {
      return System.IO.Path.GetFullPath(path);
    }

    /// <summary>
    /// This method has the same functionality of the method
    /// <see cref="System.IO.Path.GetExtension"/> and exists to allow users
    /// to use the <see cref="System.IO.Path.GetExtension"/> without including
    /// the <see cref="System.IO"/> namespace.
    /// </summary>
    public static string GetExtension(string path) {
      return System.IO.Path.GetExtension(path);
    }

    /// <summary>
    /// This method has the same functionality of the method
    /// <see cref="System.IO.Path.GetFileName"/> and exists to allow users
    /// to use the <see cref="System.IO.Path.GetFileName"/> without including
    /// the <see cref="System.IO"/> namespace.
    /// </summary>
    public static string GetFileName(string path) {
      return System.IO.Path.GetFileName(path);
    }

    /// <summary>
    /// This method has the same functionality of the method
    /// <see cref="System.IO.Path.GetFileNameWithoutExtension"/> and exists to
    /// allow users to use the
    /// <see cref="System.IO.Path.GetFileNameWithoutExtension"/> without
    /// including the <see cref="System.IO"/> namespace.
    /// </summary>
    public static string GetFileNameWithoutExtension(string path) {
      return System.IO.Path.GetFileNameWithoutExtension(path);
    }

    /// <summary>
    /// This method has the same functionality of the method
    /// <see cref="System.IO.Path.GetPathRoot"/> and exists to
    /// allow users to use the
    /// <see cref="System.IO.Path.GetPathRoot"/> without
    /// including the <see cref="System.IO"/> namespace.
    /// </summary>
    public static string GetPathRoot(string path) {
      return System.IO.Path.GetPathRoot(path);
    }
  }
}
