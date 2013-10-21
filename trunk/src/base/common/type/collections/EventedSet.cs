using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Collections
{
  /// <summary>
  /// A decorator over the <see cref="ISet{T}"/> that raise a event when
  /// a element is added.
  /// </summary>
  /// <typeparam name="T">
  /// The type of elements that the set contains.
  /// </typeparam>
  public class EventedSet<T> : ISet<T>
  {
    readonly ISet<T> set_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="EventedSet{T}"/> class
    /// by using the given <paramref name="set"/> of objects.
    /// </summary>
    /// <param name="set">
    /// A <see cref="ISet{T}"/> that can be used to store objects of the type
    /// <typeparamref name="T"/>.
    /// </param>
    public EventedSet(ISet<T> set) {
      set_ = set;
    }
    #endregion

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    void ICollection<T>.Add(T item) {
      Add(item);
    }

    public bool Add(T item) {
      bool result = set_.Add(item);
      if (result) {
        OnItemAdded(item);
      }
      return result;
    }

    public void Clear() {
      set_.Clear();
    }

    public bool Contains(T item) {
      return set_.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex) {
      set_.CopyTo(array, arrayIndex);
    }

    public int Count {
      get { return set_.Count; }
    }

    public void ExceptWith(IEnumerable<T> other) {
      set_.ExceptWith(other);
    }

    public IEnumerator<T> GetEnumerator() {
      return set_.GetEnumerator();
    }

    public void IntersectWith(IEnumerable<T> other) {
      set_.IntersectWith(other);
    }

    public bool IsProperSubsetOf(IEnumerable<T> other) {
      return set_.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other) {
      return set_.IsProperSupersetOf(other);
    }

    public bool IsReadOnly {
      get { return set_.IsReadOnly; }
    }

    public bool IsSubsetOf(IEnumerable<T> other) {
      return set_.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<T> other) {
      return set_.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other) {
      return set_.Overlaps(other);
    }

    public bool Remove(T item) {
      return set_.Remove(item);
    }

    public bool SetEquals(IEnumerable<T> other) {
      return set_.SetEquals(other);
    }

    public void SymmetricExceptWith(IEnumerable<T> other) {
      set_.SymmetricExceptWith(other);
    }

    public void UnionWith(IEnumerable<T> other) {
      set_.UnionWith(other);
    }

    /// <summary>
    /// Occurs when an item is added to the set.
    /// </summary>
    public event Action<T> ItemAdded;

    protected virtual void OnItemAdded(T obj) {
      Listeners.SafeInvoke<Action<T>>(ItemAdded, handler => handler(obj));
    }
  }
}
