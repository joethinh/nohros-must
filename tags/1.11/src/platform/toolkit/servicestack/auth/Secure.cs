using System;
using System.Web;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using Nohros.ServiceStack;

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
    /// <summary>
    /// Defines the default authentication challenge.
    /// </summary>
    public const string kWwwAuthenticateHeader = Strings.kWwwAuthenticateHeader;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SecureAttribute"/> class
    /// that applies to all HTTP methods.
    /// </summary>
    public SecureAttribute() : this(ApplyTo.All, kWwwAuthenticateHeader) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecureAttribute"/> class
    /// that applies to the HTTP  methods defined by the flag
    /// <paramref name="apply_to"/> and uses the default authentication
    /// challenge.
    /// </summary>
    /// <param name="apply_to">
    /// A flag that indicates for which method this attribute should be applied.
    /// </param>
    public SecureAttribute(ApplyTo apply_to)
      : this(apply_to, kWwwAuthenticateHeader) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecureAttribute"/> class
    /// that applies to the HTTP  methods defined by the flag
    /// <paramref name="apply_to"/> and uses the default authentication
    /// challenge.
    /// </summary>
    /// <param name="apply_to">
    /// A flag that indicates for which method this attribute should be applied.
    /// </param>
    /// <param name="challenge">
    /// The string that is used to challenge the client request. This string
    /// will be added to the WWW-Authenticate header when a subject is not
    /// authenticated.
    /// </param>
    public SecureAttribute(ApplyTo apply_to, string challenge) : base(apply_to) {
      Challenge = challenge ?? kWwwAuthenticateHeader;
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
        response.AddHeader(HttpHeaders.WwwAuthenticate, Challenge);
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

    /// <summary>
    /// Gets or sets a string that is used to challenge the client request.
    /// This string will be added to the WWW-Authenticate header when a subject
    /// is not authenticated.
    /// </summary>
    public string Challenge { get; set; }
  }
}
