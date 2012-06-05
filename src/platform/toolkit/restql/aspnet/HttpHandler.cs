using System;
using System.Net;
using System.Web;
using System.Web.Caching;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// 
  /// </summary>
  public class HttpHandler : IHttpHandler
  {
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
        // Map the token to a principal and resolve the query using the
        // specified principal.
        QueryServer server = Global.QueryServer;
        ITokenPrincipalMapper token_principal_mapper =
          server.TokenPrincipalMapper;
        token = (token_principal_mapper != null)
          ? token_principal_mapper.MapTokenToPrincipal(token)
          : server.TokenPrincipalMapperSettings.AnonymousToken;

        // Attempt to process the query using the supplied parameters.
        IQueryProcessor processor = server.QueryProcessor;
        string result;
        HttpStatusCode status_code =
          processor.Process(context.Request.QueryString, out result);

        // Set up the response and send it to the caller.
        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)status_code;
        response.Write(response);
      } catch {
        // TODO(neylor.silva) Find the right HttpStatusCode to return.
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