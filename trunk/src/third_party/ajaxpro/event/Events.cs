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

    public class Events
    {
        #region private members
        private static EventHandlerList events = new EventHandlerList();
        #endregion

        #region Event keys
        private static object EventPreProcessRequest = new object();
        private static object EventPostProcessRequest = new object();
        #endregion

        #region .ctor
        private Events() {}
        #endregion

        #region Ajax processing

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
        /// Fires after an ajax request is executed
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

        #endregion
    }
}
