using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Google.ProtocolBuffers;
using Nohros.Concurrent;
using Nohros.Ruby.Protocol;
using ZMQ;

namespace Nohros.Toolkit.RestQL
{
  public class HttpQueryApplication : IDisposable
  {
    readonly Context context_;
    readonly IThreadFactory receiver_thread_factory_;
    readonly ISettings settings_;
    readonly Socket socket_;
    Thread receiver_thread_;
    bool running_;

    #region .ctor
    public HttpQueryApplication(ISettings settings, Context context,
      IThreadFactory receiver_thread_factory) {
      settings_ = settings;
      socket_ = context.Socket(SocketType.REQ);
      receiver_thread_factory_ = receiver_thread_factory;
      context_ = context;
      running_ = false;
    }
    #endregion

    public void Dispose() {
      Stop();
      context_.Dispose();
    }

    public IFuture<HttpQueryResponse> ProcessQuery(string name,
      IDictionary<string, string> options) {
      QueryRequestMessage request = new QueryRequestMessage.Builder()
        .SetName(name)
        .AddRangeOptions(GetQueryOptions(options))
        .Build();

      RubyMessagePacket packet = GetMessagePacket(request.ToByteString());
      try {
        // send the request and wait for the response.
        socket_.Send(packet.ToByteArray());
        var future = SettableFuture<HttpQueryResponse>.Create();
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
      return new HttpQueryResponse
      {
        Name = name,
        Response = exception.Message,
        StatusCode = status
      };
    }

    HttpStatusCode ProcessResponse(byte[] response) {
      string result = Encoding.Unicode.GetString(response);
      return HttpStatusCode.OK;
    }

    void GetResponse() {
      while (running_) {
        byte[] response = socket_.Recv();
        ProcessResponse(response);
      }
    }

    RubyMessage GetMessage(ByteString message) {
      return new RubyMessage.Builder()
        .SetId(0)
        .SetAckType(RubyMessage.Types.AckType.kRubyNoAck)
        .SetType((int) MessageType.kQueryRequestMessage)
        .SetToken("query-request-message")
        .SetMessage(message)
        .Build();
    }

    RubyMessagePacket GetMessagePacket(ByteString message) {
      RubyMessage msg = GetMessage(message);
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
