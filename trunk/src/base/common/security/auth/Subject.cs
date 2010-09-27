using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

using Nohros.Data;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// A Subject represents a grouping of related information for a single entity, such a person. Such
    /// information includes the Subject's identity as well as its security-related attributes(passwords
    /// and permissions, for example).
    /// </summary>
    public partial class Subject
    {
        IList<IPermission> permissions_;
        string _id;

        #region .ctor

        /// <summary>
        /// Initializes a new instance_ of the Subject class.
        /// </summary>
        public Subject()
        {
            permissions_ = new List<IPermission>();
        }

        /// <summary>
        /// Create an instance_ of a Subject with Permissions and SubjectLoader delegate.
        /// </summary>
        /// <param name="permissions">The Subject's permission set</param>
        /// <exception cref="ArgumentNullException">permissions is null</exception>
        public Subject(Dictionary<string, IPermission> permissions)
        {
            if (permissions == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.obj);
        }
        #endregion

        /// <summary>
        /// Determines whether the access request indicated by the specified permission should be allowed
        /// or denied, based on the current security context.
        /// </summary>
        /// <param name="permission">The requested permission</param>
        /// <returns>true if the access request is permitted; otherwise false</returns>
        public bool CheckPermission(IPermission permission)
        {
            for(int i=0, j=permissions_.Count;i<j;i++) {
                if (permissions_[i].Implies(permission))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the <see cref="Dictionary&lt;TKey, TValue&gt;"/> of <see cref="IPermission"/> associated
        /// with this Subject. Each <see cref="IPermission"/> represents an access to a system resource.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="Dictionary&lt;TKey, TValue&gt;"/> is backed by this Subject's internal
        /// <see cref="IPermission"/> <see cref="Dictionary&lt;TKey, TValue&gt;"/>. Any modification to the
        /// returned <see cref="Dictionary&lt;TKey, TValue&gt;"/> affects the internal <see cref="IPermission"/>
        /// <see cref="Dictionary&alt;TKey, TValue&gt;"/> as well.
        /// </remarks>
        public IList<IPermission> Permissions
        {
            get { return permissions_; }
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the subject within the application.
        /// </summary>
        public string ID
        {
            get { return _id; }
            set
            {
                if (value == null)
                    Thrower.ThrowArgumentNullException(ExceptionArgument.value);

                // the id value is set after a sucessfull login attempt.
                // at this point we have all needed information to cache the subject.
                if(_id != value)
                {
                    _id = value;
                    Subject.Add(this);
                }
            }
        }
    }
}