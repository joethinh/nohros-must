using System;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Represents the method that handles events associated with a
  /// <see cref="IPermission"/> object.
  /// </summary>
  /// <param name="permission">
  /// The <see cref="IPermission"/> associated with the event.
  /// </param>
  public delegate void PermissionEventHandler(IPermission permission);
}
