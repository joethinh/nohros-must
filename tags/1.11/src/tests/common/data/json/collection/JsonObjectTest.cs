using System;
using NUnit.Framework;

namespace Nohros.Data.Json
{
  public class JsonObjectTest
  {
    [Test]
    public void ShouldCountTheNumberOfMembers() {
      var obj = new JsonObject();
      Assert.That(obj.Count, Is.EqualTo(0));

      obj = new JsonObject(10);
      Assert.That(obj.Count, Is.EqualTo(0));

      var member = new JsonObject.JsonMember("name", new JsonString("value"));
      obj = new JsonObject(new[] {member});
      Assert.That(obj.Count, Is.EqualTo(1));

      obj.Add(member);
      Assert.That(obj.Count, Is.EqualTo(2));
    }

    [Test]
    public void ShouldSerializeAsJsonObject() {
      var obj = new JsonObject();
      var member = new JsonObject.JsonMember("name", new JsonString("value"));

      Assert.That(obj.AsJson(), Is.EqualTo("{}"));

      obj.Add(member);
      Assert.That(obj.AsJson(),
        Is.EqualTo("{\"name\":\"value\"}"));

      obj.Add(member);
      Assert.That(obj.AsJson(),
        Is.EqualTo("{\"name\":\"value\",\"name\":\"value\"}"));
    }

    [Test]
    public void ShouldReturnObjectMembers() {
      var obj = new JsonObject();
      var member = new JsonObject.JsonMember("name", "value");
      obj.Add(member);
      obj.Add(member);

      var members = obj.Value;
      Assert.That(members.Length, Is.EqualTo(2));
      Assert.That(members[0], Is.EqualTo(member));
    }
  }
}