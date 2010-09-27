using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represents whether or not a <see cref="ILoginModule"/> implementation class is REQUIRED, REQUISITE,
    /// SUFFICIENT or OPTIONAL.
    /// </summary>
    public enum LoginModuleControlFlag
    {
        /// <summary>
        /// Optional login module
        /// </summary>
        OPTIONAL = 0,

        /// <summary>
        /// Required login module
        /// </summary>
        REQUIRED = 1,

        /// <summary>
        /// Requisite login module
        /// </summary>
        REQUISITE = 2,

        /// <summary>
        /// Suficient login module
        /// </summary>
        SUFFICIENT = 3
    }
}
