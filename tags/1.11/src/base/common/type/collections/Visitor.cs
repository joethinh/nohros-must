using System;

namespace Nohros.Collections
{
  /// <summary>
  /// A implementation of the <see cref="IVisitor{T}"/> that forwards the
  /// <see cref="Visit"/> method call to a <see cref="VisitorCallback{T}"/>.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class Visitor<T> : IVisitor<T>
  {
    readonly VisitorCallback<T> callback_;
    bool is_completed_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Visitor{T}"/> class
    /// by using the specified <see cref="VisitorCallback{T}"/>.
    /// </summary>
    /// <param name="callback">
    /// A <see cref="VisitorCallback{T}"/> to forwards the <see cref="Visit"/>
    /// method.
    /// </param>
    public Visitor(VisitorCallback<T> callback) {
      callback_ = callback;
    }
    #endregion

    /// <inheritdoc/>
    public void Visit(T obj, object state) {
      callback_(obj, state);
    }

    /// <inheritdoc/>
    public bool IsCompleted {
      get { return is_completed_; }
      set { is_completed_ = value; }
    }
  }

  /// <summary>
  /// A implementation of the <see cref="IVisitor{T1, T2}"/> that forwards the
  /// <see cref="Visit"/> method call to a <see cref="VisitorCallback{T1, T2}"/>.
  /// </summary>
  /// <typeparam name="T1"></typeparam>
  /// <typeparam name="T2"></typeparam>
  public class Visitor<T1, T2> : IVisitor<T1, T2>
  {
    readonly VisitorCallback<T1, T2> callback_;
    bool is_completed_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Visitor{T}"/> class
    /// by using the specified <see cref="VisitorCallback{T}"/>.
    /// </summary>
    /// <param name="callback">
    /// A <see cref="VisitorCallback{T}"/> to forwards the <see cref="Visit"/>
    /// method.
    /// </param>
    public Visitor(VisitorCallback<T1, T2> callback) {
      callback_ = callback;
      is_completed_ = false;
    }
    #endregion

    /// <inheritdoc/>
    public void Visit(T1 obj1, T2 obj2, object state) {
      callback_(obj1, obj2, state);
    }

    /// <inheritdoc/>
    public bool IsCompleted {
      get { return is_completed_; }
      set { is_completed_ = value; }
    }
  }
}
