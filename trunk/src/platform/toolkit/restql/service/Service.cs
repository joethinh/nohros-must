using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Nohros.Ruby;
using Nohros.Ruby.Protocol;
using R = Nohros.Resources.StringResources;

namespace Nohros.Toolkit.RestQL
{
  public class Service : AbstractRubyService
  {
    const string kClassName = "Nohros.Toolkit.RestQL.Service";
    readonly RestQLLogger logger_;

    readonly IQueryServer server_;
    readonly ManualResetEvent start_stop_service_event_;
    IRubyServiceHost service_host_;
    Thread start_thread_;

    #region .ctor
    public Service(IQueryServer server) {
      server_ = server;
      start_stop_service_event_ = new ManualResetEvent(false);
      logger_ = RestQLLogger.ForCurrentProcess;
    }
    #endregion

    public override void Start(IRubyServiceHost service_host) {
      logger_.Logger = service_host.Logger;
      start_thread_ = Thread.CurrentThread;
      start_stop_service_event_.WaitOne();
      service_host_ = service_host;
    }

    public override void Stop(IRubyMessage message) {
      start_stop_service_event_.Set();
      if (start_thread_ != null) {
        start_thread_.Join(30*1000);
      }
    }

    public override void OnMessage(IRubyMessage request) {
      try {
        switch (request.Type) {
          case (int) MessageType.kQueryRequestMessage:
            QueryRequestMessage query =
              QueryRequestMessage.ParseFrom(request.Message);
            ProcessQuery(request, query);
            break;
        }
      } catch (Exception e) {
        // TODO: Add specific exception handling.
        logger_.Error(string.Format(R.Log_MethodThrowsException, kClassName), e);
        service_host_
          .SendError(0, (int) StatusCode.kServerError, e);
      }
    }

    void ProcessQuery(IRubyMessage request, QueryRequestMessage query) {
      if (query.HasName) {
        string result;
        IDictionary<string, string> parameters = query.OptionsList
          .ToDictionary(proto => proto.Name, proto => proto.Value);
        bool processed = server_
          .QueryProcessor
          .Process(query.Name, parameters, out result);
        if (processed) {
          QueryResponseMessage response = new QueryResponseMessage.Builder()
            .SetStatusCode(QueryRequestStatus.kOk)
            .SetName(query.Name)
            .SetResponse(result)
            .BuildPartial();
          service_host_
            .Send(request.Id, (int) MessageType.kQueryResponseMessage,
              response.ToByteArray());
        }
      }
    }
  }
}
