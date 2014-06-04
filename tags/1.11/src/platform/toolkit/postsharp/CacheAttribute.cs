using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Caching.Providers;
using PostSharp.Aspects;

namespace Nohros.PostSharp
{
  [Serializable]
  public class CacheAttribute : MethodInterceptionAspect
  {
    public static Func<ICacheProvider>
      CacheProviderFactory = () => {
        throw new InvalidOperationException(
          "You should set the value of the CacheAttribute.CacheProviderFactory in order to use CacheAttribute Aspect.");
      };

    [NonSerialized] static ICacheProvider cache_;

    string method_name_;

    public CacheAttribute() {
      Lifetime = TimeSpan.MaxValue;
    }

    public CacheAttribute(TimeSpan lifetime) {
      Lifetime = lifetime;
    }

    public override void RuntimeInitialize(System.Reflection.MethodBase method) {
      cache_ = CacheProviderFactory();
    }

    public override void CompileTimeInitialize(
      System.Reflection.MethodBase method, AspectInfo aspect_info) {
      method_name_ = method.DeclaringType.FullName + method.Name;
    }

    public override void OnInvoke(MethodInterceptionArgs args) {
      var key = GetCacheKeyForArgs(args.Arguments);
      
      object obj;
      if (cache_.Get(key, out obj)) {
        args.ReturnValue = obj;
      } else {
        var @return = args.Invoke(args.Arguments);
        args.ReturnValue = @return;
        cache_.Set(key, @return, (long) Lifetime.TotalSeconds, TimeUnit.Seconds);
      }
    }

    string GetCacheKeyForArgs(Arguments arguments) {
      var sb = new StringBuilder();
      sb.Append(method_name_);
      foreach (var argument in arguments) {
        sb.Append(argument == null ? "::" : argument.ToString());
      }
      return sb.ToString();
    }

    public TimeSpan Lifetime { get; set; }
  }
}
