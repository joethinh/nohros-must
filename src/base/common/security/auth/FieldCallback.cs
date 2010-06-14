using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Underlying security services instantiate and pass a WebTextCallback to the hanlde method of a
    /// <see cref="CallbackHandler"/> to retrieve a form field information from the current HttpRequest.
    /// </summary>
    public class FieldCallback : IAuthCallback
    {
        string _name;
        string _value;

        /// <summary>
        /// Initializes a new instance of the FormFieldCallback class by using the specified field name
        /// </summary>
        /// <param name="name">The name of the field to retrieve from the current <see cref="HttpRequest"/></param>
        public FieldCallback(string name)
        {
            _name = name;
            _value = null;
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
