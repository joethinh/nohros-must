using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.Caching;

namespace Nohros.Net
{
    [Serializable]
    public class Sevens : ICloneable
    {
        Queue<string> sevens_;
        int capacity_ = 4;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the Sevens class that is empty and has the default initial capacity.
        /// </summary>
        public Sevens():this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance_ of the Sevens class by using the specified capacity
        /// </summary>
        /// <param name="capacity">The initial number of events that the Sevens can contain</param>
        public Sevens(int capacity)
        {
            capacity_ = capacity;
            sevens_ = new Queue<string>(capacity);
        }
        #endregion

        #region ICloneable
        /// <summary>
        /// Creates a deep copy of the Sevens.
        /// </summary>
        /// <returns>A deep copy of the Sevens</returns>
        public object Clone()
        {
            Sevens clone = new Sevens(sevens_.Count);

            string[] sevens = sevens_.ToArray();
            for(int i=0,j=sevens.Length;i<j;i++) {
                clone.sevens_.Enqueue(sevens[i]);
            }
            return clone;
        }
        #endregion

        /// <summary>
        /// Removes all events from the Sevens.
        /// </summary>
        public void Clear()
        {
            sevens_.Clear();
        }

        /// <summary>
        /// Merges the specified Sevens object into the current Sevens object.
        /// </summary>
        /// <param name="response">The Sevens object to be merged into the current Sevens object</param>
        public Sevens Merge(Sevens sevens)
        {
            if (sevens == null)
                throw new ArgumentNullException("sevens");

            if( this.Equals(sevens) )
                return this;

            string[] sevens_str = sevens_.ToArray();
            for (int i = 0, j = sevens_str.Length; i < j; i++) {
                sevens_.Enqueue(sevens_str[i]);
            }

            return this;
        }

        /// <summary>
        /// Outputs the server events into a javascript object.
        /// </summary>
        public override string ToString()
        {
            int num_events = Count;
            StringBuilder sb = new StringBuilder();

            //if (!string.IsNullOrEmpty(this.error))
                //return "{\"length\":0, \"error\" :\"" + this.error + "\"}";

            if (num_events == 0)
                return "{\"length\":0, \"error\":null}";

            sb.Append("{\"length\":\"").Append(num_events).Append("\", \"error\":null,");
            sb.Append("\"actions\" : [");

            // append the server events
            while (sevens_.Count > 0)
                sb.Append(sevens_.Dequeue()).Append(",");

            // remove the extra comma and close the array
            // of events and the events container object.
            sb.Remove(sb.Length - 1, 1).Append("]}");

            return sb.ToString();
        }

        #region Server events

        #region Client contents
        /// <summary>
        /// Append content to he inside of every matched element
        /// </summary>
        /// <param name="event_name">The name of the event</param>
        /// <param name="selector">The element selector in jquery-style</param>
        /// <param name="markup">The HTML content to append</param>
        protected void ClientContent(string event_name, string selector, string markup, params KeyValuePair<string, string>[] pairs)
        {
            if (string.IsNullOrEmpty(selector) || string.IsNullOrEmpty(markup))
                throw new ArgumentNullException("Arguments could not be null");

            string json;
            json = string.Concat(
                "{\"type\":\"", event_name,
                "\",\"selector\":\"", selector);

            if (pairs != null) {
                for (int i = 0, j = pairs.Length; i < j; i++) {
                    KeyValuePair<string, string> pair = pairs[i];
                    json += string.Concat("\"", pair.Key, "\":\"", pair.Value, "\",");
                }
            }

            json += "\"markup\":\"" + markup + "\"}";
        }

        /// <summary>
        /// Append content to he inside of every matched element
        /// </summary>
        public void AppendMarkup(string selector, string markup)
        {
            ClientContent("appendmkp", selector, markup);
        }

        /// <summary>
        /// Prepend content to he inside of every matched element
        /// </summary>
        public void PrependMarkup(string selector, string markup)
        {
            ClientContent("prependmkp", selector, markup);
        }

        public void AfterMarkup(string selector, string markup)
        {
            ClientContent("aftermkp", selector, markup);
        }

        public void BeforeMarkup(string selector, string markup)
        {
            ClientContent("beforemkp", selector, markup);
        }

        public void ReplaceMarkup(string selector, string markup)
        {
            ClientContent("replacemkp", selector, markup);
        }

        public void SetValue(string selector, string value)
        {
            ClientContent("setval", selector, value);
        }

        public void SetText(string selector, string value)
        {
            ClientContent("settext", selector, value);
        }

        public void SetHtml(string selector, string value)
        {
            ClientContent("sethtml", selector, value);
        }

        public void FillTable(string selector, string value, bool inner)
        {
            FillTable(selector, value, null, inner);
        }

        public void FillTable(string selector, string value, string constant, bool inner)
        {
            ClientContent((inner) ? "fillintable" : "filltable", selector, value, new KeyValuePair<string, string>("constant", constant));
        }
        #endregion

        public void Debugger()
        {
            sevens_.Enqueue("{\"type\":\"debugger\", \"selector\":\"debugger\"}");
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="set"></param>
        public void Focus(string selector, bool set)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            if (selector.Length == 0)
                throw new ArgumentOutOfRangeException("selector");

            sevens_.Enqueue(
                string.Concat(
                 "{\"type\":\"focus\",",
                 "\"selector\":\"", selector,
                 "\", \"set\":\"", set.ToString().ToLower(), "\"}")
                 );
        }

        /// <summary>
        /// Redirects a browser to the specified URL.
        /// </summary>
        /// <param name="URL">The URL to redirects the browser</param>
        public void Redirect(string URL)
        {
            if (URL == null)
                throw new ArgumentNullException("URL");

            sevens_.Enqueue(
                string.Concat(
                    "{\"type\":\"redirect\",",
                    "\"selector\":\"none\",",
                    "\"url\":\"", URL, "\"}")
                    );
        }

        /// <summary>
        /// Simulates a mouse click on a reset button for the specified form.
        /// </summary>
        /// <param name="selector">A string containing a selector expression</param>
        public void ResetForm(string selector)
        {
            if (string.IsNullOrEmpty(selector))
                throw new ArgumentNullException("Arguments could not be null");

            sevens_.Enqueue(
                string.Concat(
                    "{\"type\":\"resetfrm\",",
                    "\"selector\":\"", selector, "\"}")
                    );
        }

        /// <summary>
        /// Set a attribute for the set of matched elements.
        /// </summary>
        /// <param name="selector">A string containing a selector expression</param>
        /// <param name="attribute">The name of the attribute to set</param>
        /// <param name="value">A value to set for the attribute</param>
        public void SetAttribute(string selector, string attribute, string value)
        {
            if (selector == null || attribute == null || value == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.any);

            sevens_.Enqueue(
                string.Concat(
                    "{\"type\":\"setattr\",",
                    "\"selector\":\"", selector, "\",",
                    "\"attribute\":\"", attribute, "\",",
                    "\"value\":\"", value, "\"}")
                    );
        }

        /// <summary>
        /// Set one or more CSS properties_ for the set of matched elements.
        /// </summary>
        /// <param name="selector">A string conaining a selector expression</param>
        /// <param name="cssattr">A CSS property name</param>
        /// <param name="value">A value to set for the property</param>
        public void SetCss(string selector, string property_name, string value)
		{
            if (selector == null || property_name == null || value == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.any);

            sevens_.Enqueue(
                string.Concat(
                    "{\"type\":\"setcss\",",
                    "\"selector\":\"", selector, "\",",
                    "\"cssattr\":\"", property_name, "\",",
                    "\"value\":\"", value, "\"}")
                    );
		}

        /// <summary>
        /// Evaluates a expression after a specified number of milliseconds has elapsed.
        /// </summary>
        /// <param name="timeout">A long that specifies the number of milliseconds</param>
        /// <param name="functionToCall">A javascript statement to execute</param>
        public void SetTimeout(long ms, string function_to_call)
        {
            if (function_to_call == null)
                throw new ArgumentNullException("function_to_call");

            if (ms < 0)
                ms = 5000;

            sevens_.Enqueue(
                string.Concat(
                    "{\"type\":\"settimeout\",",
                    "\timeout\":\"", ms, "\",",
                    "\functionToCall\":\"", function_to_call, "\"}")
                    );
        }

		public void ShowInfo(string title, string message)
		{
			if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(message))
				throw new ArgumentNullException("Arguments could not be null");

			// Sets the attribute of this class
			//
			//SerializeType("showinfo");

			// Append the parameters that will be used by the client
			//SerializePair("title", title);
			//SerializeLastPair("message", message);
		}

        /// <summary>
        /// Trigger an event on every matched element
        /// </summary>
        /// <param name="selector">The name of the objects to trigger the event</param>
        /// <param name="evt">An event type to trigger</param>
        public void Trigger(string selector, string evt)
        {
            if (selector == null || evt == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.any);

            sevens_.Enqueue(
                string.Concat(
                    "{\"type\":\"trigger\",",
                    "\"selector\":\"", selector, "\",",
                    "\"evt\":\"", evt, "\"}")
                    );
        }

        #endregion

        /// <summary>
        /// Gets the number of events contained in the Sevens.
        /// </summary>
		public int Count
		{
            get { return sevens_.Count; }
		}
    }
}
