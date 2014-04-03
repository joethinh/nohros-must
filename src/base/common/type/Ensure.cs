using System;

namespace Nohros
{
  /// <summary>
  /// A collection of useful methods to eval preconditions.
  /// </summary>
  public static class Ensure
  {
    public static void NotNull<T>(T argument, string argument_name)
      where T : class {
      if (argument == null)
        throw new ArgumentNullException(argument_name);
    }

    public static void NotNullOrEmpty(string argument, string argument_name) {
      if (string.IsNullOrEmpty(argument))
        throw new ArgumentNullException(argument, argument_name);
    }

    public static void Positive(int number, string argument_name) {
      if (number <= 0)
        throw new ArgumentOutOfRangeException(argument_name,
          argument_name + " should be positive.");
    }

    public static void Positive(long number, string argument_name) {
      if (number <= 0)
        throw new ArgumentOutOfRangeException(argument_name,
          argument_name + " should be positive.");
    }

    public static void NonNegative(long number, string argument_name) {
      if (number < 0)
        throw new ArgumentOutOfRangeException(argument_name,
          argument_name + " should be non negative.");
    }

    public static void NonNegative(int number, string argument_name) {
      if (number < 0)
        throw new ArgumentOutOfRangeException(argument_name,
          argument_name + " should be non negative.");
    }

    public static void NotEmptyGuid(Guid guid, string argument_name) {
      if (Guid.Empty == guid)
        throw new ArgumentException(argument_name,
          argument_name + " shoud be non-empty GUID.");
    }
  }
}
