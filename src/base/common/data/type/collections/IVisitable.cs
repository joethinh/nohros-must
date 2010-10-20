using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// Defines methods and properties that allows a <see cref="IVisitor&gt;T&lt;"/> to visit every
    /// element contained within the structure.
    /// </summary>
    public interface IVisitable<T>
    {
        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor to accepts.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference</exception>
        void Accept(IVisitor<T> visitor);
    }

    /// <summary>
    /// Defines methods and properties that allows a <see cref="IVisitor&gt;T1&lt;,&gt;T2&lt;"/> to visit every
    /// element contained within the structure.
    /// </summary>
    public interface IVisitable<T1, T2>
    {
        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor to accepts.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference</exception>
        void Accept(IVisitor<T1, T2> visitor);
    }
}
