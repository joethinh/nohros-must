using System;
using System.Net;

namespace Nohros.RestQL
{
  public struct HttpQueryResponse
  {
    public string Name { get; set; }

    public string Response { get; set; }

    public HttpStatusCode StatusCode { get; set; }
  }
}
