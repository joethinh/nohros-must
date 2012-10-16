using System;
using Nohros.Ruby;

namespace Nohros.Toolkit.RestQL
{
  public class Program
  {
    public static void Main(string[] args) {
      var factory = new ServiceFactory();
      IRubyService service = factory.CreateService(string.Empty);
      var logger = new LoggerRubyLogger(RestQLLogger.ForCurrentProcess.Logger);
      var ruby_service_host = new LoggerRubyServiceHost(logger);
      service.Start(ruby_service_host);
    }
  }
}
