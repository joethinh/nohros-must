using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

using Nohros.Resources;

namespace Nohros
{
    /// <summary>
    /// Defines a internal class used to throw expections.
    /// The main purpose is to reduce code size.
    /// 
    /// The common way to throw exception generates quite a lot IL code and assembly code.
    /// Following is an example:
    /// <code>
    ///     C# source
    ///         throw new ArgumentNullException("key", Resources.ArgumentNull_Key)
    ///     IL code:
    ///         IL_0000:     ldstr   "key"
    ///         IL_0005:     call   Nohros.Resources::get_ArgumentNull_Key()
    ///         IL_000a:     newobj instance void System.ArgumentNullException::.ctor(string, string)
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
    ///         IL_0000:    ldc.i4.4
    ///         IL_0001:    ldc.i4.5    25
    ///         IL_0003:    call        void Nohros.Thrower::ThrowArgumentNullException(valurtype Nohros.ExceptionArgument, valuetype ExceptionResource)
    ///         IL_0008:    ret
    ///     which is 9 bytes in IL
    /// 
    /// This will also reduce the Jitted code size a lot.
    /// 
    /// 
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
            string argumentName = null;

            switch (argument)
            {
                case ExceptionArgument.index:
                    argumentName = "index";
                    break;

                case ExceptionArgument.key:
                    argumentName = "key";
                    break;

                case ExceptionArgument.obj:
                    argumentName = "obj";
                    break;

                case ExceptionArgument.arrayIndex:
                    argumentName = "arrayIndex";
                    break;

                case ExceptionArgument.comparer:
                    argumentName = "comparer";
                    break;

                case ExceptionArgument.array:
                    argumentName = "array";
                    break;

                case ExceptionArgument.name:
                    argumentName = "name";
                    break;

                case ExceptionArgument.principal:
                    argumentName = "principal";
                    break;

                case ExceptionArgument.provider:
                    argumentName = "provider";
                    break;

                case ExceptionArgument.type:
                    argumentName = "type";
                    break;
                
                case ExceptionArgument.value:
                    argumentName = "type";
                    break;

                case ExceptionArgument.collection:
                    argumentName = "collection";
                    break;

                case ExceptionArgument.any:
                    argumentName = "any variable";
                    break;

                default:
                    Debug.Assert(false , "The enum value is not defined, please checked ExceptionArgumentName Enum");
                    return string.Empty;
            }

            return argumentName;
        }

        /// <summary>
        /// Converts an <see cref="ExceptionResource"/> enum value to the resource string.
        /// </summary>
        /// <param name="resource">The <see cref="ExceptionResource"/> to convert</param>
        /// <returns>A string representation of the specified <see cref="ExceptionResource"/></returns>
        internal static string GetResourceByName(ExceptionResource resource)
        {
            switch(resource)
            {
                case ExceptionResource.Argument_AddingDuplicate:
                    return StringResources.Argument_AddingDuplicate;

                case ExceptionResource.Arg_ArrayPlusOffTooSmall:
                    return StringResources.Arg_ArrayPlusOffTooSmall;

                case ExceptionResource.Argument_Empty:
                    return StringResources.Argument_Empty;

                case ExceptionResource.DataProvider_ConnectionString:
                    return StringResources.DataProvider_ConnectionString;

                case ExceptionResource.DataProvider_CreateInstance:
                    return StringResources.DataProvider_CreateInstance;

                case ExceptionResource.DataProvider_Provider_Attributes:
                    return StringResources.DataProvider_Provider_Attributes;

                case ExceptionResource.DataProvider_InvalidProvider:
                    return StringResources.DataProvider_InvalidProvider;

                case ExceptionResource.ArgumentOutOfRange_Index:
                    return StringResources.ArgumentOutOfRange_Index;
                    
                case ExceptionResource.Argument_InvalidOfLen:
                    return StringResources.Argument_InvalidOfLen;

                case ExceptionResource.InvalidOperation_EmptyQueue:
                    return StringResources.InvalidOperation_EmptyQueue;

                case ExceptionResource.InvalidOperation_FullQueue:
                    return StringResources.InvalidOperation_FullQueue;

                default:
                    Debug.Assert(false, "The enum value is not defined, please checked ExceptionArgumentName Enum");
                    return string.Empty;
            }
        }

        #region Throws
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

        internal static void ThrowKeyNotFoundException()
        {
            throw new KeyNotFoundException();
        }

        internal static void ThrowProviderException(ExceptionResource resource)
        {
            throw new ProviderException(GetResourceByName(resource));
        }

        internal static void ThrowProviderException(ExceptionResource resource, System.Exception innerException)
        {
            throw new ProviderException(GetResourceByName(resource), innerException);
        }
        #endregion
    }

    internal enum ExceptionArgument
    {
        obj,
        index,
        arrayIndex,
        key,
        comparer,
        array,
        name,
        principal,
        provider,
        type,
        value,
        collection,
        any
    }

    internal enum ExceptionResource
    {
        Argument_AddingDuplicate = 0,
        Arg_ArrayPlusOffTooSmall = 1,
        Argument_Empty = 2,
        DataProvider_ConnectionString = 3,
        DataProvider_CreateInstance = 4,
        DataProvider_Provider_Attributes = 5,
        Caching_Invalid_expiration_combination = 6,
        DataProvider_InvalidProvider = 7,
        ArgumentOutOfRange_Index = 8,
        Argument_InvalidOfLen = 9,
        InvalidOperation_EmptyQueue = 10,
        InvalidOperation_FullQueue = 11
    }
}
