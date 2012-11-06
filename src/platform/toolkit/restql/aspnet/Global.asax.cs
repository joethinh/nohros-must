using System;
using System.Web;
using System.Web.Configuration;
using Nohros.Resources;
using ZMQ;

namespace Nohros.RestQL
{
  public class Global : HttpApplication
  {
    static readonly Context zmq_context_;

    #region .ctor
    static Global() {
      zmq_context_ = new Context(ZMQ.Context.DefaultIOThreads);
    }
    #endregion

    protected void Application_Start(object sender, EventArgs e) {
      string config_file_name =
        WebConfigurationManager.AppSettings[Strings.kConfigFileNameKey];
      string config_file_path = Server.MapPath(config_file_name);
      Settings settings = new Settings.Loader()
        .Load(config_file_path, Strings.kConfigRootNodeName);

      var factory = new HttpQueryApplicationFactory(settings);
      factory.ConfigureLogger();

      // log any unhandled exception that may occur for futher investigation.
      AppDomain.CurrentDomain.UnhandledException +=
        (obj, args) =>
          HttpQueryLogger.ForCurrentProcess.Error(
            string.Format(StringResources.Log_ThrowsException, "Application"),
            (System.Exception) args.ExceptionObject);

      HttpQueryApplication app = factory.CreateQueryApplication();
      Application[Strings.kApplicationKey] = app;
      app.Start();
    }

    protected void Session_Start(object sender, EventArgs e) {
    }

    protected void Application_BeginRequest(object sender, EventArgs e) {
    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e) {
    }

    protected void Application_Error(object sender, EventArgs e) {
    }

    protected void Session_End(object sender, EventArgs e) {
    }

    protected void Application_End(object sender, EventArgs e) {
      var app = Application[Strings.kApplicationKey] as HttpQueryApplication;
      if (app != null) {
        app.Stop();
        app.Dispose();
      }
      zmq_context_.Dispose();
    }
  }
}
