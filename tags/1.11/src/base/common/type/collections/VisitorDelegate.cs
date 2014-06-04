using System;

namespace Nohros.Collections
{
  /// <summary>
  /// Represents a callback method to be executed by a visitor
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="obj">
  /// The visited object.
  /// </param>
  /// <param name="state">
  /// An object containing information to be used
  /// by the callback method.
  /// </param>
  public delegate void VisitorCallback<in T>(T obj, object state);

  /// <summary>
  /// Represents a callback method to be executed by a visitor
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="obj1">
  /// The visited object.
  /// </param>
  /// <param name="obj2">
  /// The visited object.
  /// </param>
  /// <param name="state">
  /// An object containing information to be used
  /// by the callback method.
  /// </param>
  public delegate void VisitorCallback<in T1, in T2>(
    T1 obj1, T2 obj2, object state);
}
