using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Collections
{
    /// <summary>
    /// Defines methods and properties that allows a <see cref="IVisitor&gt;T&lt;"/> to visit, in order, every
    /// element contained within the structure. This interface is similar to the <see cref="IVisitable&gt;T&lt;"/>
    /// interface, but the <see cref="Accept"/> expects a <see cref="InOrderVisitor&gt;T&lt;"/> insted of a
    /// <see cref="IVisitor&gt;T&lt;"/> method  It is useful for classes that could be traversed in different
    /// orders(PreOrder, PostOrder, InOrder, ...)</remarks>
    /// </summary>
    public interface IInOrderVisitable<T>
    {
        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor to accepts</param>
        /// <param name="reverse_order">A value indicating if the elements will be visit in the reverse order or not.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about the visitor's
        /// current state.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference</exception>
        void Accept(InOrderVisitor<T> visitor, object state, bool reverse_order);
    }

    /// <summary>
    /// Defines methods and properties that allows a <see cref="IVisitor&gt;T1, T2&lt;"/> to visit, in order, every
    /// element contained within the structure. This interface is similar to the <see cref="IVisitable&gt;T1, T2&lt;"/>
    /// interface, but the <see cref="Accept"/> expects a <see cref="InOrderVisitor&gt;T&lt;"/> insted of a
    /// <see cref="IVisitor&gt;T&lt;"/> method  It is useful for classes that could be traversed in different
    /// orders(PreOrder, PostOrder, InOrder, ...)</remarks>
    /// </summary>
    public interface IInOrderVisitable<T1, T2>
    {
        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor to accepts</param>
        /// <param name="reverse_order">A value indicating if the elements will be visit in the reverse order or not.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about the visitor's
        /// current state.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference</exception>
        void Accept(InOrderVisitor<T1, T2> visitor, object state, bool reverse_order);
    }
}
