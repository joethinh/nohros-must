using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Collections
{
    /// <summary>
    /// Provides an interface for visitors that visits single objects.
    /// </summary>
    public interface IVisitor<T>
    {
        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">The object to visit.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about the visitor's
        /// current state.</param>
        void Visit(T obj, object state);

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
        bool IsCompleted { get; }
    }

    /// <summary>
    /// Provides an interface for visitors that visits compound objects. That is, objects that are identified
    /// by more than one object.<example>A node within a <see cref="IDictionary&gt;T&lt;"/></example>
    /// </summary>
    public interface IVisitor<T1, T2>
    {
        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj1">The first component of the object to visit.</param>
        /// <param name="obj2">The second component of the object to visit.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about the visitor's
        /// current state.</param>
        void Visit(T1 obj1, T2 obj2, object state);

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
        bool IsCompleted { get; }
    }
}
