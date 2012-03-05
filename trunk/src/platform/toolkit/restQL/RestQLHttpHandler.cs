using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// 
  /// </summary>
  public class RestQLHttpHandler : IHttpHandler
  {
    const string kAppSettingsCacheKey = "app-settings-cache-key";
    string kTokenQueryString = "token";

    /// <summary>
    /// Process the HTTP request.
    /// </summary>
    /// <param name="context">
    /// As <see cref="HttpContext"/> object that provides references to the
    /// intrinsic server objects used to service HTTP requests.
    /// </param>
    public void ProcessRequest(HttpContext context) {
      RestQLSettings settings = GetSettings(context);

      NameValueCollection query_string = context.Request.QueryString;

      // Gets the request token, if it is not defined means that the request
      // initiator is not authenticated.
      string token = query_string[kTokenQueryString];
      if(token == null) {
        // The initiator request is not authenticated, lets try to resolve the
        // query using the "anonymous" token.
        token = settings.AnonymousToken;
      }
    }

    RestQLSettings GetSettings(HttpContext context) {
      RestQLSettings settings =
        context.Cache[kAppSettingsCacheKey] as RestQLSettings;

      if(settings == null) {
        settings = new RestQLSettings();
        settings.LoadAndWatch();

        context.Cache.Add(kAppSettingsCacheKey, settings, null,
                          DateTime.MaxValue, Cache.NoSlidingExpiration,
                          CacheItemPriority.Normal, null);
      }
      return settings;
    }

    /// <summary>
    /// Gets a value indicationg if this object could be pooled.
    /// </summary>
    public bool IsReusable {
      get { return true; }
    }
  }
}