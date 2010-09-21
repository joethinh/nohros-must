using System;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

using Nohros;
using Nohros.Data;
using Nohros.Configuration;

namespace Nohros.Net
{
    /// <summary>
    /// This handler is used to handle files with the pattern "*.[CONTENT-GROUP-NAME].nohrosnet.[js|css|...]
    /// The [CONTENT-GROUP-NAME] part of the requested file name must be a name of a content group
    /// defined into the application configuration file for the requested HTTP content-type.
    /// </summary>
    public class MergeHttpHandler : IHttpHandler
    {
        #region FileCacheItem class
        /// <summary>
        /// A cached version of the merged content group.
        /// </summary>
        internal class FileCacheItem
        {
            public string Content;
            public DateTime DateEntered = DateTime.Now;

            public FileCacheItem(string content) {
                Content = content;
            }
        }
        #endregion

        const int kResourceNotFoundCode = 404;
        const string kVirtualFilePathSuffix = ".nohrosnet.";
        const string kVirtualFilePathExtension = ".ashx";
        const string kKeyPrefix = "merger-";

#if DEBUG
        const string kBuildVersion = "release";
#else
        const string kBuildVersion = "debug";
#endif

        HttpContext context_;
        NetSettings settings_;

        private FileCacheItem UpdateFileCache(ContentGroupNode content_group, string cache_key)
        {
            CacheDependency cd;
            StringBuilder merged = new StringBuilder();
            List<string> merged_files = new List<string>(content_group.Files.Count);

			// the embedded resource must be readed from the resource assembly.
			Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (string file_name in content_group.Files) {
                string file_path = Path.Combine(content_group.BasePath, file_name);

                // sanity check file existence
                if (File.Exists(file_path)) {
                    using (StreamReader stream = new StreamReader(file_path)) {
                        string content = stream.ReadToEnd().Trim();
                        if (content.Length > 0) {
                            merged.Append(content).Append("\r\n");
                            merged_files.Add(file_path);
                        }
                        stream.Close();
                    }
                }
            }

            if (merged.Length > 0) {
                FileCacheItem ci = new FileCacheItem(merged.ToString());

                // add the the application configuration file to the CacheDependency
                merged_files.Add(Utility.BaseDirectory + "web.config");
                cd = new CacheDependency(merged_files.ToArray());

                // Store the FileCacheItem in cache w/ a dependency on the merged files changing
                context_.Cache.Insert(cache_key, ci, cd);

                return ci;
            }

            return null;
        }

        /// <summary>
        /// Merges a collection of files into a single one and output its content to the requestor.
        /// </summary>
        /// <param name="files_to_merge">The files</param>
        /// <param name="content_group_name">The name of the content group.</param>
        /// <param name="content_type">The MIME type of content that will be merged.</param>
        void GetMergedContent(string content_group_name, string mime_type)
        {
            string modified_since = context_.Request.Headers["If-Modified-Since"];
            string cache_key = string.Concat(kKeyPrefix,
                content_group_name, ".",
                mime_type, ".",
                kBuildVersion);

            // check if this group is defined
            ContentGroupNode content_group = settings_.GetContentGroup(content_group_name, kBuildVersion, mime_type);
            if (content_group == null) {
                context_.Response.StatusCode = kResourceNotFoundCode;
                context_.Response.End();
                return;
            }

            // attempt to retrieve the merged content from the cache
            FileCacheItem cached_file = (FileCacheItem)context_.Cache[cache_key];
            if (cached_file != null) {
                if (modified_since != null) {
                    DateTime last_client_update;
                    DateTime.TryParseExact(context_.Request.Headers["If-Modified-Since"], "yyyyMMdd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out last_client_update);

                    if (cached_file.DateEntered <= last_client_update) {
                        // don't do anything, nothing has changed since last request.
                        context_.Response.StatusCode = 304;
                        context_.Response.StatusDescription = "Not Modified";
                        context_.Response.End();
                        return;
                    }
                }
                else {
                    // In the event that the browser does not automatically have this header, add it
                    context_.Response.AddHeader("If-Modified-Since", cached_file.DateEntered.ToString("yyyyMMdd HH:mm:ss"));
                }
            }
            else {
                // cache item not found, update cache
                cached_file = UpdateFileCache(content_group, cache_key);
                if (cached_file == null) {
                    // the files to merge are not found
                    // TODO: log the exception
                    context_.Response.StatusCode = kResourceNotFoundCode;
                    context_.Response.End();
                }
            }

            context_.Response.Cache.SetLastModified(cached_file.DateEntered);
            context_.Response.ContentType = mime_type;
            context_.Response.Write(cached_file.Content);
            context_.Response.End();
        }

        #region IHttpHandler
        /// <summary>
        /// Process the HTTP request.
        /// </summary>
        /// <param name="context">An HttpContext object that provides references to the intrinsic server objects
        /// used to service HTTP request</param>
        public void ProcessRequest(HttpContext context)
        {
            string content_type;
            string content_group_name;
            string requested_file_path;
            string requested_file_extentsion;
            int position;

            context_ = context;
            content_type = "text/html";

            try {
                settings_ = NetSettings.ForCurrentProcess;
            } catch (System.Configuration.ConfigurationErrorsException) {
                // TODO: Log the exception
                // the configuration file was not defined. we cant do nothing.
                context.Response.StatusCode = kResourceNotFoundCode;
                context.Response.End();
                return;
            }

            requested_file_path = context.Request.FilePath;

            // attempt to get the content type from the requested file estension
            requested_file_extentsion = Path.GetExtension(requested_file_path);
            switch (requested_file_extentsion) {
                case ".js":
                    content_type = "application/x-javascript";
                    break;

                case ".css":
                    content_type = "text/css";
                    break;

                default:
                    context.Response.StatusCode = kResourceNotFoundCode;
                    context.Response.End();
                    break;
            }

            // attempt to get the content group name from the virtual path.
            position = requested_file_path.IndexOf(kVirtualFilePathSuffix);
            if(position == -1) {
                // the handler is configured incorrectly.
                // TODO: log this unexpected behavior.
                context_.Response.StatusCode = kResourceNotFoundCode;
                context_.Response.End();
                return;
            }

            content_group_name = Path.GetFileNameWithoutExtension(
                requested_file_path.Substring(0, position));

            GetMergedContent(content_group_name, content_type);
        }
        
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// <remarks>
        /// Our <see cref="IHttpHandler"/> implementation doesn't do expensive initializations. So, the
        /// value returned by this property doesn't really matter(since simple object allocation is fairly inexpensive).
        /// </remarks>
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        #endregion
    }
}
