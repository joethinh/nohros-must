using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Nohros.Logging;
using Nohros.Resources;

namespace Nohros
{
  /// <summary>
  /// A collection of factory methods used to dynamically create instances of
  /// other classes.
  /// </summary>
  /// <remarks>
  /// This class is basically a simplification of the <see cref="Activator"/>
  /// class.
  /// </remarks>
  /// <typeparam name="T">
  /// The type to be created. It is usually a interface or a derived class of
  /// the type defined by a <see cref="IRuntimeType"/> object. The type that
  /// is loaded is the type defined by the <see cref="IRuntimeType"/> object,
  /// which is casted to <typeparamref name="T"/> before it is loaded.
  /// </typeparam>
  public sealed class RuntimeTypeFactory<T> where T : class
  {
    /// <summary>
    /// Creates a new instance of the type defined by the
    /// <paramref name="runtime_type"/> object.
    /// </summary>
    /// <param name="runtime_type">
    /// A <see cref="IRuntimeType"/> object containing  information about
    /// the type to be instantiated.
    /// </param>
    /// <param name="args">
    /// An array of arguments that match the parameters of the constructor to
    /// invoke. If args is an empty array or null, the constructor that takes
    /// no parameters(the default constructor) is invoked.
    /// </param>
    /// <returns>
    /// An instance of the type defined by the <paramref name="runtime_type"/>
    /// object casted to <typeparamref name="T"/> or <c>null</c> if the class
    /// could not be loaded or casted to <typeparamref name="T"/>
    /// </returns>
    /// <remarks>
    /// A exception is never raised by this method. If a exception is raised
    /// by the object constructor it will be catched and <c>null</c> will be
    /// returned. If you need to know about the exception, use the method
    /// <see cref="CreateInstance"/> instead.
    /// </remarks>
    /// <seealso cref="CreateInstance"/>
    /// <seealso cref="IRuntimeType"/>
    public static T CreateInstanceNoException(IRuntimeType runtime_type,
      params object[] args) {
      // A try/catch block is used here because this method should not throw
      // any exception.
      try {
        return CreateInstance(runtime_type, args);
      } catch (Exception exception) {
        MustLogger.ForCurrentProcess.Error(
          string.Format(StringResources.TypeLoad_CreateInstance,
            runtime_type.Type));
      }
      return default(T);
    }

    /// <summary>
    /// Creates a new instance of the type defined by the
    /// <paramref name="runtime_type"/> object and the specified
    /// arguments, falling back to the default class constructor.
    /// </summary>
    /// <param name="runtime_type">
    /// A <see cref="IRuntimeType"/> object containing  information about
    /// the type to be instantiated.
    /// </param>
    /// <param name="args">
    /// An array of arguments that match the parameters of the constructor to
    /// invoke. If args is an empty array or null, the constructor that takes
    /// no parameters(the default constructor) is invoked.
    /// </param>
    /// <returns>
    /// An instance of the type defined by the <paramref name="runtime_type"/>
    /// object casted to <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// If a constructor that match the specified array of arguments is not
    /// found, this method try to create an instance of the type defined by
    /// the <paramref name="runtime_type"/> object using the constructor that
    /// takes no parameters(the default constructor).
    /// </remarks>
    /// <seealso cref="CreateInstance"/>
    /// <seealso cref="IRuntimeType"/>
    public static T CreateInstanceFallback(IRuntimeType runtime_type,
      params object[] args) {
      return CreateInstance(runtime_type, true, args);
    }

    /// <summary>
    /// Creates a new instance of the type defined by the
    /// <paramref name="runtime_type"/> object and the specified arguments.
    /// </summary>
    /// <param name="runtime_type">
    /// A <see cref="IRuntimeType"/> object containing  information about
    /// the type to be instantiated.
    /// </param>
    /// <param name="args">
    /// An array of arguments that match the parameters of the constructor to
    /// invoke. If args is an empty array or null, the constructor that takes
    /// no parameters(the default constructor) is invoked.
    /// </param>
    /// <returns>
    /// An instance of the type defined by the <paramref name="runtime_type"/>
    /// object casted to <typeparamref name="T"/>.
    /// </returns>
    public static T CreateInstance(IRuntimeType runtime_type,
      params object[] args) {
      return CreateInstance(runtime_type, false, args);
    }

    static T CreateInstance(IRuntimeType runtime_type, bool fallback,
      params object[] args) {
      // create a new object instance using a public or non-public
      // constructor.
      const BindingFlags kFlags =
        BindingFlags.CreateInstance | BindingFlags.Public |
          BindingFlags.Instance | BindingFlags.NonPublic;

      T new_obj = null;
      Type type = RuntimeType.GetSystemType(runtime_type);
      if (type != null) {
        try {
          // If no constructors arguments was specified or if we do not need
          // to fallback do not waste time searching for a matching constructor.
          if (args.Length == 0) {
            new_obj =
              Activator.CreateInstance(type, kFlags, null, args, null) as T;
          } else {
            KeyValuePair<ConstructorInfo, ParameterInfo[]>[] list =
              GetPossibleMatchingConstructors(type, kFlags, args.Length);
            if (list.Length > 0) {
              KeyValuePair<ConstructorInfo, object[]> ctor;
              if (TryGetMatchingConstructor(list, args, out ctor)) {
                new_obj = ctor.Key.Invoke(kFlags, null, ctor.Value,
                  CultureInfo.CurrentCulture) as T;
              }
            }
          }
        } catch (MissingMethodException) {
        }

        if (new_obj == null && fallback) {
          new_obj = Activator.CreateInstance(type, true) as T;
        }
      }

      if (new_obj == null) {
        throw new TypeLoadException(
          string.Format(Resources.Resources.TypeLoad_CreateInstance,
            runtime_type.Type));
      }
      return new_obj;
    }

    static bool TryGetMatchingConstructor(
      KeyValuePair<ConstructorInfo, ParameterInfo[]>[] ctors, object[] args,
      out KeyValuePair<ConstructorInfo, object[]> matched_ctor) {
      // Copy the arguments into a array that we can modify.
      object[] mutable_args = new object[args.Length];
      Array.Copy(args, mutable_args, args.Length);

      // This list will hold the arguments of the matched constructor in
      // the same order as the related parameters.
      List<object> ordered_args = new List<object>(args.Length);
      for (int i = 0, j = ctors.Length; i < j; i++) {
        ParameterInfo[] parms = ctors[i].Value;
        for (int k = 0, m = parms.Length; k < m; k++) {
          int pos = GetArgumentOfType(mutable_args, parms[k].ParameterType);
          if (pos == -1) {
            ordered_args.Clear();
            break;
          }
          ordered_args.Add(mutable_args[pos]);
          mutable_args[pos] = null;
        }
        if (ordered_args.Count > 0) {
          matched_ctor =
            new KeyValuePair<ConstructorInfo, object[]>(ctors[i].Key,
              ordered_args.ToArray());
          return true;
        }
      }
      matched_ctor = default(KeyValuePair<ConstructorInfo, object[]>);
      return false;
    }

    static int GetArgumentOfType(object[] args, Type type) {
      for (int i = 0, j = args.Length; i < j; i++) {
        object arg = args[i];
        if (arg != null && arg.GetType() == type) {
          return i;
        }
      }
      return -1;
    }

    static KeyValuePair<ConstructorInfo, ParameterInfo[]>[]
      GetPossibleMatchingConstructors(Type type, BindingFlags flags, int count) {
      List<KeyValuePair<ConstructorInfo, ParameterInfo[]>> list =
        new List<KeyValuePair<ConstructorInfo, ParameterInfo[]>>();
      ConstructorInfo[] ctors = type.GetConstructors(flags);
      foreach (ConstructorInfo ctor in ctors) {
        ParameterInfo[] parms = ctor.GetParameters();
        if (parms.Length == count) {
          list.Add(new KeyValuePair<ConstructorInfo, ParameterInfo[]>(ctor,
            parms));
        }
      }
      return list.ToArray();
    }
  }
}
