using System;
using Nohros.Caching.Providers;

namespace Nohros.Security.Auth
{
  public class AuthenticationManager
  {
    readonly ICacheProvider cache_;
    readonly LoginContext login_context_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationManager"/>
    /// class by using the given <see cref="LoginContext"/> and
    /// <see cref="ICacheProvider"/>.
    /// </summary>
    /// <param name="login_context">
    /// A <see cref="LoginContext"/> object that can be used to authenticate
    /// subjects.
    /// </param>
    /// <param name="cache">
    /// A <see cref="ICacheProvider"/> that can be used to store authenticated
    /// subjects.
    /// </param>
    public AuthenticationManager(LoginContext login_context,
      ICacheProvider cache) {
      if (login_context == null || cache == null) {
        throw new ArgumentNullException(login_context == null
          ? "login_context"
          : "cache");
      }
      login_context_ = login_context;
      cache_ = cache;
    }
    #endregion

    /// <summary>
    /// Authenticates a given subject using the given
    /// <see cref="IAuthCallbackHandler"/> and associate it with a token.
    /// </summary>
    /// <param name="subject">
    /// The subject to be authenticated.
    /// </param>
    /// <param name="callback">
    /// A <see cref="IAuthCallbackHandler"/> object that can be used to get
    /// authentication information for the user.
    /// </param>
    /// <returns>
    /// A <see cref="AuthenticationToken"/> object containing information
    /// about the success or failure of the authentication process.
    /// </returns>
    public virtual AuthenticationToken Authenticate(ISubject subject,
      IAuthCallbackHandler callback) {
      if (subject == null || callback == null) {
        throw new ArgumentNullException(callback == null
          ? "callback"
          : "context");
      }

      if (!login_context_.Login(subject, callback)) {
        return new AuthenticationToken(string.Empty, false);
      }

      var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

      cache_.Add(token, subject, (long) TokenExpiration.TotalSeconds,
        TimeUnit.Seconds);

      return new AuthenticationToken(token, true);
    }

    /// <summary>
    /// Logs the use out and invalidates the authentication token.
    /// </summary>
    /// <returns>
    /// <c>true</c> if a logged <see cref="ISubject"/> is successfully logged
    /// out and its associated token invalidated.
    /// </returns>
    public virtual void SignOut(string token) {
      cache_.Remove(token);
    }

    /// <summary>
    /// Gets the <see cref="ISubject"/> that is associated with the given
    /// <paramref name="token"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of subject to return.
    /// </typeparam>
    /// <param name="subject">
    /// When this method return, contains the subject that is associated with
    /// the given <paramref name="token"/>.
    /// </param>
    /// <param name="token">
    /// A string representing the token associated with the subject to get.
    /// </param>
    /// <returns>
    /// <c>true</c> if a <see cref="ISubject"/> associated with the given
    /// <paramref name="token"/> is found and the token is not expired;
    /// otherwise, <c>false</c>.
    /// </returns>
    public virtual bool GetSubject<T>(string token, out T subject)
      where T : ISubject {
      if (token == null) {
        throw new ArgumentNullException("token");
      }
      return cache_.Get(token, out subject);
    }

    /// <summary>
    /// Gets or set a value indicating how long a subject authentication token
    /// should be valid.
    /// </summary>
    public TimeSpan TokenExpiration { get; set; }
  }
}
