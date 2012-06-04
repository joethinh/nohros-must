using System;
using System.Web;
using System.Web.Caching;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// 
  /// </summary>
  public class HttpHandler : IHttpHandler
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
      // the token information should be is always sent in the request body.
      string token = context.Request.Form["token"] ?? string.Empty;

      // Since this code is usually called from a javascript, we should never
      // throw an exception; a error message shoud be sent as response when
      // a unexpected event occur.
      try {
        // map the token to a principal and resolve the query using the
        // specified principal.
        QueryServer server = Global.QueryServer;
        ITokenPrincipalMapper token_principal_mapper =
          server.TokenPrincipalMapper;
        token = (token_principal_mapper != null)
          ? token_principal_mapper.MapTokenToPrincipal(token)
          : server.TokenPrincipalMapperSettings.AnonymousToken;
      } catch {
      }
    }

    /// <summary>
    /// Gets a value indicationg if this object could be pooled.
    /// </summary>
    public bool IsReusable {
      get { return true; }
    }
  }
}