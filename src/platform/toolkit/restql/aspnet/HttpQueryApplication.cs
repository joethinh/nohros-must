using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Linq;
using Google.ProtocolBuffers;
using Nohros.Ruby;
using Nohros.Concurrent;
using Nohros.Resources;
using Nohros.Ruby.Extensions;
using Nohros.Ruby.Protocol;
using Nohros.Ruby.Protocol.Control;
using ZMQ;
using QueryFuture =
  Nohros.Concurrent.SettableFuture<Nohros.RestQL.HttpQueryResponse>;
using AsyncResponseMap =
  System.Collections.Generic.Dictionary
    <int,
      System.Tuple
        <System.AsyncCallback,
          Nohros.Concurrent.SettableFuture
            <Nohros.RestQL.HttpQueryResponse>>>;

namespace Nohros.RestQL
{
  public class HttpQueryApplication : IDisposable
  {
    const string kClassName = "Nohros.RestQL.HttpQueryApplication";
    readonly Context context_;
    readonly AsyncResponseMap futures_;
    readonly HttpQueryLogger logger_;
    readonly IThreadFactory receiver_thread_factory_;
    readonly ISettings settings_;
    readonly Socket socket_;
    Thread receiver_thread_;
    int request_id_;
    bool running_;

    #region .ctor
    public HttpQueryApplication(ISettings settings, Context context,
      IThreadFactory receiver_thread_factory) {
      settings_ = settings;
      socket_ = context.Socket(SocketType.DEALER);
      receiver_thread_factory_ = receiver_thread_factory;
      context_ = context;
      running_ = false;
      futures_ = new AsyncResponseMap();
      request_id_ = 1;
      logger_ = HttpQueryLogger.ForCurrentProcess;
    }
    #endregion

    public void Dispose() {
      Stop();
      context_.Dispose();
    }

    public IFuture<HttpQueryResponse> ProcessQuery(string name,
      IDictionary<string, string> options, AsyncCallback callback) {
      return ProcessQuery(name, options, callback, null);
    }

    public IFuture<HttpQueryResponse> ProcessQuery(string name,
      IDictionary<string, string> options, AsyncCallback callback, object state) {
      QueryRequestMessage request = new QueryRequestMessage.Builder()
        .SetName(name)
        .AddRangeOptions(GetQueryOptions(options))
        .Build();

      RubyMessagePacket packet = GetMessagePacket(GetNextRequestId(),
        request.ToByteString());
      try {
        QueryFuture future = new SettableFuture<HttpQueryResponse>(state);
        Tuple<AsyncCallback, QueryFuture> tuple = Tuple.Create(callback, future);
        futures_.Add(request_id_, tuple);

        // Send the request and wait for the response. The request should
        // follow the REQ/REP pattern, which contains the following parts:
        //   1. [EMPTY FRAME]
        //   2. [MESSAGE]
        // 
        socket_.SendMore();
        socket_.Send(packet.ToByteArray());
        return future;
      } catch (ZMQ.Exception zmqe) {
        return
          Futures.ImmediateFuture(
            GetExceptionResponse(name, HttpStatusCode.InternalServerError, zmqe));
      } catch (System.Exception e) {
        return
          Futures.ImmediateFuture(
            GetExceptionResponse(name, HttpStatusCode.InternalServerError, e));
      }
    }

    HttpQueryResponse GetExceptionResponse(string name, HttpStatusCode status,
      System.Exception exception) {
      return new HttpQueryResponse {
        Name = name,
        Response = exception.Message,
        StatusCode = status
      };
    }

    void ProcessResponse(byte[] response) {
      try {
        var packet = RubyMessagePacket.ParseFrom(response);
        Tuple<AsyncCallback, QueryFuture> tuple;
        int request_id = packet.Message.Id.ToByteArray().AsInt();
        if (futures_.TryGetValue(request_id, out tuple)) {
          SettableFuture<HttpQueryResponse> future = tuple.Item2;
          switch (packet.Message.Type) {
            case (int) MessageType.kQueryResponseMessage:
              future.Set(GetQueryResponse(packet.Message), false);
              break;

            case (int) NodeMessageType.kNodeError:
              future.Set(GetError(packet.Message), false);
              break;
          }
          tuple.Item1(tuple.Item2);
          futures_.Remove(request_id_);
        }
      } catch (System.Exception exception) {
        logger_.Error(
          string.Format(StringResources.Log_MethodThrowsException,
            "ProcessResponse", kClassName), exception);
      }
    }

    HttpQueryResponse GetError(RubyMessage error) {
      ErrorMessage message =
        ErrorMessage.ParseFrom(error.Message.ToByteArray());
      int last_error_code = 400;
      foreach (ExceptionMessage e in message.ErrorsList) {
        logger_.Error(string.Format(Resources.Query_ErrorResponse_Name_Trace,
          e.Message, GetBacktrace(e)));
        last_error_code = e.Code;
      }
      return new HttpQueryResponse {
        Name = "Error",
        Response = Resources.Http_InternalServerError,
        StatusCode = (HttpStatusCode) last_error_code
      };
    }

    public string GetBacktrace(ExceptionMessage e) {
      var q = from pair in e.DataList
              where pair.Key == "backtrace"
              select pair.Value;
      return (q.FirstOrDefault() ?? string.Empty);
    }

    HttpQueryResponse GetQueryResponse(RubyMessage response) {
      QueryResponseMessage message =
        QueryResponseMessage.ParseFrom(response.Message.ToByteArray());
      return new HttpQueryResponse {
        Name = message.Name,
        Response = message.Response,
        StatusCode = HttpStatusCode.OK
      };
    }

    void GetResponse() {
      while (running_) {
        Queue<byte[]> parts = socket_.RecvAll();
        if (parts.Count != 2) {
          // The response should follow the REQ/REP pattern, which contains
          // the following parts.
          //   1. [EMPTY FRAME]
          //   2. [MESSAGE]
          //
          logger_.Error(Resources.log_received_too_may_parts);
          socket_.RecvAll();
        } else {
          parts.Dequeue();
          ProcessResponse(parts.Dequeue());
        }
      }
    }

    RubyMessage GetMessage(int id, ByteString message) {
      return new RubyMessage.Builder()
        .SetId(ByteString.CopyFrom(id.AsBytes()))
        .SetAckType(RubyMessage.Types.AckType.kRubyNoAck)
        .SetType((int) MessageType.kQueryRequestMessage)
        .SetToken("query-request-message")
        .SetMessage(message)
        .Build();
    }

    int GetNextRequestId() {
      return Interlocked.Increment(ref request_id_);
    }

    RubyMessagePacket GetMessagePacket(int id, ByteString message) {
      RubyMessage msg = GetMessage(id, message);
      RubyMessageHeader header = new RubyMessageHeader.Builder()
        .SetId(msg.Id)
        .SetSize(msg.SerializedSize)
        .Build();
      return new RubyMessagePacket.Builder()
        .SetMessage(msg)
        .SetHeader(header)
        .SetHeaderSize(header.SerializedSize)
        .SetSize(header.SerializedSize + header.Size)
        .Build();
    }

    IEnumerable<QueryOptionsProto> GetQueryOptions(
      IDictionary<string, string> options) {
      int i = 0;
      QueryOptionsProto[] list = new QueryOptionsProto[options.Count];
      foreach (KeyValuePair<string, string> key_value_pair in options) {
        list[i++] = new QueryOptionsProto.Builder()
          .SetName(key_value_pair.Key)
          .SetValue(key_value_pair.Value)
          .Build();
      }
      return list;
    }

    public void Start() {
      socket_.Connect(Transport.TCP, settings_.QueryServerAddress);
      receiver_thread_ = receiver_thread_factory_
        .CreateThread(GetResponse);
      running_ = true;
      receiver_thread_.Start();
    }

    public void Stop() {
      futures_.Clear();
      if (receiver_thread_ != null) {
        // forces the socket to close.
        socket_.Dispose();
        receiver_thread_.Join();
      }
    }

    public ISettings Settings {
      get { return settings_; }
    }
  }
}
