using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Logging
{
    /// <summary>
    /// The supported logging levels. Not all loggin libraries support all the levels and when is
    /// the case the related <see cref="ILogger"/> object will always return false for the
    /// Is"LogLevel"Enabled.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// All logging levels.
        /// </summary>
        All = 0,

        /// <summary>
        /// A TRACE logging level.
        /// </summary>
        Trace = 1,

        /// <summary>
        /// A DEBUG logging level.
        /// </summary>
        Debug = 2,

        /// <summary>
        /// A INFO loggin level.
        /// </summary>
        Info = 3,

        /// <summary>
        /// A WARN logging level.
        /// </summary>
        Warn = 4,

        /// <summary>
        /// A ERROR logging level.
        /// </summary>
        Error = 5,

        /// <summary>
        /// A FATAL logging level.
        /// </summary>
        Fatal = 6,

        /// <summary>
        /// Do not log anything.
        /// </summary>
        Off = 7
    }
}
