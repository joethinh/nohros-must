using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// 
  /// </summary>
  public class HttpHandler : IHttpHandler
  {
    #region IHttpHandler Members
    /// <summary>
    /// Process the HTTP request.
    /// </summary>
    /// <param name="context">
    /// As <see cref="HttpContext"/> object that provides references to the
    /// intrinsic server objects used to service HTTP requests.
    /// </param>
    public void ProcessRequest(HttpContext context) {
      // the token information should be is always sent in the request body.
      string name = context.Request.QueryString["name"];
      if (string.IsNullOrEmpty(name)) {
        ProcessResponse(context, HttpStatusCode.NotFound, string.Empty);
      }

      // Since this code is usually called from a javascript, we should never
      // throw an exception; a error message shoud be sent as response when
      // a unexpected event occur.
      try {
        // Map the token to a principal and resolve the query using the
        // specified principal.
        QueryServer server = Global.QueryServer;

        // Attempt to process the query using the supplied parameters.
        IQueryProcessor processor = server.QueryProcessor;

        string result;
        IDictionary<string, string> data = GetRequestData(context);
        HttpStatusCode status_code = processor.Process(name, data, out result);

        // Set up the response and send it to the caller.
        ProcessResponse(context, status_code, result);
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
    #endregion

    IDictionary<string, string> GetRequestData(HttpContext context) {
      HttpRequest request = context.Request;
      Dictionary<string, string> data = new Dictionary<string, string>();
      foreach (KeyValuePair<string, string> pair in request.QueryString) {
        data[pair.Key] = pair.Value;
      }

      if (context.Request.HttpMethod == "POST") {
        foreach (KeyValuePair<string, string> pair in request.Form) {
          data[pair.Key] = pair.Value;
        }
      }
      return data;
    }

    void ProcessResponse(HttpContext context, HttpStatusCode status,
      string result) {
      HttpResponse response = context.Response;
      response.ContentType = "application/json";
      response.StatusCode = (int) status;
      response.Write(response);
    }
  }
}
