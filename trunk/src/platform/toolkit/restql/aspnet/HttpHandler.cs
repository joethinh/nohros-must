using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using ZMQ;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// 
  /// </summary>
  public class HttpHandler : IHttpHandler
  {
    const string kJsonContentType = "application/json";

    /// <summary>
    /// Process the HTTP request.
    /// </summary>
    /// <param name="context">
    /// As <see cref="HttpContext"/> object that provides references to the
    /// intrinsic server objects used to service HTTP requests.
    /// </param>
    public void ProcessRequest(HttpContext context) {
      HttpResponse response = context.Response;
      string name = context.Request.QueryString["name"];
      if (string.IsNullOrEmpty(name)) {
        response.ContentType = kJsonContentType;
        response.StatusCode = (int) HttpStatusCode.NotFound;
        response.End();
        return;
      }

      HttpApplicationState app = context.Application;
      var settings = (Settings) app[Strings.kSettingsKey];
      var socket = (Socket) app[Strings.kSocketKey];
      socket.Connect(Transport.TCP, settings.QueryServerAddress);

      IDictionary<string, string> parameters = GetParameters(context);
      var processor = new HttpQueryProcessor(socket);

      string result;
      response.StatusCode = (int) processor
        .Process(name, parameters, settings.ResponseTimeout, out result);
      response.ContentType = kJsonContentType;
      response.Write(result);
      response.End();
    }

    /// <summary>
    /// Gets a value indicationg if this object could be pooled.
    /// </summary>
    public bool IsReusable {
      get { return true; }
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
