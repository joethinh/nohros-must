using System;
using NUnit.Framework;
using Nohros.Data.Json;

namespace Nohros.Common.data.json
{
  [TestFixture]
  public class JsonStringBuilderTest
  {
    [Test]
    public void ShouldEscapeLastWrittenToken() {
      string json = new JsonStringBuilder()
        .WriteBeginArray()
        .Escape()
        .ToString();
      Assert.AreEqual("[", json);

      json = new JsonStringBuilder()
        .WriteBeginObject()
        .WriteMember("key", "value")
        .Escape()
        .WriteEndObject()
        .ToString();
      Assert.AreEqual("{\"key\":\"value\"}", json);

      json = new JsonStringBuilder()
        .WriteBeginObject()
        .WriteMember("key", "value-with-CRLF\r\n")
        .Escape()
        .WriteEndObject()
        .ToString();
      Assert.AreEqual("{\"key\":\"value-with-CRLF\\r\\n\"}", json);
    }

    [Test]
    public void ShouldWriteJsonMember() {
      string json = new JsonStringBuilder()
        .WriteMember("name", "key")
        .ToString();
      Assert.AreEqual(json, "\"name\":\"key\"");

      json = new JsonStringBuilder()
        .WriteMember("name", (double) 20.1)
        .ToString();
      Assert.AreEqual("\"name\":20.1", json);

      json = new JsonStringBuilder()
        .WriteMember("name", (int)20)
        .ToString();
      Assert.AreEqual("\"name\":20", json);

      json = new JsonStringBuilder()
        .WriteMember("name", (long) 64000)
        .ToString();
      Assert.AreEqual("\"name\":64000", json);
    }
  }
}
