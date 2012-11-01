using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.Web;
using Nohros.Concurrent;

namespace Nohros.RestQL
{
  /// <summary>
  /// 
  /// </summary>
  public class HttpHandler : IHttpAsyncHandler
  {
    const string kJsonContentType = "application/json";

    readonly Dictionary<int, string> pending_request_;

    #region .ctor
    public HttpHandler() {
      pending_request_ = new Dictionary<int, string>();
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

    public void EndProcessRequest(IAsyncResult result) {
      var future = (IFuture<HttpQueryResponse>) result;
      var context = (HttpContext) result.AsyncState;
      int id = context.GetHashCode();

      // Checks if the query request is still pending.
      if (!pending_request_.ContainsKey(id)) {
        return;
      }

      try {
        HttpResponse response = context.Response;
        HttpQueryResponse value = future.Get(0, TimeUnit.Seconds);
        response.StatusCode = (int) value.StatusCode;
        response.ContentType = kJsonContentType;
        response.Write(value.Response);
      } catch {
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        context.Response.ContentType = kJsonContentType;
      }
      pending_request_.Remove(id);
    }

    public IAsyncResult BeginProcessRequest(HttpContext context,
      AsyncCallback callback, object state) {
      string name = context.Request.QueryString["name"];
      if (string.IsNullOrEmpty(name)) {
        return Futures.ImmediateFuture(
          new HttpQueryResponse {
            Name = string.Empty,
            Response = string.Empty,
            StatusCode = HttpStatusCode.NotFound
          });
      }

      IDictionary<string, string> parameters = GetParameters(context);
      var app = context
        .Application[Strings.kApplicationKey] as HttpQueryApplication;
      IFuture<HttpQueryResponse> result = app.ProcessQuery(name, parameters,
        callback, context);


      // Waits the processing to finish if it was not finished yet.
      HttpQueryResponse response;
      if (result.TryGet(0, TimeUnit.Seconds, out response)) {
        callback(result);
      } else {
        pending_request_[context.GetHashCode()] = name;
        ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle,
          (o, @out) => Timeout(@out, callback, result), null,
          app.Settings.ResponseTimeout, true);
      }
      return result;
    }

    void Timeout(bool timedout, AsyncCallback callback, IAsyncResult result) {
      var context = (HttpContext) result.AsyncState;
      if (timedout) {
        pending_request_.Remove(context.GetHashCode());
        context.Response.StatusCode = (int) HttpStatusCode.RequestTimeout;
      }
      callback(result);
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
