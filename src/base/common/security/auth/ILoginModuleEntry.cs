using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// This class represents a single login module configured for the application specified
    /// in the configuration file. Each respective LoginEntryModule contains a login module's name,
    /// and Type, a control flag( LoginModuleControlFlag ), and a login module's specific options.
    /// </summary>
    /// <seealso cref="ILoginConfiguration"/>
    public interface ILoginModuleEntry
    {
        /// <summary>
        /// Gets the name of the login module
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the underlying login module.
        /// </summary>
        ILoginModule Module { get; }

        /// <summary>
        /// Gets the type of the login module class.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the control flag for this login module.
        /// </summary>
        LoginModuleControlFlag ControlFlag { get; }

        /// <summary>
        /// Gets the options configured for this login module.
        /// </summary>
        IDictionary<string, object> Options { get; }
    }
}
