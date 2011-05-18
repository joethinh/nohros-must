using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Represents an abstract notion of a principal, which can be used to represent any entity such as
    /// an individual, a corporation, and a login id.
    /// </summary>
    public interface IPrincipal
    {
        /// <summary>
        /// Gets the name of the principal.
        /// </summary>
        string Name { get; }
    }
}
