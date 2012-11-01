using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Nohros.Extensions;
using Nohros.Ruby;
using Nohros.Ruby.Protocol;
using R = Nohros.Resources.StringResources;

namespace Nohros.RestQL
{
  public class Service : AbstractRubyService
  {
    const string kClassName = "Nohros.RestQL.Service";
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
      service_host_ = service_host;

      // Blocks the current thread and waits the service to stop.
      start_stop_service_event_.WaitOne();
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
        logger_.Error(
          string.Format(R.Log_MethodThrowsException, "OnMessage", kClassName), e);
        service_host_
          .SendError(request.Id, (int) StatusCode.kServerError, request.Sender,
            e);
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
            .SetName(query.Name)
            .SetResponse(result)
            .Build();
          service_host_
            .Send(request.Id, (int) MessageType.kQueryResponseMessage,
              response.ToByteArray(), request.Sender);
        } else {
          service_host_.SendError(request.Id, (int) StatusCode.kBadRequest,
            string.Format(Resources.Service_CannotProcessQuery_Name_Reason,
              query.Name, result), request.Sender);
        }
      } else {
        service_host_.SendError(request.Id, (int) StatusCode.kBadRequest,
          string.Format(Resources.Service_Arg_RequiredIsMissing_Name, "name"),
          request.Sender);
      }
    }
  }
}
