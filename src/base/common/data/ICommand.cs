using System;

namespace Nohros.Data
{
  /// <summary>
  /// Provides functionality to execute queries that retunrs no value against
  /// a specific data provider.
  /// </summary>
  public interface ICommand
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    void Execute();
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// one parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    void Execute(T arg1);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// two parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// three parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2, in T3>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    /// <param name="arg3">
    /// The type of the third parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2, T3 arg3);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// four parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2, in T3, in T4>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    /// <param name="arg3">
    /// The type of the third parameter of the query.
    /// </param>
    /// <param name="arg4">
    /// The type of the fourth parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2, T3 arg3, T4 arg4);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// five parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2, in T3, in T4, in T5>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    /// <param name="arg3">
    /// The type of the third parameter of the query.
    /// </param>
    /// <param name="arg4">
    /// The type of the fourth parameter of the query.
    /// </param>
    /// <param name="arg5">
    /// The type of the fifth parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// six parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2, in T3, in T4, in T5, in T6>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    /// <param name="arg3">
    /// The type of the third parameter of the query.
    /// </param>
    /// <param name="arg4">
    /// The type of the fourth parameter of the query.
    /// </param>
    /// <param name="arg5">
    /// The type of the fifth parameter of the query.
    /// </param>
    /// <param name="arg6">
    /// The type of the sixth parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// seven parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2, in T3, in T4, in T5, in T6, in T7>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    /// <param name="arg3">
    /// The type of the third parameter of the query.
    /// </param>
    /// <param name="arg4">
    /// The type of the fourth parameter of the query.
    /// </param>
    /// <param name="arg5">
    /// The type of the fifth parameter of the query.
    /// </param>
    /// <param name="arg6">
    /// The type of the sixth parameter of the query.
    /// </param>
    /// <param name="arg7">
    /// The type of the seventh parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// eight parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2, in T3, in T4, in T5, in T6, in T7,
                            in T8>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    /// <param name="arg3">
    /// The type of the third parameter of the query.
    /// </param>
    /// <param name="arg4">
    /// The type of the fourth parameter of the query.
    /// </param>
    /// <param name="arg5">
    /// The type of the fifth parameter of the query.
    /// </param>
    /// <param name="arg6">
    /// The type of the sixth parameter of the query.
    /// </param>
    /// <param name="arg7">
    /// The type of the seventh parameter of the query.
    /// </param>
    /// <param name="arg8">
    /// The type of the eighth parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7,
      T8 arg8);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// nine parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2, in T3, in T4, in T5, in T6, in T7,
                            in T8, in T9>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    /// <param name="arg3">
    /// The type of the third parameter of the query.
    /// </param>
    /// <param name="arg4">
    /// The type of the fourth parameter of the query.
    /// </param>
    /// <param name="arg5">
    /// The type of the fifth parameter of the query.
    /// </param>
    /// <param name="arg6">
    /// The type of the sixth parameter of the query.
    /// </param>
    /// <param name="arg7">
    /// The type of the seventh parameter of the query.
    /// </param>
    /// <param name="arg8">
    /// The type of the eighth parameter of the query.
    /// </param>
    /// <param name="arg9">
    /// The type of the ninth parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7,
      T8 arg8, T9 arg9);
  }

  /// <summary>
  /// Provides functionality to execute queries that retunrs no value and has
  /// ten parameter against a specific data provider.
  /// </summary>
  public interface ICommand<in T, in T2, in T3, in T4, in T5, in T6, in T7,
                            in T8, in T9, in T10>
  {
    /// <summary>
    /// Executes the command represented by the
    /// <see cref="ICommand{T}"/> object.
    /// </summary>
    /// <param name="arg1">
    /// The type of the first parameter of the query.
    /// </param>
    /// <param name="arg2">
    /// The type of the second parameter of the query.
    /// </param>
    /// <param name="arg3">
    /// The type of the third parameter of the query.
    /// </param>
    /// <param name="arg4">
    /// The type of the fourth parameter of the query.
    /// </param>
    /// <param name="arg5">
    /// The type of the fifth parameter of the query.
    /// </param>
    /// <param name="arg6">
    /// The type of the sixth parameter of the query.
    /// </param>
    /// <param name="arg7">
    /// The type of the seventh parameter of the query.
    /// </param>
    /// <param name="arg8">
    /// The type of the eighth parameter of the query.
    /// </param>
    /// <param name="arg9">
    /// The type of the ninth parameter of the query.
    /// </param>
    /// <param name="arg10">
    /// The type of the tenth parameter of the query.
    /// </param>
    void Execute(T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7,
      T8 arg8, T9 arg9, T10 arg10);
  }
}
