using System;
using System.Diagnostics;

namespace Nohros.Generators
{
  public sealed class Program
  {
    public static void Main(string[] args) {
      CommandLine switches = CommandLine.ForCurrentProcess;
      if (switches.HasSwitch("debug")) {
        Debugger.Launch();
      }

      Generator generator = new Generator(CommandLine.ForCurrentProcess);
      generator.Generate();
    }
  }
}
