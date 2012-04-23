using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

using Nohros.Logging;
using Nohros.Resources;

namespace Nohros
{
  /// <summary>
  /// Defines a internal class used to throw expections.
  /// The main purpose is to reduce code size.
  /// 
  /// The common way to throw exception generates quite a lot IL code and
  /// assembly code. Following is an example:
  /// <code>
  ///     C# source
  ///         throw new ArgumentNullException("key", Resources.ArgumentNull_Key)
  ///     IL code:
  ///         IL_0000:     ldstr   "key"
  ///         IL_0005:     call   Nohros.Resources::get_ArgumentNull_Key()
  ///         IL_000a:     newobj instance_ void System.ArgumentNullException::.ctor(string, string)
  ///         IL_000f:     throw
  ///     which is 16 bytes in IL
  /// 
  /// So we want to get rid of the ldstr and call to Resources.get_xxx in IL.
  /// In Order to do that, two enums are created: ExceptionResource, ExceptionArgument to represent the
  /// argument name and resource in a small integer. The source code will be changed to
  /// 
  ///     C# source
  ///         Thrower.ThrowArgumentNullException(ExceptionArgument.key, ExceptionResource.ArgumentNull_Key);
  ///     IL code:m
  ///         IL_0000:    ldc.i4.0
  ///         IL_0001:    ldc.i4.0
  ///         IL_0002:    call        void Nohros.Thrower::ThrowArgumentNullException(valuetype Nohros.ExceptionArgument, valuetype ExceptionResource)
  ///         IL_0007:    ret
  ///     which is 8 bytes in IL
  /// 
  /// This will also reduce the Jitted code size a lot.
  /// 
  /// It is very important we do this for generic classes because we can easily
  /// generate the same code multiple times for diferrent instantiations.
  /// 
  /// Some methods produce the same Jited size as the common way to through
  /// exceptions and their exists here only to avoid duplicated code.
  /// </code>
  /// </summary>
  internal class Thrower
  {
    /// <summary>
    /// Converts an <see cref="ExceptionArgument"/> enum value to the argument name string
    /// </summary>
    /// <param name="argument">The <see cref="ExceptionArgument"/> to convert</param>
    /// <returns></returns>
    internal static string GetArgumentName(ExceptionArgument argument)
    {
      string argument_name = null;

      switch (argument)
      {
        case ExceptionArgument.index:
          argument_name = "index";
          break;

        case ExceptionArgument.key:
          argument_name = "key";
          break;

        case ExceptionArgument.obj:
          argument_name = "obj";
          break;

        case ExceptionArgument.arrayIndex:
          argument_name = "arrayIndex";
          break;

        case ExceptionArgument.comparer:
          argument_name = "comparer";
          break;

        case ExceptionArgument.array:
          argument_name = "array";
          break;

        case ExceptionArgument.name:
          argument_name = "name";
          break;

        case ExceptionArgument.provider:
          argument_name = "provider";
          break;

        case ExceptionArgument.type:
          argument_name = "type";
          break;

        case ExceptionArgument.value:
          argument_name = "type";
          break;

        case ExceptionArgument.collection:
          argument_name = "collection";
          break;

        case ExceptionArgument.any:
          argument_name = "any variable";
          break;

        case ExceptionArgument.capacity:
          argument_name = "capacity";
          break;

        case ExceptionArgument.loader:
          argument_name = "loader";
          break;

        case ExceptionArgument.duration:
          argument_name = "duration";
          break;

        case ExceptionArgument.state:
          argument_name = "state";
          break;

        case ExceptionArgument.runnable:
          argument_name = "runnable";
          break;

        case ExceptionArgument.executor:
          argument_name = "executor";
          break;

        case ExceptionArgument.builder:
          argument_name = "builder";
          break;

        default:
          Debug.Assert(false, "The enum value is not defined, please checked ExceptionArgumentName Enum");
          return string.Empty;
      }

      return argument_name;
    }

    /// <summary>
    /// Converts an <see cref="ExceptionResource"/> enum value to the resource string.
    /// </summary>
    /// <param name="resource">The <see cref="ExceptionResource"/> to convert</param>
    /// <returns>A string representation of the specified <see cref="ExceptionResource"/></returns>
    internal static string GetResourceByName(ExceptionResource resource)
    {
      switch (resource)
      {
        case ExceptionResource.Argument_AddingDuplicate:
          return StringResources.Argument_AddingDuplicate;

        case ExceptionResource.Arg_ArrayPlusOffTooSmall:
          return StringResources.Arg_ArrayPlusOffTooSmall;

        case ExceptionResource.Argument_Empty:
          return StringResources.Argument_Empty;

        case ExceptionResource.ArgumentOutOfRange_Index:
          return StringResources.ArgumentOutOfRange_Index;

        case ExceptionResource.Argument_InvalidOfLen:
          return StringResources.Argument_InvalidOfLen;

        default:
          Debug.Assert(false, "The enum value is not defined, please checked ExceptionArgumentName Enum");
          return string.Empty;
      }
    }

    #region size reduced methods
    internal static void ThrowArgumentException(ExceptionResource resource)
    {
      throw new ArgumentException(GetResourceByName(resource));
    }

    internal static void ThrowEmptyArgumentException(ExceptionArgument argument)
    {
      throw new ArgumentException(StringResources.Argument_Empty, GetArgumentName(argument));
    }

    internal static void ThrowInvalidOperationException(ExceptionResource resource)
    {
      throw new InvalidOperationException(GetResourceByName(resource));
    }

    internal static void ThrowArgumentNullException(ExceptionArgument argument)
    {
      throw new ArgumentNullException(GetArgumentName(argument));
    }

    internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
    {
      throw new ArgumentOutOfRangeException(GetArgumentName(argument));
    }

    internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
    {
      throw new ArgumentOutOfRangeException(GetArgumentName(argument), GetResourceByName(resource));
    }

    /// <summary>
    /// Throws a <see cref="ConfigurationErrorsException"/> using the <see cref="StringResources.Config_FileInvalid"/>
    /// like the exception message.
    /// </summary>
    internal static void ThrowConfigurationException_FileInvalid()
    {
      throw new ConfigurationErrorsException(StringResources.Config_FileInvalid);
    }
    #endregion

    #region non size reduced methods
    /// <summary>
    /// Throws a <see cref="ConfigurationErrorsException"/> and logs the exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="source">A string representing the exception source.</param>
    /// <remarks>
    /// The exception is logged by using the <see cref="FileLogger"/> class.
    /// </remarks>
    internal static void ThrowConfigurationException(string message, string source)
    {
      MustLogger.ForCurrentProcess.Error(source + "   " + message);
      throw new ConfigurationErrorsException(message);
    }

    /// <summary>
    /// Throws a <see cref="InvalidCastExeption"/>.
    /// </summary>
    /// <param name="typeA">The original type.</param>
    /// <param name="typeB">The type which the object
    /// <typeparamref name="typeA"/> was failed to cast.</param>
    internal static void ThrowInvalidCastException(string typeA, string typeB) {
      throw new InvalidCastException(string.Format(
        StringResources.Type_InvalidCastException, typeA, typeB));
    }

    /// <summary>
    /// Throws a <see cref="ProviderException"/>, using the specified resource name.
    /// </summary>
    internal static void ThrowProviderException(ExceptionResource resource)
    {
      throw new ProviderException(GetResourceByName(resource));
    }

    /// <summary>
    /// Throws a <see cref="ProviderException"/>, using the specified resource name and inner exception.
    /// </summary>
    internal static void ThrowProviderException(ExceptionResource resource, System.Exception inner_exception)
    {
      throw new ProviderException(GetResourceByName(resource), inner_exception);
    }
    #endregion
  }

  #region Enums
  /// <summary>
  /// The convention for this enum is using the argument name as the enum name.
  /// </summary>
  internal enum ExceptionArgument
  {
    obj,
    index,
    arrayIndex,
    key,
    comparer,
    array,
    name,
    provider,
    type,
    value,
    collection,
    any,
    capacity,
    loader,
    duration,
    state,
    runnable,
    executor,
    builder
  }

  /// <summary>
  /// The convention for this enum is using resource name as the enum name.
  /// </summary>
  internal enum ExceptionResource
  {
    Argument_AddingDuplicate = 0,
    Arg_ArrayPlusOffTooSmall = 1,
    Argument_Empty = 2,
    Caching_Invalid_expiration_combination = 6,
    ArgumentOutOfRange_Index = 8,
    Argument_InvalidOfLen = 9
  }
  #endregion
}
