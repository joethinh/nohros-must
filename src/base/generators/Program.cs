using System;

namespace Nohros.Generators
{
  public sealed class Program
  {
    public static void Main(string[] args) {
      Generator generator = new Generator(CommandLine.ForCurrentProcess);
      generator.Generate();
    }
  }
}
