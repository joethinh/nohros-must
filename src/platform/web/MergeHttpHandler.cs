using System;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

namespace Nohros.Net
{
    public class MergeHttpHandler : IHttpHandler
    {
        #region FileCacheItem class
        internal class FileCacheItem
        {
            public string Content;
            public DateTime DateEntered = DateTime.Now;

            public FileCacheItem(string content) {
                Content = content;
            }
        }
        #endregion

        private FileCacheItem UpdateFileCache(HttpContext context, string filePath, string cacheKey)
        {
            CacheDependency cd;
            // the merged content
            StringBuilder merged = new StringBuilder();
            // the merged files names
            List<string> files = new List<string>();

            NetSettings settings = Utility.Settings;
            
            // the real extension of the file. The content to
            // be merged and the HTML Content-Type will
            // will be defined by this extension.
            string ext = Path.GetExtension(filePath);

			//the embedded resource must be readed from the assembly.
			Assembly assembly = Assembly.GetExecutingAssembly();

            // The Nohros.Net core files will be merged only if the
            // base merge file starts with the "merge" word[iscore == true]
			if (settings != null && iscore)
			{
				switch (ext.ToLower())
				{
					case ".js":
#if DEBUG
						MergeEmbeddedResource(assembly, "jquery", "js", merged);
						MergeEmbeddedResource(assembly, "jqueryui", "js", merged);
						MergeEmbeddedResource(assembly, "core", "js", merged);
                        MergeEmbeddedResource(assembly, "ajax", "js", merged);
#else
                        MergeEmbeddedResource(assembly, "jquery", "js", merged);
						MergeEmbeddedResource(assembly, "jqueryui", "js", merged);
						MergeEmbeddedResource(assembly, "core", "js", merged);
                        MergeEmbeddedResource(assembly, "ajax", "js", merged);
#endif
                        // merge the user defined plugins
                        string[] mergedFiles = MergeGlobalResources(settings.PluginsPath, settings.Plugins, merged);

                        // add the merged files to the cache dependancy
                        files.AddRange(files);
                        break;
					case ".css":
						MergeEmbeddedResource(assembly, "csscore", "css", merged);
						break;
				}
			}

			string root = filePath.Substring(0, filePath.LastIndexOf("\\")+1);
			// Merging the user files
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string file = root + sr.ReadLine();
                        string content = ReadToEnd(file);

                        if (!string.IsNullOrEmpty(content))
                        {
                            merged.Append(content).Append("\r\n");
                            files.Add(file);
                        }
                    }
					sr.Close();
				}
				fs.Close();
			}

            FileCacheItem ci = new FileCacheItem(merged.ToString());

            // add the merge base file...
            files.Add(filePath);
            //...and the application settings file to the CacheDependency
            files.Add(Utility.BaseDirectory + "web.config");

            cd = new CacheDependency(files.ToArray());
            // Store the FileCacheItem in cache w/ a dependency on the merged files changing
            context.Cache.Insert(cacheKey, ci, cd);

            return ci;
        }

		void MergeEmbeddedResource(Assembly assembly, string resource, string extension, StringBuilder merged)
		{
			string _resource;
            NetSettings settings = Utility.Settings;
            if (settings != null)
			{
                _resource = settings[resource, KeyType.ScriptReplacement];
                if (_resource != null)
				{
					if (_resource.Length > 0 && _resource.StartsWith("~/"))
						_resource = _resource.Substring(2);

					// Get the content of the replaced file and merge it.
					string content = ReadToEnd(Utility.MapPath(_resource + "." + extension));
					if (!string.IsNullOrEmpty(content))
						merged.Append(content).Append("\r\n");

					return;
				}
			}

			Stream s = assembly.GetManifestResourceStream(Constant.AssemblyName + "." + resource + "." + extension);
			if (s != null)
			{
				StreamReader reader = new StreamReader(s);

				merged.Append(reader.ReadToEnd());
				merged.Append("\r\n");

				reader.Close();
			}
		}

        string ReadToEnd(string file_path)
        {   
            string content = null;
            string full_path = Path.GetFullPath(file_path);

            if (File.Exists(full_path))
            {
                using (FileStream file_stream = new FileStream(full_path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader file_reader = new StreamReader(full_path))
                    {
                        content = file_reader.ReadToEnd();
                        file_reader.Close();
					}
                    file_stream.Close();
				}
			}
            return content;
        }

        void GetImageResource(HttpContext context, string resource, string contentType)
        {
            string cacheKey = Constant.NetID + "." + resource;
            
            FileCacheItem ci = (FileCacheItem)context.Cache[cacheKey];
            if(ci == null)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream s = assembly.GetManifestResourceStream(Constant.AssemblyName + "." + resource);
                if (s != null)
                {
                    int num = (int)s.Length;
                    byte[] buffer = new byte[num];
                    // reads the entire file into the buffer...
                    s.Read(buffer, 0, num);
                    //... and save it to the cache.
                    ci = new FileCacheItem(Convert.ToBase64String(buffer));
                    context.Cache.Insert(cacheKey, ci);

                    s.Close();
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.End();
                }
            }

            context.Response.Cache.SetLastModified(ci.DateEntered);
            context.Response.ContentType = contentType;
            context.Response.BinaryWrite(Convert.FromBase64String(ci.Content));
            context.Response.End();
        }

        /// <summary>
        /// Merge a collection of files into a single one and output its content to the requestor.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> associated with the <see cref="Request"/></param>
        /// <param name="merge_index_path">The fully qualified path of the file that contains the name of the files to merge</param>
        /// <param name="key">A string that uniquely identifies the final merged file</param>
        /// <param name="content_type">The type of content that will be merged</param>
        void Merge(HttpContext context, string merge_index_path, string key, string content_type)
        {
            string modified_since = context.Request.Headers["If-Modified-Since"];
			string cache_key = Constant.NetID + key + content_type.Replace("text/", "");

            // If the file does not exists we can assume:
            //  . that the user is requesting only the library-defined resources.
            //  . that the user wants to debug the files.
            //  . that the file does not exists
            if (!File.Exists(merge_index_path))
            {
                string virtual_file_name = Path.GetFileNameWithoutExtension(merge_index_path);
                if (virtual_file_name == "library")
                {
                }
                else if (virtual_file_name == "debug")
                {
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.End();
                    return;
                }
            }

            FileCacheItem cached_file = (FileCacheItem)context.Cache[cache_key];

            if (cached_file != null)
            {
                if (modified_since != null)
                {
                    DateTime last_client_update;
                    DateTime.TryParseExact(context.Request.Headers["If-Modified-Since"], "yyyyMMdd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out last_client_update);

                    if (cached_file.DateEntered <= last_client_update)
                    {
                        // don't do anything, nothing has changed since last request.
                        context.Response.StatusCode = 304;
                        context.Response.StatusDescription = "Not Modified";
                        context.Response.End();
                        return;
                    }
                }
                else 
                {
                    // In the event that the browser does not automatically have this header, add it
                    context.Response.AddHeader("If-Modified-Since", cached_file.DateEntered.ToString("yyyyMMdd HH:mm:ss"));
                }
            }
            else
            {
                // cache item not found, update cache
                cached_file = UpdateFileCache(context, merge_index_path, cache_key);
            }

            context.Response.Cache.SetLastModified(cached_file.DateEntered);
            context.Response.ContentType = content_type;
            context.Response.Write(cached_file.Content);
            context.Response.End();
        }

        #region IHttpHandler
        /// <summary>
        /// Process the HTTP request.
        /// </summary>
        /// <param name="context">An HttpContext object that provides references to the intrinsic server objects
        /// used to service HTTP request</param>
        public void ProcessRequest(HttpContext context)
        {
            int position, last_position;
            char ch;
            string virtual_path = null,
                requested_path = null,
                requested_file = null,
                requested_file_extension = null,
                json_cache_key = null;

            // this handler is called when a file with the pattern "*/nohrosnet/[FILE-NAME].[EXTENSION].ashx
            // is requested by a client. This file will be used like a index to the files that
            // need to be merged. In order to get the real file name we need to do the follow steps:
            //     . Remove the .ashx "virtual extension"
            //     . Remove the /nohrosnet/ "virtual path"
            //
            virtual_path = context.Request.PhysicalPath.ToLower();
            last_position = position = virtual_path.Length - 5; // len(.ashx) = 5
            while (--position >= 0)
            {
                // searching for the patterns:
                //
                //  | 9 8 7 6 5 4 3 2 1 0                       |
                //  | n o h r o s n e t /[FILENAME].[EXTENSION] |
                //  ,
                //  | .extension.ashx |
                ch = virtual_path[position];
                switch(ch)
                {
                    case '.':
                        if (requested_file_extension == null) {
                            requested_file_extension = virtual_path.Substring(position + 1, last_position - position - 1);
                            last_position = position - 1;
                        }
                        break;

                    case '/':
                    case '\\':
                        position = position - 9;
                        if (virtual_path[position] != 'n' || virtual_path.Substring(position, 9) != "nohrosnet")
                            return; // the expected pattern was not found

                        requested_file = virtual_path.Substring(position + 10, last_position - position);
                        requested_path = virtual_path.Substring(0, position) + requested_file + "." + requested_file_extension;
                }
            }
            json_cache_key = requested_path;

            // The Content-Type will be extracted from the file extension
            switch (extension.ToLower())
            {
                case ".css":
                    Merge(context, requested_path, merged_file_key, "text/css");
                    break;

                case ".js":
                    Merge(context, requested_path, merged_file_key, "text/javascript");
                    break;

                case ".jpg":
                    GetImageResource(context, requested_path, "image/jpeg");
                    break;

                case ".png":
                    GetImageResource(context, requested_path, "image/x-png");
                    break;

                case ".gif":
                    GetImageResource(context, requested_path, "image/gif");
                    break;

                case ".swf":
                    GetImageResource(context, requested_path, "application/x-shockwave-flash");
                    break;

                default:
                    return;
            }
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
