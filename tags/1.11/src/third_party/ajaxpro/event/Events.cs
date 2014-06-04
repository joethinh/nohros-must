/*
 * AjaxSyncHttpHandler.cs
 * 
 * Copyright © 2007 Michael Schwarz (http://www.ajaxpro.info).
 * All Rights Reserved.
 * 
 * Permission is hereby granted, free of charge, to any person 
 * obtaining a copy of this software and associated documentation 
 * files (the "Software"), to deal in the Software without 
 * restriction, including without limitation the rights to use, 
 * copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the 
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be 
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
 * ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * [0] 28-12-2010 - neylor.silva
 *     Initial release.
 * 
 */

using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace AjaxPro
{
    #region Delegates
    public delegate void AjaxProcessEventHandler(AjaxEventArgs e);
    #endregion

    /// <summary>
    /// Provides a way to interface with the ajax processing pipeline.
    /// </summary>
    public class Events
    {
        static EventHandlerList events = new EventHandlerList();

        // event objects
        static object EventPreProcessRequest = new object();
        static object EventPostProcessRequest = new object();

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Events"/> class.
        /// </summary>
        private Events() {}
        #endregion

        #region Execute events
        private static void ExecuteEventProcessEvent(object eventKey, MethodInfo method, ICustomAttribute[] attributes)
        {
            AjaxEventArgs e = new AjaxEventArgs(method, attributes);
            AjaxProcessEventHandler handler = events[eventKey] as AjaxProcessEventHandler;
            if (handler != null) {
                handler(e);
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Fires after an ajax request is executed.
        /// </summary>
        public static event AjaxProcessEventHandler PreProcessRequest {
            add { events.AddHandler(EventPreProcessRequest, value); }
            remove { events.RemoveHandler(EventPreProcessRequest, value); }
        }

        /// <summary>
        /// Fires before the ajax request is completed.
        /// </summary>
        public static event AjaxProcessEventHandler PostProcessRequest {
            add { events.AddHandler(EventPostProcessRequest, value); }
            remove { events.RemoveHandler(EventPostProcessRequest, value); }
        }
        #endregion

        /// <summary>
        /// Raises all BeforeProcessRequest events. These events are raised before a method is executed
        /// </summary>
        public static void BeforeProcessRequest(MethodInfo method, ICustomAttribute[] attributes)
        {
            Events.ExecuteEventProcessEvent(EventPreProcessRequest, method, attributes);
        }

        /// <summary>
        /// Raises all AfterProcessRequest events. These events ara raised after a method is executed
        /// </summary>
        public static void AfterProcessRequest(MethodInfo method, ICustomAttribute[] attributes)
        {
            Events.ExecuteEventProcessEvent(EventPostProcessRequest, method, attributes);
        }
    }
}
