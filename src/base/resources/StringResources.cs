using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Threading;
using System.Globalization;
using System.Reflection;

namespace Nohros.Resources
{
    public sealed class StringResources
    {
        ResourceManager lib_resources_;
        static StringResources loader;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringResources"/> class.
        /// </summary>
        internal StringResources()
        {
            lib_resources_ = new ResourceManager("Nohros.Resources.Resources", base.GetType().Assembly);
        }

        static StringResources Loader
        {
            get
            {
                if (loader == null) {
                    Interlocked.CompareExchange<StringResources>(ref loader, new StringResources(), null);
                }
                return loader;
            }
        }

        /// <summary>
        /// Returns the value of the specified <see cref="String"/> resource.
        /// </summary>
        /// <param name="name">The name of the resource to get.</param>
        /// <returns>The value of the resource localized for the caller's current culture settings.
        /// If a match is not possible, null is returned.</returns>
        public static string GetString(string name)
        {
            StringResources loader = Loader;
            if (loader == null)
                return null;

            return loader.lib_resources_.GetString(name, Culture);
        }

        /// <summary>
        /// Returns the value of the specified <see cref="String"/> resource and replaces
        /// the format item in the requested resource string with a string representation of a corresponding
        /// object in the specified array.
        /// </summary>
        /// <param name="name">The name of the resource to get.</param>
        /// <param name="args">An object array that contains zero or more objects</param>
        /// <returns>The value of the resource localized for the caller's current culture settings.
        /// If a match is not possible, null is returned.</returns>
        /// <remarks>The <see cref="string.Format()"/> method will be used to replace the
        /// format items in the requested resource value</remarks> and idependant on the behavior of
        /// that method.
        public static string GetString(string name, params object[] args)
        {
            StringResources loader = Loader;
            if (loader == null)
                return null;

            string format = loader.lib_resources_.GetString(name, Culture);
            if ((args == null) || (args.Length <= 0))
                return format;

            for (int i = 0, j = args.Length; i < j; i++)
            {
                string str2 = args[i] as string;
                if ((str2 != null) && (str2.Length > 0x400))
                    args[i] = str2.Substring(0, 0x3fd) + "...";
            }
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Gets the <see cref="CultureInfo"/> object that represents the culture which resource is localized.
        /// </summary>
        static CultureInfo Culture
        {
            get
            {
                // Forces the use of the current thread's CurrentUICulture property.
                return null;
            }
        }

        #region Internal lib_resources_

        /// <summary>
        /// Looks up a localized string similar to [An entry with the same key already exists]
        /// </summary>
        internal static string Argument_AddingDuplicate
        {
            get { return GetString("Argument_AddingDuplicate"); }
        }

        #region DataProvider
        /// <summary>
        /// Looks up a localized string similar to [Instance of the requested data provider could not be
        /// created. Check the constructor implied by the IDataProvider interface.]
        /// </summary>
        internal static string DataProvider_CreateInstance
        {
            get { return GetString("DataProvider_CreateInstance"); }
        }

        internal static string DataProvider_LoadAssembly
        {
            get { return GetString("DataProvider_LoadAssembly"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [The Provider is invalid.]
        /// </summary>
        internal static string DataProvider_InvalidProvider
        {
            get { return GetString("DataProvider_InvalidProvider"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [An invalid attributes collection has been supplied,
        /// or a required attribute has not been supplied].
        /// </summary>
        internal static string DataProvider_Provider_Attributes
        {
            get { return GetString("DataProvider_Provider_Attributes"); }
        }
        #endregion

        #region Auth
        /// <summary>
        /// Looks up a localized string similar to [A instance of the login module type {0} could not be created].
        /// </summary>
        internal static string Auth_LoginMudule_Type
        {
            get { return GetString("Auth_LoginMudule_Type"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Type was not defined].
        /// </summary>
        internal static string Auth_Config_Missing_LoginModuleType
        {
            get { return GetString("Auth_Config_Missing_LoginModuleType"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Invalid control flag].
        /// </summary>
        internal static string Auth_Config_InvalidControlFlag
        {
            get { return GetString("Auth_Config_InvalidControlFlag"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Invalid system type].
        /// </summary>
        internal static string Auth_Config_InvalidModuleType
        {
            get { return GetString("Auth_Config_InvalidModuleType"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Control flag was not defined].
        /// </summary>
        internal static string Auth_Config_Missing_ControlFlag
        {
            get { return GetString("Auth_Config_Missing_ControlFlag"); }
        }
        #endregion

        /// <summary>
        /// Looks up a localized string similar to [absoluteExpiration must be DateTime.MaxValue or slidingExpiration
        /// must be timeSpan.Zero].
        /// </summary>
        internal static string Caching_Invalid_expiration_combination
        {
            get { return GetString("Caching_Invalid_expiration_combination"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Syntax error].
        /// </summary>
        internal static string Generic_SyntaxError
        {
            get { return GetString("Generic_SyntaxError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [The number of specified fields is less than the number of columns.].
        /// </summary>
        internal static string DataHelper_OrdArrInvalidOfLen
        {
            get { return GetString("DataHelper_OrdArrInvalidOfLen"); }
        }

        #region JSON
        /// <summary>
        ///   Looks up a localized string similar to [Root value must be an array or object].
        /// </summary>
        internal static string JSON_BadRootElementType
        {
            get { return GetString("JSON_BadRootElementType"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Invalid escape sequence].
        /// </summary>
        internal static string JSON_InvalidEscape
        {
            get { return GetString("JSON_InvalidEscape"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Too much nesting].
        /// </summary>
        internal static string JSON_TooMuchNesting
        {
            get { return GetString("JSON_TooMuchNesting"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Trailing comma not allowed].
        /// </summary>
        internal static string JSON_TrailingComma
        {
            get { return GetString("JSON_TrailingComma"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Unexpected data after root element].
        /// </summary>
        internal static string JSON_UnexpectedDataAfterRoot
        {
            get { return GetString("JSON_UnexpectedDataAfterRoot"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to [Dictionary keys must be quoted].
        /// </summary>
        internal static string JSON_UnquotedDictionaryKey
        {
            get { return GetString("JSON_UnquotedDictionaryKey"); }
        }
        #endregion

        #endregion

        #region Public lib_resources_

        /// <summary>
        /// Looks up a localized string similar to [An invalid connection string argument has been supplied,
        /// or a required connection string argument has not been supplied
        /// </summary>
        public static string DataProvider_ConnectionString
        {
            get { return GetString("DataProvider_ConnectionString"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [An IDbConnection instance could not be created. Verify
        /// if your connection string and data source type were correctly specified.]
        /// </summary>
        public static string DataProvider_Connection
        {
            get { return GetString("DataProvider_Connection"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Index was out of range. Must be non-negative and less than the
        /// size of the collection].
        /// </summary>
        public static string ArgumentOutOfRange_Index
        {
            get { return GetString("ArgumentOutOfRange_Index"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Destination array is not long enough to copy all the items in
        /// the collection. Check array index and length].
        /// </summary>
        public static string Arg_ArrayPlusOffTooSmall
        {
            get { return GetString("Arg_ArrayPlusOffTooSmall"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [{0} is empty].
        /// </summary>
        public static string Argument_Empty
        {
            get { return GetString("Argument_Empty"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Offset and length were out of bounds for the array or count is
        /// greater than the number of elements from index to the end of the source collection].
        /// </summary>
        public static string Argument_InvalidOfLen
        {
            get { return GetString("Argument_InvalidOfLen"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [index is outside the range of valid indexes for array].
        /// </summary>
        public static string Arg_IndexOutOfRange
        {
            get { return GetString("Arg_IndexOutOfRange"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Not a valid value].
        /// </summary>
        public static string Arg_OutOfRange {
            get { return GetString("Arg_OutOfRange"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [The range of valid values are {0} to {1}].
        /// </summary>
        public static string Arg_RangeNotBetween {
            get { return GetString("Arg_RangeNotBetween"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Unable to find the specified configuration file].
        /// </summary>
        public static string Config_FileNotFound
        {
            get { return GetString("Config_FileNotFound"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Unable to find the file "{0}"].
        /// </summary>
        public static string Config_FileNotFound_Path
        {
            get { return GetString("Config_FileNotFound_Path"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [The configuration key "{0}" was not found.].
        /// </summary>
        public static string Config_KeyNotFound
        {
            get { return GetString("Config_KeyNotFound"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [The "{0}" path must be relative to the application base directory.]
        /// </summary>
        public static string Config_PathIsRooted {
            get { return GetString("Config_PathIsRooted"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [The specified configuration object is not valid.]
        /// </summary>
        public static string Config_InvalidObject {
            get { return GetString("Config_InvalidObject"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Configuration file is invalid.].
        /// </summary>
        public static string Config_FileInvalid {
            get { return GetString("Config_FileInvalid"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Missing configuration information for {0} at {1}.].
        /// </summary>
        public static string Config_MissingAt {
            get { return GetString("Config_MissingAt"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [{0} is not a valid value for {1}.].
        /// </summary>
        public static string Config_ArgOutOfRange {
            get { return GetString("Config_ArgOutOfRange"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Configuration error at "{0}".].
        /// </summary>
        public static string Config_ErrorAt
        {
            get { return GetString("Config_ErrorAt"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Queue empty].
        /// </summary>
        public static string InvalidOperation_EmptyQueue
        {
            get { return GetString("InvalidOperation_EmptyQueue"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Queue full].
        /// </summary>
        public static string InvalidOperation_FullQueue
        {
            get { return GetString("InvalidOperation_FullQueue"); }
        }

        /// <summary>
        /// Looks up a localized string similar to [Instance of the requested type could not be created.
        /// Check the constructor implied by the {0}.].
        /// </summary>
        public static string Type_CreateInstanceOf {
            get { return GetString("Type_CreateInstanceOf"); }
        }
        #endregion
    }
}