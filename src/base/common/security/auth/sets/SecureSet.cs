using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Nohros.Resources;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// A collection that contain no duplicates elements. More formally, sets contain no pair of elements
    /// e1 and e2 such that e1.Equals(e2), and at most one null element. As implied by its name this
    /// interface models the mathematical set abstraction.
    /// <para>This interface was designed to be used be the some classes into the security namespace and
    /// is not intended to be a general useful set. In particular it is used by the
    /// <see cref="PermissionSet"/> and <see cref="PrincipalSet"/> classes.
    /// </para>
    /// <para>
    /// .NET version 3.5 and above has classes that implements sets, but this library was designed to be
    /// compatible with .NET 2.0 and this framework version does not have the notion of sets.
    /// </para>
    /// <para>
    /// This class does not provide a way to specify a custom equality comparer, the comparison
    /// operations are done through the value returned from the GetHashCode() for each element in the set.
    /// </para>
    /// </summary>
    class SecureSet<T> : IEnumerable<T>, IEnumerable, ISecureSet<T>
    {
        Dictionary<int, T> elements_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureSet&lt;T&gt;"/> class that is empty, has
        /// the default initial capacity.
        /// </summary>
        public SecureSet() {
            elements_ = new Dictionary<int, T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureSet&lt;T&gt;"/> class that contains
        /// elements copied from the specified collection, and has the sufficient capacity to accomodate
        /// the number of elements copied.
        /// </summary>
        /// <param name="elements">The collection whose elements are copied to the new set.</param>
        /// <remarks>
        /// The capacity of a <see cref="SecureSet"/> object is the number of elements that object can
        /// hold. A <see cref="SecureSet"/> object's capacity automatically increases as elements are
        /// added to the object.
        /// <para>
        /// If the specified collection of elements contains duplicates, the set will contain one of
        /// each unique element. No exception will be throw. Therefore, the size of the resulting set is
        /// not identical to the size of <paramref name="elements"/>. Note that the value returned from
        /// the <typeparamref name="T"/>.GetHashCode() method of the permissions objects is used when
        /// comparing values in the set. If a element of the specified elements collection  is null a
        /// exception will not be throw and the element will not be stored in the set.
        /// </para>
        /// <para>
        /// This method perfors a O(n) operation where n is the size of <paramref name="elements"/>.
        /// </para>
        /// </remarks>
        public SecureSet(IEnumerable<T> elements) {
#if DEBUG
            if (elements == null)
                throw new ArgumentNullException("elements");
#endif
            foreach (T T_to_add in elements) {
                if (T_to_add != null) {
                    elements_[T_to_add.GetHashCode()] = T_to_add;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureSet&lt;T&gt;"/> class that is empty
        /// and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that this set can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        /// <remarks>
        /// The capacity of a <see cref="SecureSet"/> object is the number of elements that object can
        /// hold before resizing is necessary. A <see cref="SecureSet"/> object's capacity automatically
        /// increases as elements are added to the object.
        /// <para>
        /// If the size of the collection can be estimated, specifying the initial capacity eliminates
        /// the need to perform a number of resizing operations while adding elements to the
        /// <see cref="SecureSet&lt;T&gt;"/>
        /// </para>
        /// <para>Note that the value returned from the <typeparamref name="T"/>.GetHashCode() method of
        /// the permissions objects is used when comparing values in the set.
        /// </para>
        /// </remarks>
        public SecureSet(int capacity) {
#if DEBUG
            if (capacity < 0)
                throw new ArgumentNullException("elements");
#endif
            elements_ = new Dictionary<int, T>(capacity);
        }
        #endregion

        /// <summary>
        /// Adds the specified element to the set.
        /// </summary>
        /// <param name="element">The element to add to the set.</param>
        /// <returns>true if the element is added to the <see cref="SecureSet"/> object; false if the
        /// element is already present. Note that the value returned from the GetHashCode() method of the
        /// element object is used when comparing values in the set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is a null reference.</exception>
        public bool Add(T element) {
#if DEBUG
            if (element == null)
                throw new ArgumentNullException("element");
#endif
            // we need to return to the caller a bollean value indicating if the element was successfully
            // added to the list or not, the dictionary does have have such method, so we need to check
            // if the element is already in list. This causes a small performence decration, because the
            // element must be check if it is in the list two times, in ContainsKey and in Item[]=element.
            int hash_code = element.GetHashCode();
            if (elements_.ContainsKey(hash_code)) {
                elements_[hash_code] = element;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Removes the specified element from a <see cref="SecureSet&lt;T&gt;"/> object.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method
        /// returns false if <paramref name="element"/> is not found in the <see cref="SecureSet&lt;T&gt;"/>
        /// object. Note that the value returned from the GetHashCode() method of the element object
        /// is used when comparing values in the set. So, if two elements with the same value coexists
        /// in the set, this method, depending if the <paramref name="element"/> object overrides the
        /// <see cref="Object.Equals"/> method, could remove only one of them.</returns>
        public bool Remove(T element) {
            return elements_.Remove(element.GetHashCode());
        }

        /// <summary>
        /// Determines whether the <see cref="SecureSet&lt;T&gt;"/> contains the specified element.
        /// </summary>
        /// <param name="element">The element to locate in the <see cref="SecureSet&lt;T&gt;"/>.</param>
        /// <returns>true if the item is found in the collection; otherwise, false.</returns>
        /// <remarks>
        /// This method is a O(1) operation.
        /// </remarks>
        public bool Contains(T element) {
            return elements_.Remove(element);
        }

        /// <summary>
        /// Gets the number of elements that are contained in the set.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="SecureSet&lt;T&gt;"/></value>
        /// <remarks>
        /// The capacity of a <see cref="SecureSet&lt;T&gt;"/> object is the number of elements that the
        /// object can hold. A <see cref="SecureSet&lt;T&gt;"/> object's capacity automatically increases
        /// as elements are added to the object.
        /// <para>
        /// The capacity is always greather than or equal to <see cref="Count"/>. If count exceeds the
        /// capacity while adding elements, the capacity is increased by automatically reallocating the
        /// internal storage before copying the old elements and adding the new elements.
        /// </para>
        /// <para>
        /// Getting the value of this property is an O(1) operation.
        /// </para>
        /// </remarks>
        public int Count {
            get { return elements_.Count; }
        }

        /// <summary>
        /// Removes all items from the <see cref="SecureSet&lt;T&gt;"/>
        /// </summary>
        /// <remarks>
        /// <see cref="Count"/> is set to zero, and references to other objects from elements of the
        /// collection are also released. The capacity remains unchanged.
        /// <para>
        /// This method is an O(n) operation, where n is the capacity of the set.
        /// </para>
        /// </remarks>
        public void Clear() {
            elements_.Clear();
        }

        #region IEnumerable && IEnumerable<T>
        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="SecureSet"/>.
        /// </summary>
        /// <returns>A enumerator for the set.</returns>
        public IEnumerator<T> GetEnumerator() {
            foreach (KeyValuePair<int, T> T_set_key in elements_) {
                yield return T_set_key.Value;
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="SecureSet"/>.
        /// </summary>
        /// <returns>A enumerator for the set.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion
    }
}
