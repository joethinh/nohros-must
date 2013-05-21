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
        .WriteMember("name", (int) 20)
        .ToString();
      Assert.AreEqual("\"name\":20", json);

      json = new JsonStringBuilder()
        .WriteMember("name", (long) 64000)
        .ToString();
      Assert.AreEqual("\"name\":64000", json);
    }

    [Test]
    public void ShouldWriteJsonMemberName() {
      string json = new JsonStringBuilder()
        .WriteMemberName("name")
        .ToString();
      Assert.AreEqual("\"name\":", json);

      json = new JsonStringBuilder()
        .WriteMemberName("name")
        .WriteNumber(20)
        .ToString();
      Assert.AreEqual("\"name\":20", json);

      json = new JsonStringBuilder()
        .WriteMemberName("name")
        .WriteBeginObject()
        .WriteMember("name", "value")
        .WriteEndObject()
        .ToString();
      Assert.AreEqual("\"name\":{\"name\":\"value\"}", json);
    }

    [Test]
    public void ShouldEscapeJsonStructuralCharacters() {
      string escaped = JsonStringBuilder.Escape("\r\n\r\n");
      Assert.AreEqual("\\r\\n\\r\\n", escaped);

      escaped = JsonStringBuilder.Escape("escape:\r\n\r\n");
      Assert.AreEqual("escape:\\r\\n\\r\\n", escaped);

      escaped = JsonStringBuilder.Escape("escape:\b\f\r\n\t");
      Assert.AreEqual("escape:\\b\\f\\r\\n\\t", escaped);
    }

    [Test]
    public void ShouldEscapeunicodeCharacters() {
      string json = JsonStringBuilder.Escape("C:\\p");
      Assert.AreEqual("C:\\\\p", json);
    }
  }
}
