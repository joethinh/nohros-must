using System;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Nohros.ServiceStack
{
  /// <summary>
  /// Provides a way to set Http Headers through attributes.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
    Inherited = true, AllowMultiple = true)]
  public class HttpHeaderAttribute : RequestFilterAttribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpHeaderAttribute"/>
    /// class that applies to all HTTP methods.
    /// </summary>
    public HttpHeaderAttribute() : this(ApplyTo.All) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpHeaderAttribute"/>
    /// class that applies to the HTTP  methods defined by the flag
    /// <paramref name="apply_to"/> and uses the default authentication
    /// challenge.
    /// </summary>
    /// <param name="apply_to">
    /// A flag that indicates for which method this attribute should be applied.
    /// </param>
    public HttpHeaderAttribute(ApplyTo apply_to) : base(apply_to) {
      Value = string.Empty;
    }

    /// <inheritdoc/>
    public override void Execute(IHttpRequest request, IHttpResponse response,
      object data) {
      if (!string.IsNullOrWhiteSpace(Name)) {
        response.AddHeader(Name, Value ?? string.Empty);
      }
    }

    /// <summary>
    /// Gets or sets the header name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the header value.
    /// </summary>
    public string Value { get; set; }
  }
}
