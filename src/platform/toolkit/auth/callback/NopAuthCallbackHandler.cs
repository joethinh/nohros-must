using System;

namespace Nohros.Security.Auth
{ 
  /// <summary>
  /// A implementation of the <see cref="IAuthCallbackHandler"/> class that
  /// performs no operation.
  /// </summary>
  public class NopAuthCallbackHandler : IAuthCallbackHandler
  {
    public void Handle(IAuthCallback[] callback) {
    }
  }
}
