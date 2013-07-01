using System;
using System.Runtime.Serialization;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Defines a base class for predefined exceptions in the Nohros.Security.Auth namespace
    /// </summary>
    [Serializable]
    public class LoginException : System.Exception
    {
        public LoginException():base()
        {
        }

        public LoginException(string message):base(message)
        {
        }

        public LoginException(SerializationInfo info, StreamingContext context):base(info, context)
        {
        }

        public LoginException(string message, System.Exception innerException): base(message, innerException)
        {
        }
    }
}
