using System;
using System.Runtime.Serialization;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Provides the abstract base class for the <see cref="IPermission"/> interface.
    /// <para>
    /// This class implements the GetDataObject method as well as the constructor implied
    /// by the <see cref="ISerializable"/> interface. The methods of the <see cref="IPermission"/>
    /// are not implemented and must be overrided by derived classes.
    /// </para>
    /// </summary>
    public abstract class PermissionBase : IPermission
    {
        /// <summary>
        /// Permission name
        /// </summary>
        protected string name_;

        /// <summary>
        /// Permission actions bitmask
        /// </summary>
        protected long mask_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the PermissionBase class with the default initial capacity.
        /// </summary>
        protected PermissionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PermissionBase class with serialized data.
        /// </summary>
        /// <param name="info">The object taht holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        protected PermissionBase(SerializationInfo info , StreamingContext context)
        {
            name_ = info.GetString("name");
            mask_ = info.GetInt64("mask");
        }
        #endregion

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data</param>
        /// <param name="context">The destination <see cref="StreamingContext"/>for this serialization</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name_, typeof(string));
            info.AddValue("mask", mask_, typeof(long));
        }

        /// <summary>
        /// Checks if the specified permission's actions are "implied by" this object's actions. This must
        /// be implemented by derived classes of IPermission, as they are the only ones that can impose
        /// semantics on a IPermission object. The implies method is used by the SecurityContext to determine
        /// whether or not a requested permission is implied by another permission that is known to be valid
        /// in the current execution context.
        /// </summary>
        /// <param name="perm">The permission to check against</param>
        /// <returns>true if the specified permission is implied by this object, false if not</returns>
        public abstract bool Implies(IPermission perm);

        /// <summary>
        /// Gets the name of this permission.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the actions bitmask that tells the actions that are permited for the object.
        /// </summary>
        public abstract long Mask { get; }
    }
}