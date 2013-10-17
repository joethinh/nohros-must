using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using NUnit.Framework;
using Nohros;
using Nohros.Caching.Providers;
using Nohros.Configuration;
using Nohros.Security.Auth;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace web
{
  public class HttpAuthenticationManagerTests
  {
    class Cache : ICacheProvider
    {
      readonly IDictionary<string, object> cache_;

      #region .ctor
      public Cache() {
        cache_ = new Dictionary<string, object>();
      }
      #endregion

      public T Get<T>(string key) {
        T t;
        Get(key, out t);
        return t;
      }

      public bool Get<T>(string key, out T item) {
        object obj;
        if (cache_.TryGetValue(key, out obj)) {
          item = (T) obj;
          return true;
        }
        item = default(T);
        return false;
      }

      public void Set(string key, object value) {
        cache_[key] = value;
      }

      public void Set(string key, object value, long duration, TimeUnit unit) {
        cache_[key] = value;
      }

      public void Add(string key, object value) {
        cache_[key] = value;
      }

      public void Add(string key, object value, long duration, TimeUnit unit) {
        cache_[key] = value;
      }

      public void Remove(string key) {
        cache_.Remove(key);
      }

      public void Clear() {
        cache_.Clear();
      }
    }

    ICacheProvider cache_;

    IAuthCallbackHandler callback_;
    ILoginModuleFactory login_module_factory_;
    ILoginModule module_;
    IDictionary<string, string> options_;
    IDictionary<string, string> shared_;
    ISubject subject_;

    [SetUp]
    public void SetUp() {
      login_module_factory_ = Mock.Create<ILoginModuleFactory>();
      callback_ = new NopAuthCallbackHandler();
      shared_ = new Dictionary<string, string>();
      options_ = new Dictionary<string, string>();

      module_ = Mock.Create<ILoginModule>();
      Mock
        .Arrange(() => module_.Login())
        .Returns(true);
      Mock
        .Arrange(() => module_.Commit())
        .Returns(true);
      Mock
        .Arrange(() => module_.ControlFlag)
        .Returns(LoginModuleControlFlag.Required);

      Mock
        .Arrange(() =>
          login_module_factory_.CreateLoginModule(callback_, shared_, options_))
        .Returns(module_);

      cache_ = new Cache();
      subject_ = Mock.Create<ISubject>();
      Mock
        .Arrange(() => subject_.Permissions)
        .Returns(new PermissionSet());
      Mock
        .Arrange(() => subject_.Principals)
        .Returns(new PrincipalSet());
    }

    [Test]
    public void
      should_create_cookie_and_put_subject_on_cache_when_authentication_succeed() {
      var context =
        new LoginContext(new[] {
          new KeyValuePair<ILoginModuleFactory, IDictionary<string, string>>(
            login_module_factory_, options_)
        });
      var manager = new HttpAuthenticationManager(context, cache_);
      var http_response = new HttpResponse(new StringWriter());
      var http_context =
        new HttpContext(new HttpRequest("index.html", "http://nohros.com",
          string.Empty), http_response);
      var cookies_count = http_response.Cookies.Count;
      var authenticated = manager.Authenticate(subject_, callback_, http_context);

      ISubject subject;
      manager.GetSubject(http_context, out subject);

      Assert.That(authenticated, Is.True);
      Assert.That(subject, Is.SameAs(subject_));
      Assert.That(http_response.Cookies.Count, Is.GreaterThan(cookies_count));
    }

    [Test]
    public void should_not_touch_cookies_or_cache_when_authentication_fails() {
      Mock
        .Arrange(() => module_.Login())
        .Returns(false);

      var context =
        new LoginContext(new[] {
          new KeyValuePair<ILoginModuleFactory, IDictionary<string, string>>(
            login_module_factory_, options_)
        });
      var manager = new HttpAuthenticationManager(context, cache_);
      var http_response = new HttpResponse(new StringWriter());
      var http_context =
        new HttpContext(new HttpRequest("index.html", "http://nohros.com",
          string.Empty), http_response);
      var cookies_count = http_response.Cookies.Count;
      var authenticated = manager.Authenticate(subject_, callback_, http_context);

      ISubject subject;
      manager.GetSubject(http_context, out subject);

      Assert.That(authenticated, Is.False);
      Assert.That(subject, Is.Null);
      Assert.That(http_response.Cookies.Count, Is.EqualTo(cookies_count));
    }

    [Test]
    public void should_delete_cookies_and_cache_entries_on_signout() {
      var context =
        new LoginContext(new[] {
          new KeyValuePair<ILoginModuleFactory, IDictionary<string, string>>(
            login_module_factory_, options_)
        });
      var manager = new HttpAuthenticationManager(context, cache_);
      var http_response = new HttpResponse(new StringWriter());
      var http_context =
        new HttpContext(new HttpRequest("index.html", "http://nohros.com",
          string.Empty), http_response);

      var cookies_count = http_response.Cookies.Count;
      var authenticated = manager.Authenticate(subject_, callback_, http_context);
      ISubject subject;
      manager.GetSubject(http_context, out subject);

      Assert.That(authenticated, Is.True);
      Assert.That(subject, Is.Not.Null);
      Assert.That(http_response.Cookies.Count, Is.GreaterThan(cookies_count));

      manager.SignOut(http_context);
      manager.GetSubject(http_context, out subject);

      Assert.That(subject, Is.Null, "Subject should be removed from cache");
      Assert.That(http_response.Cookies.Count, Is.EqualTo(cookies_count),
        "Authentication cookies should be removed");
    }
  }
}
