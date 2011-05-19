using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represent a set of principals.
    /// </summary>
    public class PrincipalSet : ISecureSet<IPrincipal>, IEnumerable<IPrincipal>, IEnumerable
    {
        SecureSet<IPrincipal> principals_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalSet"/> class that is empty, has
        /// the default initial capacity.
        /// </summary>
        public PrincipalSet() {
            principals_ = new SecureSet<IPrincipal>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalSet"/> class that contains
        /// elements copied from the specified collection, and has the sufficient capacity to accomodate
        /// the number of elements copied.
        /// </summary>
        /// <param name="principals">The collection whose elements are copied to the new set.</param>
        /// <remarks>
        /// The capacity of a <see cref="PrincipalSet"/> object is the number of elements that object can
        /// hold. A <see cref="PrincipalSet"/> object's capacity automatically increases as elements are
        /// added to the object.
        /// <para>
        /// If the specified collection of principals contains duplicates, the set will contain one of
        /// each unique element. No exception will be throw. Therefore, the size of the resulting set is
        /// not identical to the size of <paramref name="principals"/>. Note that the value returned from
        /// the <see cref="IPrincipal.GetHashCode()"/> method of the princiapls objects is used when
        /// comparing values in the set. If a element of the specified elements collection is null a
        /// exception will not be throw and the element will not be stored in the set.
        /// </para>
        /// <para>
        /// This method perfors a O(n) operation where n is the size of <paramref name="principals"/>.
        /// </para>
        /// </remarks>
        public PrincipalSet(IEnumerable<IPrincipal> principals) {
            if (principals == null)
                throw new ArgumentNullException("principals");
            principals_ = new SecureSet<IPrincipal>(principals);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalSet"/> class that is empty
        /// and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that this set can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        /// <remarks>
        /// The capacity of a <see cref="PrincipalSet"/> object is the number of elements that object can
        /// hold before resizing is necessary. A <see cref="PrincipalSet"/> object's capacity automatically
        /// increases as elements are added to the object.
        /// <para>
        /// If the size of the collection can be estimated, specifying the initial capacity eliminates
        /// the need to perform a number of resizing operations while adding elements to the
        /// <see cref="PrincipalSet"/>
        /// </para>
        /// <para>Note that the value returned from the <see cref="IPrincipal.GetHashCode()"/> method of
        /// the principals objects is used when comparing values in the set.
        /// </para>
        /// </remarks>
        public PrincipalSet(int capacity) {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            principals_ = new SecureSet<IPrincipal>(capacity);
        }
        #endregion

        /// <summary>
        /// Adds the specified principal to a set.
        /// </summary>
        /// <param name="principal">The principal to add to the set.</param>
        /// <returns>true if the element is added to the <see cref="PrincipalSet"/> object; false if the
        /// element is already present. Note that the principal name is used when comparing values in the
        /// set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="principal"/> is a null reference.</exception>
        public bool Add(IPrincipal principal) {
            if (principal == null)
                throw new ArgumentNullException("principal");
            return principals_.Add(principal);
        }

        /// <summary>
        /// Removes the specified element from a <see cref="PrincipalSet"/> object.
        /// </summary>
        /// <param name="principal">The element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method
        /// returns false if <paramref name="principal"/> is not found in the <see cref="PrincipalSet"/>
        /// object. Note that the principal name is used when comparing values in the set.</returns>
        /// <remarks>
        /// If <paramref name="principal"/> is null a exception will not be throw and this method simple
        /// returns false.
        /// </remarks>
        public bool Remove(IPrincipal principal) {
            if (principal == null)
                return false;
            return principals_.Remove(principal);
        }

        /// <summary>
        /// Determines whether the <see cref="PrincipalSet"/> contains the specified element.
        /// </summary>
        /// <param name="principal">The principal to locate in the <see cref="PrincipalSet"/>.</param>
        /// <returns>true if the item is found in the collection; otherwise, false.</returns>
        /// <remarks>
        /// This method is a O(1) operation.
        /// </remarks>
        public bool Contains(IPrincipal principal) {
            return principals_.Contains(principal);
        }

        /// <summary>
        /// Gets the number of elements that are contained in the set.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="PrincipalSet"/></value>
        /// <remarks>
        /// The capacity of a <see cref="PrincipalSet"/> object is the number of elements that the
        /// object can hold. A <see cref="PrincipalSet"/> object's capacity automatically increases
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
            get { return principals_.Count; }
        }

        /// <summary>
        /// Removes all items from the <see cref="PrincipalSet"/>
        /// </summary>
        /// <remarks>
        /// <see cref="Count"/> is set to zero, and references to other objects from elements of the
        /// collection are also released. The capacity remains unchanged.
        /// <para>
        /// This method is an O(n) operation, where n is the capacity of the set.
        /// </para>
        /// </remarks>
        public void Clear() {
            principals_.Clear();
        }

        /// <summary>
        /// Removes the a principal whose name is <paramref name="name"/> from a <see cref="PrincpalSet"/>.
        /// </summary>
        /// <param name="name">The name of the principal to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method
        /// returns false if a principal whose name is <paramref name="name"/> is not found in the
        /// <see cref="PrincipalSet"/> object. Note that the value returned from the GetHashCode() is
        /// used when comparing values in the set.</returns>
        /// <remarks>
        /// If <paramref name="principal"/> is null a exception will not be throw and this method simple
        /// returns false.
        /// </remarks>
        //public bool Remove(string name) {
        //}

        #region IEnumerable && IEnumerable<T>
        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="PrincipalSet"/>.
        /// </summary>
        /// <returns>A enumerator for the set.</returns>
        public IEnumerator<IPrincipal> GetEnumerator() {
            return principals_.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="PrincipalSet"/>.
        /// </summary>
        /// <returns>A enumerator for the set.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return principals_.GetEnumerator();
        }
        #endregion
    }
}