using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represent a set of principals.
    /// </summary>
    public class PrincipalSet : IEnumerable<IPrincipal>, IEnumerable
    {
        Dictionary<string, IPrincipal> principals_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalSet"/> class that is empty.
        /// </summary>
        public PrincipalSet() {
            principals_ = new Dictionary<string, IPrincipal>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalSet"/> class that contains elements
        /// copied from the specified collection, and has sufficient capacity to accomodate the number of
        /// elements copied.
        /// </summary>
        /// <param name="principals">The collection whose elements are copied to the new set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="principals"/> is a null reference.</exception>
        /// <remarks>
        /// The capacity of a <see cref="PrincipalSet"/> object is the number of elements that object
        /// can hold. A <see cref="PrincipalSet"/> object's capacity automatically increases as elements
        /// are added to the object.
        /// <para>
        /// If the specified collection of principals contains duplicates, the set will contain one of
        /// each unique element. No exception will be throw. Therefore, the size of the resulting set is
        /// not identical to the size of <paramref name="principals"/>. Note that the name of the
        /// principal is used when comparing values in the set. If a element of the specified principals
        /// collection is null a exception will not be throw and the element will not be stored in the set.
        /// </para>
        /// <para>
        /// This method performs an O(n) operation.
        /// </para>
        /// </remarks>
        public PrincipalSet(IEnumerable<IPrincipal> principals) {
            if (principals == null)
                throw new ArgumentNullException("principals");

            foreach (IPrincipal principal_to_add in principals) {
                if (principal_to_add != null) {
                    principals_[principal_to_add.Name] = principal_to_add;
                }
            }
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

            string principal = principal.Name;
            if (principals_.ContainsKey(principal)) {
                principals_[principal] = principal;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Removes the specified element from a <see cref="PrincipalSet"/> object.
        /// </summary>
        /// <param name="principal">The element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method
        /// returns false if <paramref name="principal"/> is not found in the <see cref="PrincipalSet"/>
        /// object. Note that the principal name is used when comparing values in the set.</returns>
        public bool Remove(IPrincipal principal) {
            if (principal == null)
                throw new ArgumentNullException("principal");
            return principals_.Remove(principal.Name);
        }

        #region IEnumerable && IEnumerable<T>
        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="PrincipalSet"/>.
        /// </summary>
        /// <returns>A enumerator for the set.</returns>
        public IEnumerator<IPrincipal> GetEnumerator() {
            foreach (KeyValuePair<int, IPrincipal> principal_set_key in principals_) {
                yield return principal_set_key.Value;
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="PrincipalSet"/>.
        /// </summary>
        /// <returns>A enumerator for the set.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion
    }
}