using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;

using Nohros.Collections;
using Nohros.Configuration;
using Nohros.Providers;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// 
  /// </summary>
  public class RestQLHttpHandler : IHttpHandler
  {
    const string kAppSettingsCacheKey = "app-settings-cache-key";
    const string kCommonSimpleProvidersConfigKey = "common";
    const string kTokenPrincipalMapperConfigKey = "token-principal-mapper";

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

      // the token information should be is always sent in the request body.
      string token = context.Request.Form["token"];

      // Since this code is usually called from a javascript, we should never
      // throw an exception; a error message shoud be sent as response when
      // a unexpected event occur.
      try {
        // map the token to a principal and resolve the query using the specified
        // principal.
        ITokenPrincipalMapper token_principal_mapper = GetTokenPrincipalMapper();
        if (token_principal_mapper != null) {
          token = token_principal_mapper.MapTokenToPrincipal(token);
        } else {
          token = settings.AnonymousToken;
        }

        if (token == null) {
          // The initiator request is not authenticated, lets try to resolve the
          // query using the "anonymous" token.
          token = settings.AnonymousToken;
        }
      } catch {
      }
    }

    /// <summary>
    /// Gets the cached version of the application settings our build a new
    /// one if the cached version does not exists.
    /// </summary>
    /// <param name="context">An <see cref="HttpCache"/> ibject related with
    /// the current request.</param>
    /// <returns>A <see cref="RestQLSettings"/> objects containing the current
    /// application settings.</returns>
    RestQLSettings GetSettings(HttpContext context) {
      RestQLSettings settings =
        context.Cache[kAppSettingsCacheKey] as RestQLSettings;

      if(settings == null) {
        settings = new RestQLSettings();
        settings.LoadAndWatch();

        // settings loading could be a expensive operation, lets cache it
        // for perfoemance reasons.
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