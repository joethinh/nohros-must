using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// Defines properties_ and methods that objects that must be validate before it usage
    /// should implement.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Evaluates the condition it checks and updates the IsValid property.
        /// </summary>
        void Validate();

        /// <summary>
        /// Gets or sets the error message text generated when the condition beign validated fails.
        /// </summary>
        /// <returns>The error message to generate</returns>
        string ErrorMessage { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the object passes validation
        /// </summary>
        /// <returns>true if the object is valid; otherwise, false</returns>
        bool IsValid { get; }
    }
}
