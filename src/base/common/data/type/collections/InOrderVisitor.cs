using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// A visitor that visits single objects in order.
    /// </summary>
    /// <typeparam name="T">The type of objects to be visited.</typeparam>
    /// <remarks>This class only wraps the methods of the <see cref="Visitor"/>. It is useful
    /// for classes that could be traversed in different orders(PreOrder, PostOrder, InOrder, ...)</remarks>
    public class InOrderVisitor<T> : IVisitor<T>
    {
        IVisitor<T> visitor_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the InOrderVisitor class by using the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor to use when visiting the object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference.</exception>
        public InOrderVisitor(IVisitor<T> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            visitor_ = visitor;
        }
        #endregion

        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">The object to visit.</param>
        public void Visit(T obj) {
            visitor_.Visit(obj);
        }

        /// <summary>
        /// Gets a value indicating whether the visitor is done performing your work.
        /// </summary>
        /// <value><c>true</c> if the visitor is done performing your work; otherwise <c>false</c>.</value>
        /// <remarks>
        /// This property is typically used in collection traversal operations. In some situations a
        /// collection need to be traversed(in some order) and the traversal operation must be stoped at some
        /// point. A visitor could inform the visited collection about the stop point, by setting the value of
        /// this property to false. The visited collection can check the value of this property for each visited
        /// element and then determine when the traversal operation must be stoped.
        /// </remarks>
        public bool HasCompleted {
            get { return visitor_.HasCompleted; }
        }
    }

    /// <summary>
    /// A visitor that visits compound objects in order. Compound objects are objects that are identified
    /// by more than one object.<example>A node within a <see cref="IDictionary&gt;T&lt;"/></example>
    /// </summary>
    /// <typeparam name="T">The type of objects to be visited.</typeparam>
    /// <remarks>This class only wraps the methods of the <see cref="Visitor"/>. It is useful
    /// for classes that could be traversed in different orders(PreOrder, PostOrder, InOrder, ...)</remarks>
    public class InOrderVisitor<T1, T2> : IVisitor<T1, T2>
    {
        IVisitor<T1, T2> visitor_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the InOrderVisitor class by using the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor to use when visiting the object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is a null reference.</exception>
        public InOrderVisitor(IVisitor<T1, T2> visitor) {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            visitor_ = visitor;
        }
        #endregion

        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj1">The first component of the object to visit.</param>
        /// <param name="obj2">The second component of the object to visit.</param>
        public void Visit(T1 obj1, T2 obj2) {
            visitor_.Visit(obj1, obj2);
        }

        /// <summary>
        /// Gets a value indicating whether the visitor is done performing your work.
        /// </summary>
        /// <value><c>true</c> if the visitor is done performing your work; otherwise <c>false</c>.</value>
        /// <remarks>
        /// This property is typically used in collection traversal operations. In some situations a
        /// collection need to be traversed(in some order) and the traversal operation must be stoped at some
        /// point. A visitor could inform the visited collection about the stop point, by setting the value of
        /// this property to false. The visited collection can check the value of this property for each visited
        /// element and then determine when the traversal operation must be stoped.
        /// </remarks>
        public bool HasCompleted {
            get { return visitor_.HasCompleted; }
        }
    }
}
