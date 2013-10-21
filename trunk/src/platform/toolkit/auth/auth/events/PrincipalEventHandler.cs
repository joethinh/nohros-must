using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Represents the method that handles events associated with a
  /// <see cref="IPrincipal"/> object.
  /// </summary>
  /// <param name="principal">
  /// The <see cref="IPrincipal"/> associated with the event.
  /// </param>
  public delegate void PrincipalEventHandler(IPrincipal principal);
}
