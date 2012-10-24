using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// 
  /// </summary>
  public class HttpHandler : IHttpAsyncHandler
  {
    const string kJsonContentType = "application/json";

    #region .ctor
    public HttpHandler() {
    }
    #endregion

    public virtual void ProcessRequest(HttpContext context) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Gets a value indicationg if this object could be pooled.
    /// </summary>
    public bool IsReusable {
      get { return false; }
    }

    public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object state) {
    }

    public void EndProcessRequest(IAsyncResult result) {
    }

    /// <summary>
    /// Process the HTTP request.
    /// </summary>
    /// <param name="context">
    /// As <see cref="HttpContext"/> object that provides references to the
    /// intrinsic server objects used to service HTTP requests.
    /// </param>
    public void ProcessRequestAsync(HttpContext context) {
      IAsyncResult IHttpAsyncHandler.Begin
      HttpResponse response = context.Response;
      string name = context.Request.QueryString["name"];
      if (string.IsNullOrEmpty(name)) {
        response.ContentType = kJsonContentType;
        response.StatusCode = (int) HttpStatusCode.NotFound;
        response.End();
        return;
      }

      HttpApplicationState state = context.Application;
      IDictionary<string, string> parameters = GetParameters(context);
      var app = (HttpQueryApplication) state[Strings.kApplicationKey];

      string result;
      response.StatusCode = (int) app.ProcessQuery(name, parameters, out result);
      response.ContentType = kJsonContentType;
      response.Write(result);
    }

    void OnQueryResponse(string name, HttpStatusCode status, string result,
      object state) {
      var response = (HttpResponse) state;
      response.ContentType = kJsonContentType;
      response.Write(result);
    }

    IDictionary<string, string> GetParameters(HttpContext context) {
      HttpRequest request = context.Request;
      var data = new Dictionary<string, string>();
      NameValueCollection query_string = request.QueryString;
      string[] keys = query_string.AllKeys;
      for (int i = 0, j = keys.Length; i < j; i++) {
        data[keys[i]] = query_string[i];
      }

      NameValueCollection form = request.QueryString;
      keys = form.AllKeys;
      for (int i = 0, j = keys.Length; i < j; i++) {
        data[keys[i]] = form[i];
      }
      return data;
    }
  }
}
