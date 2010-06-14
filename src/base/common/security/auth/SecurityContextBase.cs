using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// This API supports the Nohros Framework infrastructure and is not intended to be used
    /// directly from your code.
    /// </summary>
    public class SecurityContextBase
    {
        protected Subject _subject;

        /// <summary>
        /// Determines whether the access request by the specified permission should be allowed or denied.
        /// </summary>
        /// <param name="perm">The requested permission</param>
        /// <returns>true if the access request is permitted; otherwise, false.</returns>
        public bool CheckPermission(IPermission perm)
        {
            if (_subject == null)
                return false;
            return _subject.CheckPermission(perm);
        }

        /// <summary>
        /// Gets the <see cref="Subject"/> associated with this SecurityContext.
        /// </summary>
        public Subject Subject
        {
            get { return _subject; }
            internal set { _subject = value; }
        }
    }
}
