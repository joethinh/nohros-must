using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Threading;

using Nohros;
using Nohros.Data;
using Nohros.Resources;
using Nohros.Configuration;

namespace Nohros.Net
{
    internal class NetSettings : NohrosConfiguration
    {
        static NetSettings instance_;

        #region .ctor
        /// <summary>
        /// Static initializer.
        /// </summary>
        static NetSettings() {
            instance_ = new NetSettings();
            instance_.Load();
        }

        /// <summary>
        /// Initializes a new instance_ of the NetSettings class.
        /// </summary>
        public NetSettings() { }
        #endregion

        /// <summary>
        /// Gets the singleton NetSettings representing the current application net settings.
        /// </summary>
        public static NetSettings ForCurrentProcess {
            get { return instance_; }
        }
    }
}