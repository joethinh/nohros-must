using System;
using System.Threading;
using Nohros.Ruby;
using Nohros.Ruby.Protocol;

namespace Nohros.Toolkit.RestQL
{
  public class Service : AbstractRubyService
  {
    readonly IQueryServer server_;
    readonly ManualResetEvent start_stop_service_event_;
    Thread start_thread_;

    #region .ctor
    public Service(IQueryServer server) {
      server_ = server;
      start_stop_service_event_ = new ManualResetEvent(false);
    }
    #endregion

    public override void Start(IRubyServiceHost service_host) {
      start_thread_ = Thread.CurrentThread;
      start_stop_service_event_.WaitOne();
    }

    public override void Stop(IRubyMessage message) {
      start_stop_service_event_.Set();
      if (start_thread_ != null) {
        start_thread_.Join(30*1000);
      }
    }

    public override void OnMessage(IRubyMessage message) {
    }
  }
}
