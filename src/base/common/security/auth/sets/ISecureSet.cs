using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// A collection that contains no duplicates elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISecureSet<T>
    {
        /// <summary>
        /// Adds the specified element to the set.
        /// </summary>
        /// <param name="element">The element to add to the set.</param>
        /// <returns>true if the element is added to the <see cref="SecureSet"/> object; false if the
        /// element is already present. Note that the value returned from the GetHashCode() method of the
        /// element object is used when comparing values in the set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is a null reference.</exception>
        bool Add(T element);

        /// <summary>
        /// Removes the specified element from a <see cref="ISecureSet&lt;T&gt;"/> object.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method
        /// returns false if <paramref name="element"/> is not found in the <see cref="ISecureSet&lt;T&gt;"/>
        /// object. Note that the value returned from the GetHashCode() method of the element object
        /// is used when comparing values in the set. So, if two elements with the same value coexists
        /// in the set, this method, depending if the <paramref name="element"/> object overrides the
        /// <see cref="object.Equals(object)"/> method, could remove only one of them.</returns>
        bool Remove(T element);

        /// <summary>
        /// Determines whether the <see cref="ISecureSet&lt;T&gt;"/> contains a specific value.
        /// </summary>
        /// <param name="element">The object to locate in the <see cref="ISecureSet&lt;T&gt;"/>.</param>
        /// <returns>true if the item is found in the collection; otherwise, false.</returns>
        bool Contains(T element);

        /// <summary>
        /// Gets the number of elements that are contained in the set.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="ISecureSet&lt;T&gt;"/></value>
        int Count { get; }

        /// <summary>
        /// Removes all items from the <see cref="ISecureSet&lt;T&gt;"/>
        /// </summary>
        /// <remarks>
        /// <see cref="Count"/> must be set to zero, and references to other objects from elements of the
        /// collection must be released.
        /// </remarks>
        void Clear();
    }
}
