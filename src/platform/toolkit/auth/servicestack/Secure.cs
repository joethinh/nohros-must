using System;
using System.Web;
using Nohros.Security.Auth;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Nohros.Security.Auth.ServiceStack
{
  /// <summary>
  /// Indicates that the request, which is associated with this attribute,
  /// requires the existence of a <see cref="ISubject"/>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
    Inherited = true, AllowMultiple = true)]
  public class SecureAttribute : RequestFilterAttribute
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SecureAttribute"/> class
    /// that applies to all Http methods by using the given
    /// <see cref="HttpAuthenticationManager"/> object.
    /// </summary>
    public SecureAttribute()
      : base(ApplyTo.All) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecureAttribute"/> class
    /// by using the given <see cref="HttpAuthenticationManager"/> object and
    /// method restriction.
    /// </summary>
    /// <param name="apply_to">
    /// A flag that indicates for which method this attribute should be applied.
    /// </param>
    public SecureAttribute(ApplyTo apply_to) : base(apply_to) {
    }
    #endregion

    /// <inheritdoc/>
    public override void Execute(IHttpRequest request, IHttpResponse response,
      object data) {
      HttpRequest aspnet_request = (HttpRequest) request.OriginalRequest;
      HttpContextBase context_base = aspnet_request.RequestContext.HttpContext;
      HttpContext context = context_base.ApplicationInstance.Context;
      ISubject subject;
      if (!AuthenticationManager.GetSubject(context, out subject)) {
        response.AddHeader(HttpHeaders.WwwAuthenticate,
          Strings.kWwwAuthenticateHeader);
        throw HttpError.Unauthorized(Resources.Request_Unauthorized);
      }
    }

    /// <summary>
    /// Gets or sets a <see cref="HttpAuthenticationManager"/> object that
    /// can be used to check if a <see cref="ISubject"/> is associated with
    /// an HTTP request.
    /// </summary>
    /// <remarks>
    /// There is no way to pass an instance object to an attribute, so we will
    /// rely on the IOC property injector feature to get a instance of the
    /// <see cref="HttpAuthenticationManager"/>.
    /// </remarks>
    public HttpAuthenticationManager AuthenticationManager { get; set; }
  }
}
