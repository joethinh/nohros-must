using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AjaxPro
{
    public class AjaxEventArgs : EventArgs
    {
        private MethodInfo _method;
        private ICustomAttribute[] _attributes;

        /// <summary>
        /// Initializes a new instance of the AjaxEventArgs class
        /// </summary>
        /// <param name="method">The MethodInfo object representing the server side
        /// ajax called method</param>
        /// <param name="attributes">The user-defined attributes binded to the method</param>
        public AjaxEventArgs(MethodInfo method, ICustomAttribute[] attributes)
        {
            _method = method;
            _attributes = attributes;
        }

        /// <summary>
        /// Gets a MethodInfo object representing the
        /// client requested method.
        /// </summary>
        public MethodInfo Method {
            get { return _method; }
        }

        /// <summary>
        /// Gets a ICustomAttribute array representing the user defined attributes.
        /// </summary>
        public ICustomAttribute[] Attributes {
            get { return _attributes; }
        }
    }
}
