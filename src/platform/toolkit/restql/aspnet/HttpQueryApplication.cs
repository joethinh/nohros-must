using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Google.ProtocolBuffers;
using Nohros.Ruby.Protocol;
using ZMQ;

namespace Nohros.Toolkit.RestQL
{
  public class HttpQueryApplication
  {
    readonly Context context_;
    readonly ISettings settings_;
    readonly Socket socket_;

    #region .ctor
    public HttpQueryApplication(ISettings settings, Context context) {
      settings_ = settings;
      socket_ = context.Socket(SocketType.REQ);
    }
    #endregion

    public HttpStatusCode ProcessQuery(string name,
      IDictionary<string, string> options,
      out string result) {
      QueryRequestMessage request = new QueryRequestMessage.Builder()
        .SetName(name)
        .AddRangeOptions(GetQueryOptions(options))
        .Build();

      RubyMessagePacket packet = GetMessagePacket(request.ToByteString());
      try {
        // send the request and wait for the response.
        socket_.Send(packet.ToByteArray());
        byte[] response = socket_.Recv(settings_.ResponseTimeout);
        if (response != null) {
          return ProcessResponse(response, out result);
        }
        result = string.Empty;
        return HttpStatusCode.RequestTimeout;
      } catch (ZMQ.Exception zmqe) {
        result = zmqe.Message;
        return HttpStatusCode.InternalServerError;
      } catch (System.Exception e) {
        result = string.Empty;
        return HttpStatusCode.InternalServerError;
      }
    }

    HttpStatusCode ProcessResponse(byte[] response, out string result) {
      result = Encoding.Unicode.GetString(response);
      return HttpStatusCode.OK;
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

    public void Run() {
      socket_.Connect(Transport.TCP, settings_.QueryServerAddress);
    }

    public ISettings Settings {
      get { return settings_; }
    }
  }
}
